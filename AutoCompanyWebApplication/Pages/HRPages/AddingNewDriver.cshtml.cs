using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class AddingNewDriverModel : PageModel
    {
        public static List<DrivingCategory> categories = new List<DrivingCategory>();
        public static string driverId;
        public string errorMessage = "";

        public void OnGet()
        {
            categories.Clear();
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
            }
            catch (Exception ex) { }
        }

        public void OnPost()
        {
            string[] items = Request.Form["items"];
            string surname = Request.Form["surname"];
            string name = Request.Form["name"];
            string middleName = Request.Form["middleName"];
            string birthday = Request.Form["birthday"];
            string admissionYear = Request.Form["admissionYear"];
            string experience = Request.Form["experience"];
            string telephone = Request.Form["telephone"];
            string address = Request.Form["address"];

            if (items.Length > 0 && surname.Length > 0 && name.Length > 0 && middleName.Length > 0 && birthday.Length > 0 && admissionYear.Length > 0 && experience.Length > 0 && telephone.Length == 11 && address.Length > 0 && Convert.ToInt32(admissionYear) > 0 && Convert.ToInt32(experience) >= 0)
            {
                string[] categoryId = new string[items.Length];
                for (var i = 0; i < items.Length; i++)
                {
                    foreach (var category in categories)
                    {
                        if (category.Category == items[i])
                        {
                            categoryId[i] = (category.Id);
                        }
                    }
                }

                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO Drivers " +
                                    "(Surname, Name, MiddleName, Birthday, AdmissionYear, Experience, Telephone, Address) VALUES " +
                                    "(@Surname, @Name, @MiddleName, @Birthday, @AdmissionYear, @Experience, @Telephone, @Address);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Surname", surname);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@MiddleName", middleName);
                            command.Parameters.AddWithValue("@Birthday", birthday);
                            command.Parameters.AddWithValue("@AdmissionYear", Convert.ToInt32(admissionYear));
                            command.Parameters.AddWithValue("@Experience", Convert.ToInt32(experience));
                            command.Parameters.AddWithValue("@Telephone", telephone);
                            command.Parameters.AddWithValue("@Address", address);

                            command.ExecuteNonQuery();
                        }
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT MAX(Driver_Id) FROM Drivers";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    driverId = "" + reader.GetInt32(0);
                                }
                            }
                        }
                    }
                    foreach (var item in categoryId)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "INSERT INTO Licenses " +
                                        "(Driver_Id, DrivingCategory_Id) VALUES " +
                                        "(@Driver_Id, @DrivingCategory_Id);";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Driver_Id", Convert.ToInt32(driverId));
                                command.Parameters.AddWithValue("@DrivingCategory_Id", Convert.ToInt32(item));
                                command.ExecuteNonQuery();
                            }
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
