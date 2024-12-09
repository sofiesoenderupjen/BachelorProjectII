using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DataLayer.Models
{
    public class PersonEntity
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public string UUID { get; set; }

        public int Alder { get; set; }

        public string Fodselsdato { get; set; }

        public int PinKode { get; set; }

        public int? FortrukneApotekId { get; set; }

        public List<IndlaegsseddelEntity> GemteIndlaegssedler { get; set; }

        public List<IndlaegsseddelEntity> ReceptIndlaegssedler { get; set; }
    }
}
