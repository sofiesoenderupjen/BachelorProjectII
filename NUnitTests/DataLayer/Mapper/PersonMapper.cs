using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DomainLayer.Models;
using Microsoft.Data.SqlClient;

namespace BachelorProjectII.DataLayer.Mapper
{
    public static class PersonMapper
    {
    
        public static PersonModel FromEntityToDomainModel(this PersonEntity entity)
        {
            if (entity == null) { return null; }

            return new PersonModel()
            {
                Id = entity.Id,
                Navn = entity.Navn,
                UUID = entity.UUID,
                Fodselsdato = entity.Fodselsdato,
                Alder = entity.Alder,
                Pinkode = entity.PinKode,
                FortrukneApotekId = entity.FortrukneApotekId
            };
        }

        public static PersonEntity FromDomainModelToEntity(this PersonModel model)
        {
            if (model == null) { return null; }

            return new PersonEntity()
            {
                Id = model.Id,
                Navn = model.Navn,
                UUID = model.UUID,                
                Fodselsdato = model.Fodselsdato,
                Alder = model.Alder,
                PinKode = model.Pinkode,
                FortrukneApotekId = model.FortrukneApotekId
            };
        }
    }
}
