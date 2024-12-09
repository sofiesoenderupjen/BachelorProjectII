using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Services
{
    public interface IMitIdLoginService
    {
        Task<bool> LoginWithMitId();
    }
}
