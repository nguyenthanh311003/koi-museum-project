﻿using KoiMuseum.Data.Models;
using KoiMuseum.Data.Repositories;

namespace KoiMuseum.Data
{
    public class UnitOfWork
    {
        private Fa24Se172594Prn231G1KfsContext context;
        private UserRepository repository;
        private RegisterDetailRepository registerDetailRepository;
        private RegistrationRepository registrationRepository;
        private RankRepository rankRepository;
        private ContestRankRepository contestRankRepository;
        private ContestRepository contestRepository;
        private JudgeRepository judgeRepository;
        private JudgeResultRepository judgeResultRepository;
        private PaymentRepository paymentRepository;

        public UnitOfWork()
        {
            context ??= new Fa24Se172594Prn231G1KfsContext();
        }

        public UserRepository UserRepository { get { return repository ??= new UserRepository(); } }
        public RegisterDetailRepository RegisterDetailRepository { get { return registerDetailRepository ??= new RegisterDetailRepository(); } }
        public RegistrationRepository RegistrationRepository { get { return registrationRepository ??= new RegistrationRepository(); } }
        public RankRepository RankRepository { get { return rankRepository ??= new RankRepository(); } }
        public ContestRankRepository ContestRankRepository { get { return contestRankRepository ??= new ContestRankRepository(); } }
        public ContestRepository ContestRepository { get { return contestRepository ??= new ContestRepository(); } }
        public JudgeRepository JudgeRepository { get { return judgeRepository ??= new JudgeRepository(); } }
        public JudgeResultRepository JudgeResultRepository { get { return judgeResultRepository ??= new JudgeResultRepository(); } }
        public PaymentRepository PaymentRepository { get { return paymentRepository ??= new PaymentRepository(); } }
    }
}
