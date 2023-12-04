using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
	public class ComposeMailModel : PageModel
	{
		public EmailInfo emailInfo = new EmailInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
			// Initialization logic for GET requests
		}

		public void OnPost()
		{
			// Assuming you are using ASP.NET Core Identity for authentication
			string currentUserUsername = User.Identity.Name;

			emailInfo.EmailReceiver = Request.Form["EmailReceiver"];
			emailInfo.EmailSubject = Request.Form["EmailSubject"];
			emailInfo.EmailMessage = Request.Form["EmailMessage"];
			emailInfo.EmailSender = currentUserUsername;

			if (string.IsNullOrEmpty(emailInfo.EmailReceiver) || string.IsNullOrEmpty(emailInfo.EmailSubject) ||
				string.IsNullOrEmpty(emailInfo.EmailMessage))
			{
				errorMessage = "All the fields are required";
				return;
			}

			try
			{

				SaveEmailToDatabase();

				emailInfo.EmailReceiver = "";
				emailInfo.EmailSubject = "";
				emailInfo.EmailMessage = "";
			}
			catch (Exception ex)
			{
				errorMessage = "An error occurred while sending the email: " + ex.Message;
			}
		}

		private void SaveEmailToDatabase()
		{
			try
			{
				String connectionString = "Server=tcp:buemailsystem.database.windows.net,1433;Initial Catalog=BUEmail;Persist Security Info=False;User ID=bumailsystem;Password=@bumail1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO emails (EmailReceiver, EmailSubject, EmailMessage, EmailSender, EmailIsRead) " +
								 "VALUES (@EmailReceiver, @EmailSubject, @EmailMessage, @EmailSender, 0);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@EmailReceiver", emailInfo.EmailReceiver);
						command.Parameters.AddWithValue("@EmailSubject", emailInfo.EmailSubject);
						command.Parameters.AddWithValue("@EmailMessage", emailInfo.EmailMessage);
						command.Parameters.AddWithValue("@EmailSender", emailInfo.EmailSender);

						command.ExecuteNonQuery();
					}
                }
                successMessage = "Email sent successfully";
            }
			catch (Exception ex)
			{
				errorMessage = "Please Login First";
			}
		}
	}
}