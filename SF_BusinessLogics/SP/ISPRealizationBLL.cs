using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.SP;

namespace SF_BusinessLogics.SP
{
    public interface ISPRealizationBLL
    {
        List<SP_SELECT_SP_REALIZATION_DTO> GetSPRealization(SPInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorList(SPInputs inputs);
        bool AddSpeakerAdditional(SPInputs inputs);
        bool UpdateRealization(Collection inputs);
        bool SubmitRealization(SPInputs inputs);
        List<t_sp_attachment_DTO> GetSPAttachment(SPInputs inputs);
        string GetFilePath(int spfid);
        bool DeleteSPAttachment(int spfid);
    }
}
