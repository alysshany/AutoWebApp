using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using AutoCompanyWebApplication.Classes;

namespace AutoCompanyWebApplication.Pages.DispatcherPages
{
    public class ViewAllRoutesModel : PageModel
    {
        public static List<Classes.Route> routes = new List<Classes.Route>();
        public static List<Driver> drivers = new List<Driver>();
        public static List<Vehicle> vehicles = new List<Vehicle>();

        public void OnGet()
        {
            routes.Clear();
            drivers.Clear();
            vehicles.Clear();
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
                    string sql = "SELECT * FROM Routes";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Classes.Route route = new Classes.Route();
                                route.Id = "" + reader.GetInt32(0);
                                route.Title = reader.GetString(1);
                                route.CheckOutTime = reader.GetString(2);
                                route.ArrivalTime = reader.GetString(3);
                                route.Destination = reader.GetString(4);
                                route.VehicleId = "" + reader.GetInt32(5);
                                route.DriverId = "" + reader.GetInt32(6);

                                foreach (var vehicle in vehicles)
                                {
                                    if (vehicle.Id == route.VehicleId)
                                    {
                                        route.VehiceTitle = vehicle.LicensePlate;
                                    }
                                }
                                foreach (var driver in drivers)
                                {
                                    if (driver.Id == route.DriverId)
                                    {
                                        route.DriverTitle = driver.Surname + " " + driver.Name + " " + driver.MiddleName;
                                    }
                                }

                                routes.Add(route);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
