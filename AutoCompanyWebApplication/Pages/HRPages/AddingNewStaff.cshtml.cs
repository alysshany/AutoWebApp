using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class AddingNewStaffModel : PageModel
    {
        public static List<Position> positions = new List<Position>();
        public string errorMessage = "";

        public void OnGet()
        {
            positions.Clear();
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Positions";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Position position = new Position();
                                position.Id = "" + reader.GetInt32(0);
                                position.Title = reader.GetString(1);
                                positions.Add(position);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void OnPost()
        {
            string items = Request.Form["items"];
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
                string positionId = "";
                for (var i = 0; i < items.Length; i++)
                {
                    foreach (var category in positions)
                    {
                        if (category.Title == items)
                        {
                            positionId = (category.Id);
                        }
                    }
                }

                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO [Service staff] " +
                                    "(Surname, Name, MiddleName, Birthday, AdmissionYear, Experience, Telephone, Address, Position_Id) VALUES " +
                                    "(@Surname, @Name, @MiddleName, @Birthday, @AdmissionYear, @Experience, @Telephone, @Address, @Position_Id);";
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
                            command.Parameters.AddWithValue("@Position_Id", Convert.ToInt32(positionId));

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

            Response.Redirect("/HRPages/ViewAllStaff");
        }
    }
}
