using Demo.DAL.Models;
using System.Net.Mail;
using System.Net;
using System;

namespace Demo.PL.Helper
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			using (var client = new SmtpClient("smtp.gmail.com", 587))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential("ss7377991@gmail.com@gmail.com", "reht ysec zfhw gyfu");

				var mail = new MailMessage("ss7377991@gmail.com", email.To, email.Subject, email.Body);

				try
				{
					client.Send(mail);
				}
				catch (Exception ex)
				{
					
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}
