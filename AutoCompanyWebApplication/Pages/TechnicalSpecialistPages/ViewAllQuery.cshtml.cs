using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using AutoCompanyWebApplication.Classes;

namespace AutoCompanyWebApplication.Pages.TechnicalSpecialistPages
{
    public class ViewAllQueryModel : PageModel
    {
        public static List<Repairing> repairings = new List<Repairing>();
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static List<RepairingStage> stages = new List<RepairingStage>();

        public void OnGet()
        {
            repairings.Clear();
            vehicles.Clear();
            stages.Clear();

            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [Repairing stages]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RepairingStage stage = new RepairingStage();
                                stage.Id = "" + reader.GetInt32(0);
                                stage.Title = reader.GetString(1);
                                stages.Add(stage);
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

                                vehicles.Add(vehicle);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Repairings WHERE RepairingStage_Id = 1";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Repairing repairing = new Repairing();
                                repairing.Id = "" + reader.GetInt32(0);
                                repairing.RepairingStageId = "" + reader.GetInt32(3);
                                repairing.VehicleId = "" + reader.GetInt32(7);

                                foreach (var vehicle in vehicles)
                                {
                                    if (vehicle.Id == repairing.VehicleId)
                                    {
                                        repairing.VehicleTitle = vehicle.LicensePlate;
                                    }
                                }
                                foreach (var stage in stages)
                                {
                                    if (stage.Id == repairing.RepairingStageId)
                                    {
                                        repairing.RepairingStageTitle = stage.Title;
                                    }
                                }
                                repairings.Add(repairing);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
