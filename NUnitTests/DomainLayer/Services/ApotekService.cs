using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Services
{
    public class ApotekService : IApotekService
    {
        public IApotekRepository _apotekRepo;

        public ApotekService(IApotekRepository apotekRepository)
        {
            _apotekRepo = apotekRepository;
        }
        
        public async Task<List<ApotekModel>> GetApotekerAsync()
        {
            return await _apotekRepo.GetApotekerAsync();
        }

        public async Task<ApotekModel> GetApotekByIdAsync(int id)
        {
            return await _apotekRepo.GetApotekByIdAsync(id);
        }

        public async Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLagerAsync(int indlaegsseddelId)
        {
            return await _apotekRepo.GetApotekerWithIndlaegsseddelPaaLagerAsync(indlaegsseddelId);
        }

        public async Task<bool> IsIndlaegsseddelPaaLagerAsync(int apotekId, int indlaegsseddelId)
        {
            return await _apotekRepo.IsIndlaegsseddelPaaLagerAsync(apotekId, indlaegsseddelId);
        }
    }
}
