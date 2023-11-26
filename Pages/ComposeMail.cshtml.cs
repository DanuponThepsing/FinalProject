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
        }

        public void OnPost()
        {
            emailInfo.EmailSubject = Request.Form["EmailSubject"];
            emailInfo.EmailMessage = Request.Form["EmailMessage"];
            emailInfo.EmailSender = Request.Form["EmailSender"];
            emailInfo.EmailReceiver = Request.Form["EmailReceiver"];

            if (emailInfo.EmailSubject.Length == 0 || emailInfo.EmailMessage.Length == 0 ||
                emailInfo.EmailSender.Length == 0 || emailInfo.EmailReceiver.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:bumailsystem.database.windows.net,1433;Initial Catalog=bumailsystem;Persist Security Info=False;User ID=bumailsystem;Password=@bumail1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO emails (EmailSubject, EmailMessage, EmailSender, EmailReceiver) " +
                                 "VALUES (@EmailSubject, @EmailMessage, @EmailSender, @EmailReceiver);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailSubject", emailInfo.EmailSubject);
                        command.Parameters.AddWithValue("@EmailMessage", emailInfo.EmailMessage);
                        command.Parameters.AddWithValue("@EmailSender", emailInfo.EmailSender);
                        command.Parameters.AddWithValue("@EmailReceiver", emailInfo.EmailReceiver);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            emailInfo.EmailSubject = "";
            emailInfo.EmailMessage = "";
            emailInfo.EmailSender = "";
            emailInfo.EmailReceiver = "";
            successMessage = "Send Successfully";

            Response.Redirect("/Page/ComposeMail");
        }
    }
}