using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DataLayer.Models;
using BachelorProjectII.DomainLayer.Models;
using Microsoft.Data.SqlClient;


namespace BachelorProjectII.DataLayer.Repositories
{
    public interface IApotekRepository
    {
        Task<List<ApotekModel>> GetApotekerAsync();

        Task<ApotekModel> GetApotekByIdAsync(int id);

        Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLagerAsync(int indlaegsseddelId);

        Task<bool> IsIndlaegsseddelPaaLagerAsync(int apotekId, int indlaegsseddelId);
    }
}
