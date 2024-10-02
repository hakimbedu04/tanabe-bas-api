using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.SP;
using SF_Repositories.Common;
using SF_Repositories.SPRepo;
using SF_Utils;

namespace SF_BusinessLogics.SP
{
    public class SPRealizationBLL : ISPRealizationBLL
    {
        private readonly ISqlSPRepository _spRepo;
        private readonly IBasGenericRepositories<t_sp_attachment> _tAttachRepo;
        private readonly ISPRepo _mainRepo;

        public SPRealizationBLL(ISqlSPRepository spRepo, IBasGenericRepositories<t_sp_attachment> tAttachRepo, ISPRepo mainRepo)
        {
            _spRepo = spRepo;
            _tAttachRepo = tAttachRepo;
            _mainRepo = mainRepo;
        }

        public List<SP_SELECT_SP_REALIZATION_DTO> GetSPRealization(SPInputs inputs)
        {
            return _spRepo.GetSPRealization(inputs);
        }

        public List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorList(SPInputs inputs)
        {
            return _spRepo.GetDoctorList(inputs);
        }

        public bool AddSpeakerAdditional(SPInputs inputs)
        {
            return _spRepo.AddSpeakerAdditional(inputs);
        }

        public bool UpdateRealization(Collection inputs)
        {
            return _spRepo.UpdateRealization(inputs);
        }

        public bool SubmitRealization(SPInputs inputs)
        {
            return _spRepo.SubmitRealization(inputs);
        }

        public List<t_sp_attachment_DTO> GetSPAttachment(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<t_sp_attachment>();
            if (!String.IsNullOrEmpty(inputs.SprId))
            {
                queryFilter = queryFilter.And(x => x.spr_id == inputs.SprId);
            }
            var dbResult = _tAttachRepo.Get(queryFilter).ToList();
            var newReturn = dbResult.Select(x => new t_sp_attachment()
            {
                spf_id = x.spf_id,
                spr_id = x.spr_id,
                spf_file_name = x.spf_file_name,
                spf_file_path = (inputs.Host.Replace("bas_api_mobile", "bas").Contains("vodjo") ? (inputs.Host.Replace("bas_api_mobile", "bas") + ":8088/") : inputs.Host.Replace("bas_api_mobile", "bas")) + GetStringPattern().Replace(x.spf_file_path, ""),
                spf_date_uploaded = x.spf_date_uploaded
            }).ToList();


            return Mapper.Map<List<t_sp_attachment_DTO>>(newReturn);
        }

        public Regex GetStringPattern()
        {
            Regex pattern = new Regex("~");
            return pattern;
        }

        public string GetFilePath(int spfid)
        {
            return _mainRepo.GetFilePath(spfid);
        }

        public bool DeleteSPAttachment(int spfid)
        {
            return _mainRepo.DeleteSPAttachment(spfid);
        }
    }
}
