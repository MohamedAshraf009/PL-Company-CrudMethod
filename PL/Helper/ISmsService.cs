using PL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace PL.Helper
{
	public interface ISmsService
	{
		MessageResource SendSms(SmsMessage smsMessage);
	}
}
