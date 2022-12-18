using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class EditingDriverModel : PageModel
    {
        public static List<DrivingCategory> categories = new List<DrivingCategory>();
        public static List<DrivingCategory> driverCategories = new List<DrivingCategory>();
        public static Driver driver = new Driver();
        public string errorMessage = "";

        public void OnGet()
        {
            string id = Request.Query["ID"];
            categories.Clear();
            driverCategories.Clear();
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [Driving categories]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DrivingCategory category = new DrivingCategory();
                                category.Id = "" + reader.GetInt32(0);
                                category.Category = reader.GetString(1);
                                categories.Add(category);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [Driving categories] WHERE DrivingCategory_Id IN (SELECT DrivingCategory_Id FROM Licenses WHERE Driver_Id = @Driver_Id)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Driver_Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DrivingCategory category = new DrivingCategory();
                                category.Id = "" + reader.GetInt32(0);
                                category.Category = reader.GetString(1);
                                driverCategories.Add(category);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Drivers WHERE Driver_Id = @Driver_Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Driver_Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                driver.Id = "" + reader.GetInt32(0);
                                driver.Surname = reader.GetString(1);
                                driver.Name = reader.GetString(2);
                                driver.MiddleName = reader.GetString(3);
                                driver.Birthday = reader.GetString(4);
                                driver.AdmissionYear = "" + reader.GetInt32(5);
                                driver.Experience = "" + reader.GetInt32(6);
                                driver.Address = reader.GetString(7);
                                driver.Telephone = reader.GetString(8);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void OnPost()
        {
            driver.Surname = Request.Form["surname"];
            driver.Name = Request.Form["name"];
            driver.MiddleName = Request.Form["middleName"];
            driver.Birthday = Request.Form["birthday"];
            driver.AdmissionYear = Request.Form["admissionYear"];
            driver.Experience = Request.Form["experience"];
            driver.Address = Request.Form["address"];
            driver.Telephone = Request.Form["telephone"];
            string[] newCategories = Request.Form["items"];

            if (newCategories.Length > 0 && driver.Surname.Length > 0 && driver.Name.Length > 0 && driver.MiddleName.Length > 0 && driver.Birthday.Length > 0 && driver.AdmissionYear.Length > 0 && driver.Experience.Length > 0 && driver.Telephone.Length == 11 && driver.Address.Length > 0)
            {
                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Licenses WHERE Driver_Id = @Driver_Id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Driver_Id", Convert.ToInt32(driver.Id));
                            command.ExecuteNonQuery();
                        }
                    }

                    foreach (var item in newCategories)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "INSERT INTO Licenses " +
                                        "(Driver_Id, DrivingCategory_Id) VALUES " +
                                        "(@Driver_Id, @DrivingCategory_Id);";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Driver_Id", Convert.ToInt32(driver.Id));
                                command.Parameters.AddWithValue("@DrivingCategory_Id", Convert.ToInt32(item));
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE Drivers " +
                                    "SET Surname=@Surname, Name=@Name, MiddleName=@MiddleName, Birthday = @Birthday, AdmissionYear = @AdmissionYear, Experience = @Experience, Telephone = @Telephone, Address = @Address " +
                                    "WHERE Driver_Id = @id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", Convert.ToInt32(driver.Id));
                            command.Parameters.AddWithValue("@Surname", driver.Surname);
                            command.Parameters.AddWithValue("@Name", driver.Name);
                            command.Parameters.AddWithValue("@MiddleName", driver.MiddleName);
                            command.Parameters.AddWithValue("@Birthday", driver.Birthday);
                            command.Parameters.AddWithValue("@AdmissionYear", Convert.ToInt32(driver.AdmissionYear));
                            command.Parameters.AddWithValue("@Experience", Convert.ToInt32(driver.Experience));
                            command.Parameters.AddWithValue("@Telephone", driver.Telephone);
                            command.Parameters.AddWithValue("@Address", driver.Address);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex) { }

            }
            else
            {
                errorMessage = "Some fields are empty";
                return;
            }

            Response.Redirect("/HRPages/ViewAllDrivers");
        }
    }
}
