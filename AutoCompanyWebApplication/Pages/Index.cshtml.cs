using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using AutoCompanyWebApplication.Classes;

namespace AutoCompanyWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static List<Account> accounts = new List<Account>();
        public static List<User> users = new List<User>();
        public string errorMessage = "";
        public string successMessage = "";
        public string navigateMessage = "";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Accounts";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();
                                account.Id = "" + reader.GetInt32(0);
                                account.Login = reader.GetString(1);
                                account.Password = reader.GetString(2);
                                account.UserId = "" + reader.GetInt32(3);

                                accounts.Add(account);
                            }
                        }
                    }
                }
                connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();
                                user.Id = "" + reader.GetInt32(0);
                                user.Name = reader.GetString(1);
                                user.Surname = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                user.Telephone = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                user.Address = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                user.RoleId = "" + reader.GetInt32(5);

                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { string s = ex.Message; }
        }

        public void OnPost()
        {
            string login = Request.Form["login"];
            string password = Request.Form["password"];

            if (login.Length != 0 && password.Length != 0)
            {
                foreach (Account account in accounts)
                {
                    if (account.Login == login && account.Password == password)
                    {
                        CurrentAccount.CurrAccount = account;
                    }
                }
                if (CurrentAccount.CurrAccount != null)
                {
                    foreach (User user in users)
                    {
                        if (CurrentAccount.CurrAccount.UserId == user.Id)
                        {
                            CurrentUser.CurrUser = user;
                        }
                    }
                    switch (CurrentUser.CurrUser.RoleId)
                    {
                        case "1":
                            navigateMessage = "TechnicalSpecialist";
                            break;
                        case "2":
                            navigateMessage = "Driver";
                            break;
                        case "3":
                            navigateMessage = "HR";
                            break;
                        case "4":
                            navigateMessage = "Dispatcher";
                            break;
                    }
                }
                else
                {
                    errorMessage = "Login or password is not correct";
                    return;
                }
            }
            else
            {
                errorMessage = "Some fields are empty";
                return;
            }
        }
    }
}