using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Models
{
    public class PersonModel
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public string UUID { get; set; }

        public int Alder { get; set; }

        public string Fodselsdato { get; set; }

        public int Pinkode { get; set; }

        public int? FortrukneApotekId { get; set; }

        public List<IndlaegsseddelModel> GemteIndlaegssedler { get; set; }

        public List<IndlaegsseddelModel> ReceptIndlaegssedler { get; set; }

        public ApotekModel FortrukneApotek { get; set; }
    }
}
