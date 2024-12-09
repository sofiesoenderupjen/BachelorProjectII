using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DomainLayer.Services
{
    public interface IPersonService
    {
        void LogIn(PersonModel user);

        void LogOut();
        
        Task<PersonModel> LoadPersonFromPreferences();

        Task<List<PersonModel>> GetPersoner();

        Task<PersonModel> GetPersonByUUID(string uuId);

        Task<bool> CreatePerson(PersonModel person);

        Task<bool> UpdatePinkode(int personId, int newPinKode);

        Task<bool> SetFortrukneApotek(int personId, int apotekId);

        Task<bool> AddGemteIndlaegsseddel(int personId, int indlaegsseddelId);

        Task<bool> RemoveGemteIndlaegsseddel(int personId, int indlaegsseddelId);

        Task AddRandomReceptIndlaegssedlerForPersonAsync(string uuId);
    }
}
