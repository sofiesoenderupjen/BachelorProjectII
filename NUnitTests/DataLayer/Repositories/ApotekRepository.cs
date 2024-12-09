using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DataLayer.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace BachelorProjectII.DataLayer.Repositories
{
    public class ApotekRepository : IApotekRepository
    {
        private readonly DBConnect _dbConnect;

        public ApotekRepository(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }

        public async Task<List<ApotekModel>> GetApotekerAsync()
        {
            var apoteker = new List<ApotekEntity>();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Apotek", connection);
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            apoteker.Add(new ApotekEntity()
                            {
                                Id = reader.GetInt32(0),
                                ApotekNavn = reader.GetString(1),
                                Adresse = reader.GetString(2),
                                TelefonNummer = reader.GetInt32(3)
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (apoteker.Count() == 0)
            {
                return null;
            }

            return apoteker.Select(x => x.FromEntityToDomainModel()).ToList();
        }
        
        public async Task<ApotekModel> GetApotekByIdAsync(int id)
        {
            var apotek = new ApotekEntity();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Apotek WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            apotek = new ApotekEntity()
                            {
                                Id = reader.GetInt32(0),
                                ApotekNavn = reader.GetString(1),
                                Adresse = reader.GetString(2),
                                TelefonNummer = reader.GetInt32(3)
                            };

                        }
                    }
                }
                catch (Exception)
                {

                    return null;
                }
            }

            if (apotek == null)
            {
                return null;
            }

            return apotek.FromEntityToDomainModel();
        }

        public async Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLagerAsync(int indlaegsseddelId)
        {
            var apoteker = new List<ApotekEntity>();

            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(
                "SELECT * FROM Apotek " +
                "INNER JOIN Apotek_IndlaegssedlerPaaLager ON Apotek.Id = Apotek_IndlaegssedlerPaaLager.ApotekId " +
                "WHERE Apotek_IndlaegssedlerPaaLager.IndlaegsseddelId = @IndlaegsseddelId", connection);
                command.Parameters.AddWithValue("@IndlaegsseddelId", indlaegsseddelId);
                
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            apoteker.Add(new ApotekEntity
                            {
                                Id = reader.GetInt32(0),
                                ApotekNavn = reader.GetString(1),
                                Adresse = reader.GetString(2),
                                TelefonNummer = reader.GetInt32(3)
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            if (apoteker.Count() == 0)
            {
                return null;
            }

            return apoteker.Select(x => x.FromEntityToDomainModel()).ToList();

        }

        public async Task<bool> IsIndlaegsseddelPaaLagerAsync(int apotekId, int indlaegsseddelId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) FROM Apotek_IndlaegssedlerPaaLager WHERE ApotekId = @ApotekId AND IndlaegsseddelId = @IndlaegsseddelId", connection);

                    command.Parameters.AddWithValue("@ApotekId", apotekId);
                    command.Parameters.AddWithValue("@IndlaegsseddelId", indlaegsseddelId);

                    int count = (int)await command.ExecuteScalarAsync();
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
