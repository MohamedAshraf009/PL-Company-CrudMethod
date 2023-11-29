using Microsoft.Extensions.Options;
using PL.eSettings;
using PL.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PL.Helper
{
	public class SmsService : ISmsService
	{
		private readonly TwoilioSetting options;

		public SmsService(IOptions<TwoilioSetting> _options)
		{
			options = _options.Value;
		}

		public MessageResource SendSms(SmsMessage smsMessage)
		{
			TwilioClient.Init(options.AccountSID,options.AuthToken);

			var message = MessageResource.Create(
			body: smsMessage.body,
			from: new Twilio.Types.PhoneNumber(options.TwilioPhoneNumber),
			to: smsMessage.PhoneNumber
			);
			return message;
		}
	}
}
