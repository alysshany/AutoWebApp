using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.HRPages
{
    public class ViewAllStaffModel : PageModel
    {
        public static List<ServiceStaff> staffs = new List<ServiceStaff>();
        public static List<Position> positions = new List<Position>();

        public void OnGet()
        {
            staffs.Clear();
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
                    string sql = "SELECT * FROM [Service staff]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ServiceStaff staff = new ServiceStaff();
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

                                staffs.Add(staff);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
