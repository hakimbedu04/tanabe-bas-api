using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_Domain.DTOs;

namespace SF_Domain.Mappers
{
    public class prod_MapperProfiles : Profile
    {
        public prod_MapperProfiles()
        {
            CreateMap<Prod_All_KaryawanDTO, SF_DAL.Prod.All_Karyawan>().PreserveReferences().ReverseMap();
        }
    }
}
