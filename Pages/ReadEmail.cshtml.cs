using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
	public class ReadEmailModel : PageModel
	{
		public EmailInfo emailInfo = new EmailInfo();

		public void OnGet()
		{
			String emailId = Request.Query["emailid"];
			try
			{
				String connectionString = "Server=tcp:buemailsystem.database.windows.net,1433;Initial Catalog=BUEmail;Persist Security Info=False;User ID=bumailsystem;Password=@bumail1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM emails WHERE emailid=@emailid";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@emailid", emailId);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								emailInfo.EmailID = "" + reader.GetInt32(0);
								emailInfo.EmailSubject = reader.GetString(1);
								emailInfo.EmailMessage = reader.GetString(2);
								emailInfo.EmailIsRead = reader.GetString(4);
								emailInfo.EmailSender = reader.GetString(5);

								// Check if the email is unread and update the status to "1"
								if (emailInfo.EmailIsRead.Equals("0"))
								{
									// Update the database to mark the email as read
									UpdateEmail(emailId);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void UpdateEmail(string emailId)
		{
			try
			{
				String connectionString = "Server=tcp:buemailsystem.database.windows.net,1433;Initial Catalog=BUEmail;Persist Security Info=False;User ID=bumailsystem;Password=@bumail1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String updateSql = "UPDATE emails SET EmailIsRead = '1' WHERE EmailID = @EmailID";
					using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
					{
						updateCommand.Parameters.AddWithValue("@EmailID", emailId);
						updateCommand.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
