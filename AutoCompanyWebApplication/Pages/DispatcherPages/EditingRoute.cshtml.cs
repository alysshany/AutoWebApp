using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoCompanyWebApplication.Classes;
using System.Data.SqlClient;

namespace AutoCompanyWebApplication.Pages.DispatcherPages
{
    public class EditingRouteModel : PageModel
    {
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static List<Driver> drivers = new List<Driver>();
        public static Classes.Route route = new Classes.Route();
        public string errorMessage = "";
        public void OnGet()
        {
            string id = Request.Query["ID"];
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

                                vehicles.Add(vehicle); 
                            }
                        }
                    }
                }

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
                    string sql = "SELECT * FROM [Routes] WHERE Route_Id = @Route_Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Route_Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void OnPost()
        {
            route.Title = Request.Form["title"];
            route.CheckOutTime = Request.Form["checkOutTime"];
            route.ArrivalTime = Request.Form["arrivalTime"];
            route.Destination = Request.Form["destination"];
            route.DriverId = Request.Form["drivers"];
            route.VehicleId = Request.Form["vehicles"];

            if (route.Title.Length > 0 && route.CheckOutTime.Length > 0 && route.ArrivalTime.Length > 0 && route.Destination.Length > 0 && route.VehicleId.Length > 0 && route.DriverId.Length > 0)
            {
                try
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE [Routes] " +
                                    "SET Title=@Title, CheckOutTime=@CheckOutTime, ArrivalTime=@ArrivalTime, Destination = @Destination, Driver_Id = @Driver_Id, Vehicle_Id = @Vehicle_Id " +
                                    "WHERE Route_Id = @id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", Convert.ToInt32(route.Id));
                            command.Parameters.AddWithValue("@Title", route.Title);
                            command.Parameters.AddWithValue("@CheckOutTime", route.CheckOutTime);
                            command.Parameters.AddWithValue("@ArrivalTime", route.ArrivalTime);
                            command.Parameters.AddWithValue("@Destination", route.Destination);
                            command.Parameters.AddWithValue("@Driver_Id", Convert.ToInt32(route.DriverId));
                            command.Parameters.AddWithValue("@Vehicle_Id", Convert.ToInt32(route.VehicleId));

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
