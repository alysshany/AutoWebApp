using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using AutoCompanyWebApplication.Classes;

namespace AutoCompanyWebApplication.Pages.DispatcherPages
{
    public class AddingNewRouteModel : PageModel
    {
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static List<Driver> drivers = new List<Driver>();
        public string errorMessage = "";

        public void OnGet()
        {
            vehicles.Clear();
            drivers.Clear();
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Vehicles";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Vehicle vehicle = new Vehicle();
                                vehicle.Id = "" + reader.GetInt32(0);
                                vehicle.LicensePlate = reader.GetString(1);
                                vehicle.RunningTime = "" + reader.GetInt32(2);
                                vehicle.Mileage = "" + reader.GetInt32(3);
                                vehicle.AmountOfRepairings = "" + reader.GetInt32(4);
                                vehicle.LoadCapacity = reader.IsDBNull(5) ? "" : "" + reader.GetInt32(5); 
                                vehicle.Capacity = reader.IsDBNull(6) ? "" : "" + reader.GetInt32(6);
                                vehicle.ReleaseYear = "" + reader.GetInt32(7);
                                vehicle.CurrentlyUsed = "" + reader.GetBoolean(8);
                                vehicle.TechnicalStateId = "" + reader.GetInt32(9);
                                vehicle.VehicleTypeId = "" + reader.GetInt32(10);

                                if (vehicle.CurrentlyUsed == "False")
                                { vehicles.Add(vehicle); }
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Drivers WHERE Driver_Id NOT IN (SELECT Driver_Id FROM [Routes] WHERE ArrivalTime IS NULL)";
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

        public void OnPost()
        {
            string driver = Request.Form["drivers"];
            string vehicle = Request.Form["vehicles"];
            string title = Request.Form["title"];
            string checkOutTime = Request.Form["checkOutTime"];
            string arrivalTime = Request.Form["arrivalTime"];
            string destination = Request.Form["destination"];

            if (vehicle.Length > 0 && driver.Length > 0 && title.Length > 0 && checkOutTime.Length > 0 && arrivalTime.Length > 0 && destination.Length > 0)
            {
                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO [Routes] " +
                                    "(Title, CheckOutTime, ArrivalTime, Destination, Vehicle_Id, Driver_Id) VALUES " +
                                    "(@Title, @CheckOutTime, @ArrivalTime, @Destination, @Vehicle_Id, @Driver_Id);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Title", title);
                            command.Parameters.AddWithValue("@CheckOutTime", checkOutTime);
                            command.Parameters.AddWithValue("@ArrivalTime", arrivalTime);
                            command.Parameters.AddWithValue("@Destination", destination);
                            command.Parameters.AddWithValue("@Vehicle_Id", Convert.ToInt32(vehicle));
                            command.Parameters.AddWithValue("@Driver_Id", Convert.ToInt32(driver));

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

            Response.Redirect("/DispatcherPages/ViewAllRoutes");
        }
    }
}
