using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_Domain.DTOs;
using SF_DAL.HRD;


namespace SF_Domain.Mappers
{
    public class hrd_MapperProfiles : Profile
    {
        public hrd_MapperProfiles()
        {
            CreateMap<hrd_tUserDTO, tuser>().PreserveReferences().ReverseMap();
            CreateMap<hrd_All_KaryawanDTO, All_Karyawan>().PreserveReferences().ReverseMap();
        }
    }
}
