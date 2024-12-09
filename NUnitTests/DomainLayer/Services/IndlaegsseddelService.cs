using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Repositories;
using BachelorProjectII.DomainLayer.Models;

namespace BachelorProjectII.DomainLayer.Services
{
    public class IndlaegsseddelService : IIndlaegsseddelService
    {
        private IIndlaegsseddelRepository _indlaegsseddelRepo;

        public IndlaegsseddelService(IIndlaegsseddelRepository indlaegsseddelRepo)
        {
            _indlaegsseddelRepo = indlaegsseddelRepo;
        }

        public async Task<List<IndlaegsseddelModel>> GetIndlaegssedler()
        {
            return await _indlaegsseddelRepo.GetIndlaegssedler();
        }

        public async Task<IndlaegsseddelModel> GetIndlaegsseddelById(int id)
        {
            return await _indlaegsseddelRepo.GetIndlaegsseddelById(id);
        }

    }
}
