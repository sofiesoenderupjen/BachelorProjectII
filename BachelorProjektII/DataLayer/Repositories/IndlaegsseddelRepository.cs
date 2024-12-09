using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DataLayer.Mapper;
using Microsoft.Data.SqlClient;
using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;

namespace BachelorProjectII.DataLayer.Repositories
{
    public class IndlaegsseddelRepository : IIndlaegsseddelRepository
    {
        private readonly DBConnect _dbConnect;

        public IndlaegsseddelRepository(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }
        public async Task<List<IndlaegsseddelModel>> GetIndlaegssedler()
        {
            var indlaegssedler = new List<IndlaegsseddelEntity>();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Indlaegsseddel", connection);
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            indlaegssedler.Add(new IndlaegsseddelEntity()
                            {
                                Id = reader.GetInt32(0),
                                Navn = reader.GetString(1),
                                FormOgStyrke = reader.GetString(2),
                                Virksomhed = reader.GetString(3),
                                Indlaegsseddel = reader.GetString(4)
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (indlaegssedler.Count() == 0)
            {
                return null;
            }

            return indlaegssedler.Select(x => x.FromEntityToDomainModel()).ToList();
        }

        public async Task<IndlaegsseddelModel> GetIndlaegsseddelById(int id)
        {
            var indlaegsseddel = new IndlaegsseddelEntity();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Indlaegsseddel WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            indlaegsseddel = new IndlaegsseddelEntity()
                            {
                                Id = reader.GetInt32(0),
                                Navn = reader.GetString(1),
                                FormOgStyrke = reader.GetString(2),
                                Virksomhed = reader.GetString(3),
                                Indlaegsseddel = reader.GetString(4)
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (indlaegsseddel == null)
            {
                return null;
            }
            return indlaegsseddel.FromEntityToDomainModel();
        }
    }
}
