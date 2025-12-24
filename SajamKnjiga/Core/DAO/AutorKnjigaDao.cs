using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class AutorKnjigaDao
    {
        private readonly Storage<AutorKnjiga> _storage;

        public AutorKnjigaDao()
        {
            _storage = new Storage<AutorKnjiga>("autor_knjiga.csv");
        }

        public List<AutorKnjiga> GetAll()
        {
            return _storage.Load();
        }
    }
}
