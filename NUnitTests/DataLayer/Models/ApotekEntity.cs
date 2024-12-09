using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DataLayer.Models
{
    public class ApotekEntity
    {
        public int Id { get; set; }

        public string ApotekNavn { get; set; }

        public string Adresse { get; set; }

        public int TelefonNummer { get; set; }

        public List<IndlaegsseddelEntity> IndlaegssedlerPaaLager { get; set; }
    }
}