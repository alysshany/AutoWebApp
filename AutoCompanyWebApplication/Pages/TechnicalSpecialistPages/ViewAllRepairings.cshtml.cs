using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using AutoCompanyWebApplication.Classes;

namespace AutoCompanyWebApplication.Pages.TechnicalSpecialistPages
{
    public class ViewAllRepairingsModel : PageModel
    {
        public static List<Repairing> repairings = new List<Repairing>();
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static List<RepairingStage> stages = new List<RepairingStage>();
        public static List<Garage> garages = new List<Garage>();
        public static List<ServiceStaff> staffs = new List<ServiceStaff>();
        public static List<Position> positions = new List<Position>();
        public static List<RepairingType> types = new List<RepairingType>();

        public void OnGet()
        {
            repairings.Clear();
            vehicles.Clear();
            stages.Clear();
            garages.Clear();
            staffs.Clear();
            positions.Clear();
            types.Clear();

            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AutoBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [Repairing types]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RepairingType type = new RepairingType();
                                type.Id = "" + reader.GetInt32(0);
                                type.Title = reader.GetString(1);
                                types.Add(type);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Garages";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Garage garage = new Garage();
                                garage.Id = "" + reader.GetInt32(0);
                                garage.Title = reader.GetString(1);
                                garage.Address = reader.GetString(1);
                                garages.Add(garage);
                            }
                        }
                    }
                }

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
                    string sql = "SELECT * FROM Repairings WHERE RepairingStage_Id != 1";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Repairing repairing = new Repairing();
                                repairing.Id = "" + reader.GetInt32(0);
                                repairing.DeliveryDate = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                repairing.DateOfIssue = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                repairing.RepairingStageId = "" + reader.GetInt32(3);
                                repairing.GarageId = reader.IsDBNull(4) ? "" : "" + reader.GetInt32(4);
                                repairing.RepairingTypeId = reader.IsDBNull(5) ? "" : "" + reader.GetInt32(5);
                                repairing.ServiceStaffId = reader.IsDBNull(6) ? "" : "" + reader.GetInt32(6);
                                repairing.VehicleId = "" + reader.GetInt32(7);

                                foreach (var garage in garages)
                                {
                                    if (garage.Id == repairing.GarageId)
                                    {
                                        repairing.GarageTitle = garage.Title;
                                    }
                                }
                                foreach (var type in types)
                                {
                                    if (type.Id == repairing.RepairingTypeId)
                                    {
                                        repairing.RepairingTypeTitle = type.Title;
                                    }
                                }
                                foreach (var staff in staffs)
                                {
                                    if (staff.Id == repairing.ServiceStaffId)
                                    {
                                        repairing.ServiceStaffTitle = staff.Surname + " " + staff.Name + " " + staff.MiddleName;
                                    }
                                }
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
