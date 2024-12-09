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
        Task<List<ApotekModel>> GetApoteker();

        Task<ApotekModel> GetApotekById(int id);     
        
        Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLager(int indlaegsseddelId);

        Task<bool> IsIndlaegsseddelPaaLager(int apotekId, int indlaegsseddelId);

    }
}
