using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DomainLayer.Services
{
    public interface IApotekService
    {
        Task<List<ApotekModel>> GetApotekerAsync();

        Task<ApotekModel> GetApotekByIdAsync(int id);     
        
        Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLagerAsync(int indlaegsseddelId);

        Task<bool> IsIndlaegsseddelPaaLagerAsync(int apotekId, int indlaegsseddelId);

    }
}
