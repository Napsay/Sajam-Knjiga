using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IKupovinaDao
    {
        Kupovina addKupovina(Kupovina kupovina);
        Kupovina deleteKupovina(int kupovinaId);
        Kupovina updateKupovina(Kupovina kupovina);
        List<Kupovina> getAllKupovine();
        Kupovina GetById(int kupovinaId);
    
    }
}
