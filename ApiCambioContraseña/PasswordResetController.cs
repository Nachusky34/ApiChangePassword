using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCambioContraseña
{
    [ApiController]
    [Route("password")]
    [Authorize]
    public class PasswordResetController : ControllerBase
    {
        private PasswordResetRepository passwordResetRepository;
        private EmailService emailService;
        private SmsService smsService;
        private TokenGenerator tokenGenerator;
        private string URL = "https://da11-77-211-4-183.ngrok-free.app";

        public PasswordResetController(IConfiguration config)
        {
            passwordResetRepository = new PasswordResetRepository();
            tokenGenerator = new TokenGenerator(config);
            emailService = new EmailService();
            smsService = new SmsService();
        }

        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest param)
        {
            await passwordResetRepository.ResetPassword(param.Email, param.Password);
            return Ok(new { message = "Password reset corectly" });
        }

        [HttpGet("link-email")]
        [AllowAnonymous]
        public async Task<ActionResult> SendLink(string email)
        {
            if (email is null)
            {
                return BadRequest("Body needs 1 param {email}");                
            }

            var token = tokenGenerator.BuildToken(email);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var encodedEmail = System.Web.HttpUtility.UrlEncode(email);
            var link = $"{URL}?token={encodedToken}&email={encodedEmail}";

            await emailService.SendEmailAsync(email, "Password reset", $"Click here to reset the password: {link}");

            return Ok("Correo enviado.");
        }

        [HttpGet("link-sms")]
        [AllowAnonymous]
        public async Task<ActionResult> SendLinkSms(string number)
        {
            if (number is null)
            {
                return BadRequest("Body needs 1 param {number}");
            }

            string email = await passwordResetRepository.GetEmailFromNumber(number);

            var token = tokenGenerator.BuildToken(email);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var encodedEmail = System.Web.HttpUtility.UrlEncode(email);
            var link = $"{URL}?token={encodedToken}&email={encodedEmail}";

            string sid = smsService.EnviarSms("+34" + number, $"Click here to reset the password: {link}");

            return Ok("Sms enviado.");
        }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
