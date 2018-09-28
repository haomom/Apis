using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SuspenseAccount, SuspenseAccountDTO>();
                cfg.CreateMap<SuspenseAccountDTO, SuspenseAccount>();
                cfg.CreateMap<UserCredential, UserCredentialDTO>();
                cfg.CreateMap<UserCredentialDTO, UserCredential>();
                cfg.CreateMap<Branch, BranchDTO>();
            });
        }
    }
}