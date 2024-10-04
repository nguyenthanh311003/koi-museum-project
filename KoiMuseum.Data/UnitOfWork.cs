using KoiMuseum.Data.Models;
using KoiMuseum.Data.Repositories;

namespace KoiMuseum.Data
{
    public class UnitOfWork
    {
        private Fa24Se172594Prn231G1KfsContext context;
        private UserRepository repository;
        private RegisterDetailRepository registerDetailRepository;
        private RegistrationRepository registrationRepository;

        public UnitOfWork()
        {
            context ??= new Fa24Se172594Prn231G1KfsContext();
        }

        public UserRepository UserRepository { get { return repository ??= new UserRepository(); } }
        public RegisterDetailRepository RegisterDetailRepository { get { return registerDetailRepository ??= new RegisterDetailRepository(); } }
        public RegistrationRepository RegistrationRepository { get { return registrationRepository ??= new RegistrationRepository(); } }
    }
}
