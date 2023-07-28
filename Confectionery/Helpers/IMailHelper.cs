using Confectionery.Common;

namespace Confectionery.Helpers
{
	public interface IMailHelper
	{
		Response SendMail(string toName, string toEmail, string subject, string body);
	}
}
