using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Mapper;
using Microsoft.Data.SqlClient;

namespace BachelorProjectII.DataLayer.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DBConnect _dbConnect;

        public PersonRepository(DBConnect dbConnect)
        {
            _dbConnect = dbConnect;
        }

        public async Task<List<PersonModel>> GetPersonerAsync()
        { 
            var personer = new List<PersonEntity>();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Person", connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        personer.Add(new PersonEntity()
                        {
                            Id = reader.GetInt32(0),
                            UUID = reader.GetString(1),                            
                            Navn = reader.GetString(2),
                            Alder = reader.GetInt32(3),
                            Fodselsdato = reader.GetString(4),
                            PinKode = (reader.IsDBNull(5) ? 0 : reader.GetInt32(6)),
                            FortrukneApotekId = (reader.IsDBNull(6)?0:reader.GetInt32(6))
                        });
                    }
                }
            }

            if (personer.Count() == 0)
            {
                return null;
            }

            return personer.Select(x => x.FromEntityToDomainModel()).ToList();
        }

        public async Task<PersonModel> GetPersonByUUIDAsync(string uuId)
        {
            var person = new PersonEntity();
            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM Person WHERE UUId = @UUId", connection);
                command.Parameters.AddWithValue("@UUId", uuId);
                try
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            person = new PersonEntity()
                            {
                                Id = reader.GetInt32(0),
                                UUID = reader.GetString(1),                            
                                Navn = reader.GetString(2),
                                Alder = reader.GetInt32(3),
                                Fodselsdato = reader.GetString(4),
                                PinKode = (reader.IsDBNull(5) ? 0 : reader.GetInt32(5)),
                                FortrukneApotekId = (reader.IsDBNull(6) ? 0 : reader.GetInt32(6))
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                
            }

            if (person == null)
            {
                return null;
            }
            return person.FromEntityToDomainModel();
        }


        private async Task AddRandomIndlaegssedlerForPersonAsync(int personId, SqlConnection connection)
        {
            // Select 1-5 random Indlaegssedler IDs
            SqlCommand selectRandomIndlaegssedlerCommand = new SqlCommand(
                "SELECT TOP (@Count) Id FROM Indlaegsseddel ORDER BY NEWID()", connection);

            Random random = new Random();
            int randomCount = random.Next(1, 6); // Get a random number between 1 and 5
            selectRandomIndlaegssedlerCommand.Parameters.AddWithValue("@Count", randomCount);

            List<int> selectedIndlaegssedlerIds = new List<int>();
            using (var reader = await selectRandomIndlaegssedlerCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    selectedIndlaegssedlerIds.Add(reader.GetInt32(0));
                }
            }

            // Insert selected Indlaegssedler IDs into Person_ReceptIndlaegssedler
            foreach (int indlaegsseddelId in selectedIndlaegssedlerIds)
            {
                SqlCommand insertReceptCommand = new SqlCommand(
                    "INSERT INTO Person_ReceptIndlaegssedler (PersonId, IndlaegsseddelId) VALUES (@PersonId, @IndlaegsseddelId)", connection);

                insertReceptCommand.Parameters.AddWithValue("@PersonId", personId);
                insertReceptCommand.Parameters.AddWithValue("@IndlaegsseddelId", indlaegsseddelId);

                await insertReceptCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> SetFortrukneApotekAsync(int personId, int apotekId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(
                        "UPDATE Person SET FortrukneApotekId = @ApotekId WHERE Id = @PersonId", connection);

                    command.Parameters.AddWithValue("@ApotekId", apotekId);
                    command.Parameters.AddWithValue("@PersonId", personId);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<List<IndlaegsseddelModel>> GetGemteIndlaegssedlerAsync(int personId)
        {
            var indlaegssedler = new List<IndlaegsseddelEntity>();

            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(
                "SELECT * FROM Indlaegsseddel " +
                "INNER JOIN Person_GemteIndlaegssedler ON Indlaegsseddel.Id = Person_GemteIndlaegssedler.IndlaegsseddelId " +
                "WHERE Person_GemteIndlaegssedler.PersonId = @PersonId", connection);
                command.Parameters.AddWithValue("@PersonId", personId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        indlaegssedler.Add(new IndlaegsseddelEntity
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
            if (indlaegssedler.Count() == 0)
            {
                return new List<IndlaegsseddelModel>();
            }

            return indlaegssedler.Select(x => x.FromEntityToDomainModel()).ToList();

        }

        public async Task<List<IndlaegsseddelModel>> GetReceptIndlaegssedlerAsync(int personId)
        {
            var indlaegssedler = new List<IndlaegsseddelEntity>();

            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(
                "SELECT * FROM Indlaegsseddel " +
                "INNER JOIN Person_ReceptIndlaegssedler ON Indlaegsseddel.Id = Person_ReceptIndlaegssedler.IndlaegsseddelId " +
                "WHERE Person_ReceptIndlaegssedler.PersonId = @personId", connection);
                command.Parameters.AddWithValue("@PersonId", personId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        indlaegssedler.Add(new IndlaegsseddelEntity
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
            if (indlaegssedler.Count() == 0)
            {
                return new List<IndlaegsseddelModel>();
            }

            return indlaegssedler.Select(x => x.FromEntityToDomainModel()).ToList();

        }

        public async Task<bool> CreatePersonAsync(PersonModel person)
        {
            var personEntity = person.FromDomainModelToEntity();

            person.UUID = person.UUID.ToLower();

            using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
            {
                await connection.OpenAsync();

                SqlCommand command1 = new SqlCommand(
                    "SELECT COUNT(*) FROM Person WHERE UUId = '" + person.UUID + "'", connection);

                int count = (int)await command1.ExecuteScalarAsync();
                if (count > 0)
                {
                    return false;
                }

                SqlCommand command = new SqlCommand(
                    "INSERT INTO Person (Navn, UUId, Alder, Fodselsdato) VALUES (@Navn, @UUID, @Alder, @Fodselsdato)", connection);

                command.Parameters.AddWithValue("@Navn", personEntity.Navn);
                command.Parameters.AddWithValue("@UUID", personEntity.UUID);
                command.Parameters.AddWithValue("@Alder", personEntity.Alder);
                command.Parameters.AddWithValue("@Fodselsdato", personEntity.Fodselsdato);

                await command.ExecuteNonQueryAsync();
            }
            return true;
        }


        public async Task<bool> UpdatePinkodeAsync(int personId, int newPinKode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(
                        "UPDATE Person SET PinKode = @PinKode WHERE Id = @PersonId", connection);

                    // Tilføj parametrene til forespørgslen
                    command.Parameters.AddWithValue("@PersonId", personId);
                    command.Parameters.AddWithValue("@PinKode", newPinKode);

                    // Udfør opdateringen
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public async Task<bool> AddGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
                {
                    await connection.OpenAsync();
                    SqlCommand command1 = new SqlCommand(
                    "SELECT COUNT(*) FROM Person_GemteIndlaegssedler WHERE PersonId = " + personId + " and IndlaegsseddelId = " + indlaegsseddelId +"", connection);

                    int count = (int)await command1.ExecuteScalarAsync();
                    if (count > 0)
                    {
                        return false;
                    }

                    SqlCommand command = new SqlCommand(
                        "INSERT INTO Person_GemteIndlaegssedler (PersonId, IndlaegsseddelId) VALUES (@PersonId, @IndlaegsseddelId)", connection);

                    // Tilføj parametre til forespørgslen
                    command.Parameters.AddWithValue("@PersonId", personId);
                    command.Parameters.AddWithValue("@IndlaegsseddelId", indlaegsseddelId);

                    // Udfør INSERT-forespørgslen
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        public async Task<bool> RemoveGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnect.DBConnectionsstring))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(
                        "DELETE FROM Person_GemteIndlaegssedler WHERE PersonId = @PersonId AND IndlaegsseddelId = @IndlaegsseddelId", connection);

                    // Tilføj parametre til forespørgslen
                    command.Parameters.AddWithValue("@PersonId", personId);
                    command.Parameters.AddWithValue("@IndlaegsseddelId", indlaegsseddelId);

                    // Udfør DELETE-forespørgslen
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
            
        }
    }
}
