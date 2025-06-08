using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ApiCambioContraseña
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService()
        {
            _accountSid = "ACef8a9fc8ebff99f4a26ed29ce0599abd";
            _authToken = "6ac94a07e7e36a55fdd18aaebcdf01ff";
            _fromPhoneNumber = "+18144975154";

            TwilioClient.Init(_accountSid, _authToken);
        }

        public string EnviarSms(string toPhoneNumber, string mensaje)
        {
            var message = MessageResource.Create(
                body: mensaje,
                from: new PhoneNumber(_fromPhoneNumber),
                to: new PhoneNumber(toPhoneNumber)
            );

            return message.Sid;
        }
    }

}
