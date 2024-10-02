//using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using SF_DAL.BAS;
using SF_DAL.Prod;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using All_Karyawan = SF_DAL.Prod.All_Karyawan;

namespace SF_BusinessLogics.LoginBLL
{
    public class LoginBLL : ILoginBLL
    {
        //private readonly IGenericRepository<All_Karyawan> _AllKaryawanProdRepo;

        //public LoginBLL(IGenericRepository<All_Karyawan> AllKaryawanProdRepo)
        //{
        //    _AllKaryawanProdRepo = AllKaryawanProdRepo;
        //}

        //public List<hrd_tUserDTO> CheckGlobalUser(string username,string password)
        //{
        //    //return _loginRepo.CheckGlobalUser(inputs);
        //    var queryFilter = PredicateHelper.True<tUser>();
        //    if (!String.IsNullOrEmpty(username))
        //    {
        //        queryFilter = queryFilter.And(x => x.uName == username);
        //    }

        //    if (!String.IsNullOrEmpty(password))
        //    {
        //        queryFilter = queryFilter.And(x => x.uPwd == password);
        //    }
        //    var dbResult = _repo.Get(queryFilter).ToList();
        //    return Mapper.Map<List<hrd_tUserDTO>>(dbResult);
        //}

        public Prod_All_KaryawanDTO GetFullName(string nik)
        {
            //var queryFilter = PredicateHelper.True<All_Karyawan>();
            //if (!String.IsNullOrEmpty(nik))
            //{
            //    queryFilter = queryFilter.And(x => x.Nomor_Induk == nik);
            //}
            //var dbResult = _AllKaryawanProdRepo.Get(queryFilter).FirstOrDefault();
            //return Mapper.Map<Prod_All_KaryawanDTO>(dbResult);
            using (var context = new ProdEntities())
            {
                All_Karyawan dbResult = context.All_Karyawan.FirstOrDefault(x => x.Nomor_Induk == nik);
                return Mapper.Map<Prod_All_KaryawanDTO>(dbResult);
            }
        }

        public bool GetPositionByRole(string role)
        {
            if (String.IsNullOrEmpty(role)) return false;
            using (var context2 = new basEntities())
            {
                var data = context2.m_position.Where(x => x.pos_description == role).Select(x => x.pos_mobile).FirstOrDefault();
                return Convert.ToBoolean(data.Value);
            }
        }

        public string CheckGlobalUser(string username, string password)
        {
            using (var context = new ProdEntities())
            {
                string dbResult;
                string defPwd = GetDefaultPassword();
                string decPwd = DecryptPassword(password);
                if (decPwd == defPwd)
                {
                    dbResult =
                        context.tUsers.Where(x => x.uName == username)
                            .Select(x => x.Nomor_Induk)
                            .FirstOrDefault();
                }
                else
                {
                    dbResult =
                        context.tUsers.Where(x => x.uName == username && x.userPwd == password)
                            .Select(x => x.Nomor_Induk)
                            .FirstOrDefault();
                }
                return dbResult;
            }
        }

        public bool IsSupervisor(string nik)
        {
            throw new NotImplementedException();
        }

        public bas_v_rep_fullDTO CheckMvaUserInfo(string nik)
        {
            using (var context = new bas_trialEntities())
            {
                v_rep_full dbResult = context.v_rep_full.FirstOrDefault(x => x.rep_id == nik);
                return Mapper.Map<bas_v_rep_fullDTO>(dbResult);
            }
        }

        public bas_m_repDTO CheckMvaUserRole(string nik)
        {
            using (var context = new bas_trialEntities())
            {
                m_rep dbResult = context.m_rep.FirstOrDefault(x => x.rep_id == nik);
                return Mapper.Map<bas_m_repDTO>(dbResult);
            }
        }

