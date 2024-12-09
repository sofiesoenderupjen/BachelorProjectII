using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DataLayer.Repositories
{
    public interface IPersonRepository
    {
        Task<List<PersonModel>> GetPersonerAsync();

        Task<PersonModel> GetPersonByUUIDAsync(string uuId);

        Task<List<IndlaegsseddelModel>> GetGemteIndlaegssedlerAsync(int personId);

        Task<List<IndlaegsseddelModel>> GetReceptIndlaegssedlerAsync(int personId);

        Task<bool> CreatePersonAsync(PersonModel person);

        Task<bool> UpdatePinkodeAsync(int personId, int newPinKode);

        Task<bool> SetFortrukneApotekAsync(int personId, int apotekId);

        Task<bool> AddGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId);

        Task<bool> RemoveGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId);
    }
}
