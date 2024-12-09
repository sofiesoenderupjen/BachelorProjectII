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
        private readonly IApotekRepository _apotekRepo;

        public ApotekService(IApotekRepository apotekRepository)
        {
            _apotekRepo = apotekRepository;
        }
        
        public async Task<List<ApotekModel>> GetApoteker()
        {
            return await _apotekRepo.GetApoteker();
        }

        public async Task<ApotekModel> GetApotekById(int id)
        {
            return await _apotekRepo.GetApotekById(id);
        }

        public async Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLager(int indlaegsseddelId)
        {
            return await _apotekRepo.GetApotekerWithIndlaegsseddelPaaLager(indlaegsseddelId);
        }

        public async Task<bool> IsIndlaegsseddelPaaLager(int apotekId, int indlaegsseddelId)
        {
            return await _apotekRepo.IsIndlaegsseddelPaaLager(apotekId, indlaegsseddelId);
        }
    }
}
