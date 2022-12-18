using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class EditingStaffModel : PageModel
    {
        public static List<Position> positions = new List<Position>();
        public static ServiceStaff staff = new ServiceStaff();
        public string errorMessage = "";

        public void OnGet()
        {
            string id = Request.Query["ID"];
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [Service staff] WHERE ServiceStaff_Id = @ServiceStaff_Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStaff_Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                staff.Id = "" + reader.GetInt32(0);
                                staff.Surname = reader.GetString(1);
                                staff.Name = reader.GetString(2);
                                staff.MiddleName = reader.GetString(3);
                                staff.Birthday = reader.GetString(4);
                                staff.AdmissionYear = "" + reader.GetInt32(5);
                                staff.Experience = "" + reader.GetInt32(6);
                                staff.Address = reader.GetString(7);
                                staff.Telephone = reader.GetString(8);
                                staff.PositionId = "" + reader.GetInt32(9);

                                foreach (var position in positions)
                                {
                                    if (position.Id == staff.PositionId)
                                    {
                                        staff.PositionTitle = position.Title;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

        }

        public void OnPost()
        {
            staff.Surname = Request.Form["surname"];
            staff.Name = Request.Form["name"];
            staff.MiddleName = Request.Form["middleName"];
            staff.Birthday = Request.Form["birthday"];
            staff.AdmissionYear = Request.Form["admissionYear"];
            staff.Experience = Request.Form["experience"];
            staff.Address = Request.Form["address"];
            staff.Telephone = Request.Form["telephone"];
            staff.PositionId = Request.Form["items"];

            if (staff.PositionId.Length > 0 && staff.Surname.Length > 0 && staff.Name.Length > 0 && staff.MiddleName.Length > 0 && staff.Birthday.Length > 0 && staff.AdmissionYear.Length > 0 && staff.Experience.Length > 0 && staff.Telephone.Length == 11 && staff.Address.Length > 0)
            {
                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE [Service staff] " +
                                    "SET Surname = @Surname, Name = @Name, MiddleName = @MiddleName, Birthday = @Birthday, AdmissionYear = @AdmissionYear, Experience = @Experience, Telephone = @Telephone, Address = @Address, Position_Id = @Position_Id " +
                                    "WHERE ServiceStaff_Id = @id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", Convert.ToInt32(staff.Id));
                            command.Parameters.AddWithValue("@Surname", staff.Surname);
                            command.Parameters.AddWithValue("@Name", staff.Name);
                            command.Parameters.AddWithValue("@MiddleName", staff.MiddleName);
                            command.Parameters.AddWithValue("@Birthday", staff.Birthday);
                            command.Parameters.AddWithValue("@AdmissionYear", Convert.ToInt32(staff.AdmissionYear));
                            command.Parameters.AddWithValue("@Experience", Convert.ToInt32(staff.Experience));
                            command.Parameters.AddWithValue("@Telephone", staff.Telephone);
                            command.Parameters.AddWithValue("@Address", staff.Address);
                            command.Parameters.AddWithValue("@Position_Id", Convert.ToInt32(staff.PositionId));

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
