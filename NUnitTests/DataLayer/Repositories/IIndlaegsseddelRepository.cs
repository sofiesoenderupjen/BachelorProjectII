using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DataLayer.Repositories
{
    public interface IIndlaegsseddelRepository
    {
        Task<List<IndlaegsseddelModel>> GetIndlaegssedler();
        
        Task<IndlaegsseddelModel> GetIndlaegsseddelById(int id);
    }
}
