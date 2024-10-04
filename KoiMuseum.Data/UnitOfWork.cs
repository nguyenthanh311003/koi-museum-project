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
        private UserRepository userRepository;
        private RankRepository rankRepository;

        public UnitOfWork()
        {
            context ??= new Fa24Se172594Prn231G1KfsContext();
        }

        public UserRepository UserRepository { get { return userRepository ??= new UserRepository(); } }
        public RankRepository RankRepository { get { return rankRepository ??= new RankRepository(); } }
    }
}
