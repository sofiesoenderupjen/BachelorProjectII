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
        Task<List<ApotekModel>> GetApoteker();

        Task<ApotekModel> GetApotekById(int id);

        Task<List<ApotekModel>> GetApotekerWithIndlaegsseddelPaaLager(int indlaegsseddelId);

        Task<bool> IsIndlaegsseddelPaaLager(int apotekId, int indlaegsseddelId);
    }
}
