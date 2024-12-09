using BachelorProjectII.DataLayer;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DataLayer.Repositories;
using BachelorProjectII.DomainLayer.Models;
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
        public IPersonRepository _personRepo;

        public PersonService(IPersonRepository personRepository) 
        { 
            _personRepo = personRepository;
        }
        
        public async Task<List<PersonModel>> GetPersonerAsync()
        {
            return await _personRepo.GetPersonerAsync();
        }

        public async Task<PersonModel> GetPersonByUUIDAsync(string uuId)
        {
            var person = await _personRepo.GetPersonByUUIDAsync(uuId);
            if (person != null)
            {
                person.GemteIndlaegssedler = await _personRepo.GetGemteIndlaegssedlerAsync(person.Id);
                person.ReceptIndlaegssedler = await _personRepo.GetReceptIndlaegssedlerAsync(person.Id);

                return person;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreatePersonAsync(PersonModel person)
        {
            return await _personRepo.CreatePersonAsync(person);
        }

        public async Task<bool> UpdatePinkodeAsync(int personId, int newPinKode)
        {
            return await _personRepo.UpdatePinkodeAsync(personId, newPinKode);
        }

        public async Task<bool> SetFortrukneApotekAsync(int personId, int apotekId)
        {
            return await _personRepo.SetFortrukneApotekAsync(personId, apotekId);
        }

        public async Task<bool> AddGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId)
        {
            return await _personRepo.AddGemteIndlaegsseddelAsync(personId, indlaegsseddelId);

        }

        public async Task<bool> RemoveGemteIndlaegsseddelAsync(int personId, int indlaegsseddelId)
        {
            return await _personRepo.RemoveGemteIndlaegsseddelAsync(personId, indlaegsseddelId);

        }
    }
}
