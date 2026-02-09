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
        private readonly List<AutorKnjiga> _veze;

        public AutorKnjigaDao()
        {
            _storage = new Storage<AutorKnjiga>("autor_knjiga.csv");
            _veze = _storage.Load() ?? new List<AutorKnjiga>();
        }

        public void AddVeza(int autorId, string isbn)
        {
            if (!_veze.Any(v => v.AutorId == autorId && v.ISBN == isbn))
            {
                _veze.Add(new AutorKnjiga
                {
                    AutorId = autorId,
                    ISBN = isbn
                });

                _storage.Save(_veze);
            }
        }

        public void RemoveByISBN(string isbn)
        {
            _veze.RemoveAll(v => v.ISBN == isbn);
            _storage.Save(_veze);
        }

        public List<AutorKnjiga> GetAll()
        {
            return _veze;
        }

    }

}
