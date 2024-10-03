using KoiMuseum.Data.Models;
using KoiMuseum.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data
{
    public class UnitOfWork
    {
        private Fa24Se172594Prn231G1KfsContext context;
        private UserRepository repository;

        public UnitOfWork()
        {
            context ??= new Fa24Se172594Prn231G1KfsContext();
        }

        public UserRepository UserRepository { get { return repository ??= new UserRepository(); } }
    }
}
