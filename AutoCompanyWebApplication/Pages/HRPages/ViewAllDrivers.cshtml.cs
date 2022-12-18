using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class ViewAllDriversModel : PageModel
    {
        public static List<Driver> drivers = new List<Driver>();

        public void OnGet()
        {
            drivers.Clear();
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Drivers";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Driver driver = new Driver();
                                driver.Id = "" + reader.GetInt32(0);
                                driver.Surname = reader.GetString(1);
                                driver.Name = reader.GetString(2);
                                driver.MiddleName = reader.GetString(3);
                                driver.Birthday = reader.GetString(4);
                                driver.AdmissionYear = "" + reader.GetInt32(5);
                                driver.Experience = "" + reader.GetInt32(6);
                                driver.Address = reader.GetString(7);
                                driver.Telephone = reader.GetString(8);

                                drivers.Add(driver);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
