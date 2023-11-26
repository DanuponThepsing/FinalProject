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
				// Your email sending logic here
				// Replace the following line with your actual email sending code

				// For testing purposes, let's assume the email is successfully sent
				successMessage = "Email sent successfully";

				// Save the email to the database
				SaveEmailToDatabase();

				// Clear the form fields
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
				String connectionString = "Server=tcp:bumailsystem.database.windows.net,1433;Initial Catalog=bumailsystem;Persist Security Info=False;User ID=bumailsystem;Password=@bumail1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
			}
			catch (Exception ex)
			{
				errorMessage = "An error occurred while saving the email to the database: " + ex.Message;
			}
		}
	}
}