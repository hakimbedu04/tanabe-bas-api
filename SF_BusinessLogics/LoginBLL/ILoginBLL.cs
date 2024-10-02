using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;

namespace SF_BusinessLogics.LoginBLL
{
    public interface ILoginBLL
    {
        //List<hrd_tUserDTO> CheckGlobalUser(string username,string password);
        Prod_All_KaryawanDTO GetFullName(string nik);
        string CheckGlobalUser(string username, string password);
        bool IsSupervisor(string nik);
        bas_v_rep_fullDTO CheckMvaUserInfo(string nik);
        bas_m_repDTO CheckMvaUserRole(string nik);
        string GetUserId(string tokenaccess);
        int InsertAuth(string nik, string auth);
        List<UserProfileDTO> getData(string rep_id,string host);
        int editProfile(string rep_id, string newPass, string picture);

        ProfileDoctorDetail getProfileDoctor(string dr_code);
        List<DoctorActivityDTO> getDoctorActivity(string dr_code, int year);
        string GetProfilePic(string nik);

        void LogLogin(LogLoginInput inputs);
        bool GetPositionByRole(string role);
        //string 
    }
}
