using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DataLayer
{
    public  class DBConnect
    {
        public string DBConnectionsstring { get; private set; } = "Server=tcp:indlaegsseddel.database.windows.net,1433;Initial Catalog=IndlaegsseddelDB;User ID=indlaegsseddel_admin;Password=ThisShouldBeGoodEnough123#;";

        //public string DBConnectionsstring { get; private set; } = "Server=20.166.252.213;Database=IndlaegsseddelDB;User ID=sa;Password=r5t6y7u8;MultipleActiveResultSets=True;TrustServerCertificate=True;";
    }
}