        public string GetProfilePic(string nik)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.m_rep.Where(x => x.rep_id == nik).Select(x => x.profile_picture_path).FirstOrDefault();
                return dbResult;
            }
        }

        public string GetUserId(string tokenaccess)
        {
            using (var context = new bas_trialEntities())
            {
                string nik =
                    context.usermobiles.Where(x => x.UserToken == tokenaccess).Select(x => x.rep_id).FirstOrDefault();
                return nik;
            }
        }

        public int InsertAuth(string nik, string auth)
        {
            using (var context = new bas_trialEntities())
            {
                usermobile dbResult = context.usermobiles.FirstOrDefault(x => x.UserToken == auth);
                if (dbResult != null)
                {
                    var existing = Mapper.Map<usermobile>(dbResult);
                    existing.UserToken = auth;
                    context.Entry(existing).State = EntityState.Modified;
                }
                else
                {
                    var data = new usermobile
                    {
                        rep_id = nik,
                        UserToken = auth,
                        CreatedDate = DateTime.Now
                    };
                    context.usermobiles.Add(data);
                }
                return context.SaveChanges();
            }
        }

        public List<UserProfileDTO> getData(string rep_id,string host)
        {
            var bas = new bas_trialEntities();
            return
                bas.v_rep_full_new.Where(x => x.rep_id == rep_id)
                    .Select(
                        x =>
                            new UserProfileDTO
                            {
                                address = x.address,
                                birth_date = x.birth_date,
                                birth_place = x.birth_place,
                                phone = x.phone,
                                ProfilePicture = host + x.ProfilePicture.Substring(1),
                                rep_full_name = x.rep_full_name,
                                rep_id = x.rep_id,
                                rep_position = x.rep_position,
                                rep_region = x.rep_region,
                                rep_sbo = x.rep_sbo
                            })
                    .ToList();
        }

        public int editProfile(string rep_id, string newPass, string picture)
        {
            var bas = new bas_trialEntities();
            var prd = new ProdEntities();
            int result = 1;

            if (!String.IsNullOrEmpty(newPass))
            {
                result =
                    prd.Database.ExecuteSqlCommand("UPDATE tUser SET userPwd = '" + newPass +
                                                   "' WHERE [Nomor Induk] = '" + rep_id + "'");
            }

            if (!String.IsNullOrEmpty(picture))
            {
                //result = bas.Database.ExecuteSqlCommand("UPDATE v_rep_full_new SET ProfilePicture = '" + picture + "' WHERE rep_id = '" + rep_id + "'");
                result =
                    bas.Database.ExecuteSqlCommand("UPDATE m_rep SET profile_picture_path = '" + picture +
                                                   "' WHERE rep_id = '" + rep_id + "'");
            }

            return result;
        }

        public ProfileDoctorDetail getProfileDoctor(string dr_code)
        {
            int temp = Int32.Parse(dr_code);
            var bas = new bas_trialEntities();
            ProfileDoctorDetail profile =
                bas.v_m_doctor.Select(
                    x =>
                        new ProfileDoctorDetail
                        {
                            dr_birthday = x.dr_birthday,
                            dr_bo = x.dr_bo,
                            dr_code = x.dr_code,
                            dr_monitoring = x.dr_monitoring,
                            dr_name = x.dr_name,
                            dr_number_patient = x.dr_number_patient,
                            dr_phone = x.dr_phone,
                            dr_quadrant = x.dr_quadrant,
                            dr_sbo = x.dr_sbo,
                            dr_spec = x.dr_spec
                        }).Where(x => x.dr_code == temp).SingleOrDefault();

            return profile;
        }

        public List<DoctorActivityDTO> getDoctorActivity(string dr_code,int year)
        {
            var bas = new bas_trialEntities();
            return
                bas.Database.SqlQuery<DoctorActivityDTO>("EXEC SP_SELECT_PROFILE_DOCTORE_DETAIL '" + dr_code + "',"+year+" ")
                    .ToList();
        }

       public string GetDefaultPassword()
        {
            using (var context = new bas_trialEntities())
            {
                decimal dbResult =
                    context.C_parameter.Where(x => x.transaction_id == "PASS")
                        .Select(x => x.min_value.Value)
                        .FirstOrDefault();
                int amount = Convert.ToInt32(dbResult);

                return Convert.ToString(amount);
            }
        }

        public string DecryptPassword(string iText)
        {
            string newText = "";
            int counter = iText.Length;
            for (int i = 0; i < counter; i++)
            {
                char charText = Convert.ToChar(iText.Substring(i, 1));
                int ascii = charText;
                string res = "";

                if (ascii >= 192 && ascii <= 217)
                {
                    res = Convert.ToString(Convert.ToChar(ascii - 127));
                }
                else if (ascii >= 218 && ascii <= 243)
                {
                    res = Convert.ToString(Convert.ToChar(ascii - 121));
                }
                else if (ascii >= 244 && ascii <= 253)
                {
                    res = Convert.ToString(Convert.ToChar(ascii - 196));
                }
                else if (ascii == 32)
                {
                    res = Convert.ToString(Convert.ToChar(32));
                }
                else
                {
                    res = Convert.ToString(charText);
                }
                newText = newText + res;
            }
            return newText;
        }

        public void LogLogin(LogLoginInput inputs)
        {
            using (var context = new bas_trialEntities())
            {
                var logLogin = new log_login();
                
                logLogin.rep_id = inputs.rep_id;
                logLogin.hostname = inputs.hostname;
                logLogin.ip_addressv4 = inputs.ip_addressv4;
                //logLogin.ip_addressv6 = inputs.ip_addressv6;
                logLogin.latitude = inputs.latitude;
                logLogin.longitude = inputs.longitude;
                logLogin.address = inputs.address;
                logLogin.source = "IPAD";
                logLogin.log_date = inputs.log_date;
                logLogin.status = inputs.status;
                logLogin.notes = inputs.notes;
                logLogin.created_by = "System";
                logLogin.date_created = DateTime.Now;
                context.log_login.Add(logLogin);
                context.SaveChanges();
            }
            //throw new NotImplementedException();
        }
    }
}