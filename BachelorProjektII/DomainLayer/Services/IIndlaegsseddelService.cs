using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DomainLayer.Services
{
    public interface IIndlaegsseddelService
    {
        Task<List<IndlaegsseddelModel>> GetIndlaegssedler();

        Task<IndlaegsseddelModel> GetIndlaegsseddelById(int id);
    }
}
