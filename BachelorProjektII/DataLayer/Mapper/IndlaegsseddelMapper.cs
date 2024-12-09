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
    public static class IndlaegsseddelMapper
    {
        public static IndlaegsseddelModel FromEntityToDomainModel(this IndlaegsseddelEntity entity)
        {
            if (entity == null) { return null; }

            return new IndlaegsseddelModel()
            {
                Id = entity.Id,
                Navn = entity.Navn,
                FormOgStyrke = entity.FormOgStyrke,
                Virksomhed = entity.Virksomhed,
                Indlaegsseddel = entity.Indlaegsseddel
            };
        }

        public static IndlaegsseddelEntity FromDomainModelToEntity(this IndlaegsseddelModel model)
        {
            if (model == null) { return null; }

            return new IndlaegsseddelEntity()
            {
                Id = model.Id,
                Navn = model.Navn,
                FormOgStyrke = model.FormOgStyrke,
                Virksomhed = model.Virksomhed,
                Indlaegsseddel = model.Indlaegsseddel
            };
        }
    }
}
