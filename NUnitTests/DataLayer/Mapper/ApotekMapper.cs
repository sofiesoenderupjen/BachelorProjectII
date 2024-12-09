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
    public static class ApotekMapper
    {
        public static ApotekModel FromEntityToDomainModel(this ApotekEntity entity)
        {
            if (entity == null) { return null; }
            
            return new ApotekModel()
            {
                Id = entity.Id,
                Adresse = entity.Adresse,
                ApotekNavn = entity.ApotekNavn,
                TelefonNummer = entity.TelefonNummer,
                IndlaegssedlerPaaLager = entity.IndlaegssedlerPaaLager?.Select(x => x.FromEntityToDomainModel()).ToList()
            };
            
        }

        public static ApotekEntity FromDomainModelToEntity(this ApotekModel model)
        {
            if (model == null) { return null; }
           
            return new ApotekEntity()
            {
                Id = model.Id,
                Adresse = model.Adresse,
                ApotekNavn = model.ApotekNavn,
                TelefonNummer = model.TelefonNummer,
                IndlaegssedlerPaaLager = model.IndlaegssedlerPaaLager?.Select(x => x.FromDomainModelToEntity()).ToList()
            };
            
        }

    }
}
