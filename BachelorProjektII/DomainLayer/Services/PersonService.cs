using BachelorProjectII.DataLayer;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DataLayer.Repositories;
using BachelorProjectII.DomainLayer.Models;
using DevExpress.Maui.Editors;
using Java.Util;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Services
{
    public class PersonService : IPersonService
    {
        public readonly IPersonRepository _personRepo;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepo = personRepository;
            _ = LoadPersonFromPreferences();
        }

        public void LogIn(PersonModel user)
        {
            Preferences.Set("LoggedInUUID", user.UUID);
        }

        public void LogOut()
        {
            Preferences.Remove("LoggedInUUID");
        }

        public async Task<PersonModel> LoadPersonFromPreferences()
        {
            var uuId = Preferences.Get("LoggedInUUID", string.Empty);

            if (!string.IsNullOrEmpty(uuId))
            {
                // Hent brugeren fra dit repository eller database ved hjælp af uuid
                return await GetPersonByUUID(uuId);
            }
            else
            {
                return null;
            }
        }
        
        public async Task<List<PersonModel>> GetPersoner()
        {
            return await _personRepo.GetPersoner();
        }

        public async Task<PersonModel> GetPersonByUUID(string uuId)
        {
            var person = await _personRepo.GetPersonByUUID(uuId);
            if (person != null)
            {
                person.GemteIndlaegssedler = await _personRepo.GetGemteIndlaegssedler(person.Id);
                person.ReceptIndlaegssedler = await _personRepo.GetReceptIndlaegssedler(person.Id);

                return person;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreatePerson(PersonModel person)
        {
            return await _personRepo.CreatePerson(person);
        }

        public async Task<bool> UpdatePinkode(int personId, int newPinKode)
        {
            return await _personRepo.UpdatePinkode(personId, newPinKode);
        }

        public async Task<bool> SetFortrukneApotek(int personId, int apotekId)
        {
            return await _personRepo.SetFortrukneApotek(personId, apotekId);
        }

        public async Task<bool> AddGemteIndlaegsseddel(int personId, int indlaegsseddelId)
        {
            return await _personRepo.AddGemteIndlaegsseddel(personId, indlaegsseddelId);

        }

        public async Task<bool> RemoveGemteIndlaegsseddel(int personId, int indlaegsseddelId)
        {
            return await _personRepo.RemoveGemteIndlaegsseddel(personId, indlaegsseddelId);

        }

        public async Task AddRandomReceptIndlaegssedlerForPersonAsync(string uuId)
        {
            var person = await GetPersonByUUID(uuId);
            if (person.ReceptIndlaegssedler.Count() == 0)
            {
                await _personRepo.AddRandomReceptIndlaegssedlerForPerson(person.Id);
            }
        }
    }
}
