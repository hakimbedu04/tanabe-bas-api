using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.DTOs.HRD;
using SF_Domain.Inputs.SP;
using SF_Domain.Inputs.User;
using SF_Domain.Inputs.Visit;

namespace SF_Repositories.Common
{
    public class SqlSPRepository : ISqlSPRepository
    {
        private readonly bas_trialEntities _basContext;

        public SqlSPRepository(bas_trialEntities basContext)
        {
            _basContext = basContext;
        }

        public void SaveAdditionalVisit(VisitInputs inputs)
        {
            int drcode = Convert.ToInt32(inputs.DrCode);
            string visitid = GetVisitNumber();
            string newvisitid = visitid.Replace(" ", String.Empty);
            _basContext.SP_INSERT_VISIT_REALIZATION_NO_PLAN(newvisitid, inputs.RepId, inputs.VisitDatePlan, drcode,
                inputs.Info, inputs.RepPosition, inputs.visit_type);
        }

        public List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSpRealization(string visitid)
        {
            List<SP_SELECT_VISIT_SP_REALIZATION_Result> dbResult =
                _basContext.SP_SELECT_VISIT_SP_REALIZATION(visitid).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_REALIZATION_DTO>>(dbResult);
            return viewMapper;
        }

        public List<SP_SELECT_VISIT_SP_ASSOCIATED_DTO> GetSPVisitAssociated(string rep_id)
        {
            List<SP_SELECT_VISIT_ASSOCIATED_Result> dbResult =
                _basContext.SP_SELECT_VISIT_ASSOCIATED(rep_id).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_ASSOCIATED_DTO>>(dbResult);
            return viewMapper;
        }

        public List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSpPlan(string visitid)
        {
            List<SP_SELECT_VISIT_SP_PLAN_Result> dbResult = _basContext.SP_SELECT_VISIT_SP_PLAN(visitid).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_DTO>>(dbResult);
            return viewMapper;
        }

        public void DeleteVisitPlan(VisitInputs inputs)
        {
            _basContext.SP_DELETE_VISIT_PLAN(inputs.VisitId, inputs.RepPosition);
        }

        public void DeleteVisitProduct(VisitInputs inputs)
        {
            _basContext.SP_DELETE_PRODUCT_VISIT(inputs.VdId);
        }

        public List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid)
        {
            List<SP_SELECT_SP_ATTACHMENT_Result> dbResult = _basContext.SP_SELECT_SP_ATTACHMENT(visitid).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_SP_ATTACHMENT_DTO>>(dbResult);
            return viewMapper;
        }

        public int CheckMaxVisit(VisitInputs inputs)
        {
            int? dbResult = _basContext.SP_INSERT_ADDITIONAL_VISIT(inputs.RepId, inputs.VisitDatePlan).FirstOrDefault();
            return dbResult.HasValue ? dbResult.Value : 0; // != 0 ? 1 : 0;
        }

        public bool CheckNewMaxVisit(VisitInputs inputs)
        {
            var dbResult = _basContext.SP_CHECK_MAX_ADDITIONAL_VISIT(inputs.RepId, inputs.VisitDatePlan).FirstOrDefault();
            //return dbResult.HasValue ? dbResult : false;
            if(dbResult == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> GetProductTopicList(int vdid)
        {
            List<SP_SELECT_PRODUCT_TOPIC_LIST_Result> dbResult = _basContext.SP_SELECT_PRODUCT_TOPIC_LIST(vdid).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO>>(dbResult);
            return viewMapper;
        }

        public List<SP_SELECT_DOCTOR_LIST_DTO> GetDataDoctorList(VisitInputs inputs)
        {
            List<SP_SELECT_DOCTOR_LIST_Result> dbResult =
                _basContext.SP_SELECT_DOCTOR_LIST(inputs.RepId, inputs.RepPosition).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_DTO>>(dbResult);
            return viewMapper;
        }

        public List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductList()
        {
            List<SP_SELECT_PRODUCT_VISIT_Result> dbResult = _basContext.SP_SELECT_PRODUCT_VISIT().ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_VISIT_DTO>>(dbResult);
            return viewMapper;
        }

        public bool InsertVisitProduct(VisitInputs inputs)
        {
            try
            {
                _basContext.SP_INSERT_PRODUCT_VISIT(inputs.RepId, inputs.VisitId, inputs.VisitCode, inputs.Sp,
                    inputs.Percentage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateVisitProduct(VisitInputs inputs)
        {
            _basContext.SP_UPDATE_VISIT_PRODUCT(inputs.VdId, inputs.VisitCode, inputs.Sp, inputs.Percentage);
        }

        public List<SP_SELECT_PRODUCT_USER_DTO> dsProductLookup()
        {
            ObjectResult<SP_SELECT_PRODUCT_USER_Result> dbResult = _basContext.SP_SELECT_PRODUCT_USER();
            var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_USER_DTO>>(dbResult);
            return viewMapper;
        }

        public void SaveReportPlan(string id)
        {
            DateTime currentDate = DateTime.Now;
            int month = currentDate.Month;
            int year = currentDate.Year;
            _basContext.SP_INSERT_TRANSACT_EMAIL(month, year, "RVP", id, currentDate);
        }

        public bool InsertProductTopic(int vdid, int topicid)
        {
            try
            {
                int dbResult = _basContext.SP_INSERT_PRODUCT_TOPIC(vdid, topicid);
                return (dbResult == 1);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void InsertSPAttachment(string visitid, string filename, string filepath)
        {
            _basContext.SP_INSERT_SP_ATTACHMENT_MOBILE(visitid, filename, filepath);
        }

        public bool SaveRealizationVisit(VisitInputs inputs)
        {
            try
            {
                //_basContext.SP_INSERT_VISIT_REALIZATION(inputs.VisitId, inputs.RepId, inputs.Info, inputs.Visited, inputs.SPRealizationStat, inputs.RealAmount, inputs.EName, inputs.EPlace);
                ////return true;
                //List<SP_INSERT_VISIT_REALIZATION> dbResult = new List<SP_INSERT_VISIT_REALIZATION>();
                _basContext.Database.SqlQuery<SP_INSERT_VISIT_REALIZATION>("SP_INSERT_VISIT_REALIZATION_MOBILE '" +
                                                                           inputs.VisitId + "', '" + inputs.RepId +
                                                                           "', '" + inputs.Info + "', " + inputs.Visited +
                                                                           ", " + inputs.SPRealizationStat + ", " +
                                                                           (inputs.RealAmount.HasValue
                                                                               ? inputs.RealAmount.Value
                                                                               : 0) + ", '" + inputs.EName + "', '" +
                                                                           inputs.EPlace + "', '" +
                                                                           inputs.visit_type + "'").ToList();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_SELECT_SPR_INFO_DTO> GetSprInformation(string visitid)
        {
            ObjectResult<SP_SELECT_SPR_INFO_Result> dbResult = _basContext.SP_SELECT_SPR_INFO(visitid);
            var viewMapper = Mapper.Map<List<SP_SELECT_SPR_INFO_DTO>>(dbResult);
            return viewMapper;
        }

        public bool SubmitAdditionalRealizationVisit(VisitInputs inputs)
        {
            try
            {
                _basContext.SP_INSERT_SP_2_ON_VISIT_REALIZATION_ADDITIONAL_MOBILE(inputs.VisitId, inputs.Info,
                    inputs.SpBa, inputs.RealAmount, inputs.EName, inputs.EPlace, inputs.ListAttachment, inputs.visit_type);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateVisitPlanByCancellation(VisitInputs inputs)
        {
            DateTime currentDate = DateTime.Now.Date;
            int res = 0;
            foreach (string visitid in inputs.Collections.Select(data => data.Code))
            {
                res = _basContext.SP_UPDATE_VISIT_PLAN_CANCELLATION(visitid, inputs.ConditionText, currentDate,
                    inputs.RepPosition, inputs.RepId);
            }
            return res == 1;
        }

        public bool UpdateVisitPlanByDate(VisitInputs inputs)
        {
            try
            {
                DateTime visitPlanDate = inputs.VisitDateTime.Value.Date;
                DateTime currentDate = DateTime.Now.Date;
                _basContext.SP_UPDATE_VISIT_PLAN_BY_DATE(inputs.RepId, inputs.RepPosition, inputs.ConditionText,
                    currentDate, visitPlanDate);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void AddDetailProduct(VisitInputs inputs)
        {
            DateTime currentDate = DateTime.Now.Date;
            _basContext.SP_INSERT_VISIT_PRODUCT(inputs.VisitId, inputs.VisitCode, currentDate);
        }

        public void InsertNotVisitedByDefault(string visitid)
        {
            DateTime currentDate = DateTime.Now.Date;
            _basContext.SP_INSERT_VISIT_PRODUCT(visitid, "T0", currentDate);
        }

        public bool SaveRealizationVisitWithAdditionalSP(VisitInputs inputs)
        {
            try
            {
                _basContext.SP_INSERT_SP_2_ON_VISIT_REALIZATION_MOBILE(inputs.VisitId, inputs.RepId, inputs.Info,
                    inputs.Visited, inputs.RealAmount, inputs.SpBa, inputs.EName, inputs.EPlace, inputs.ListAttachment, inputs.visit_type);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExecUpdate(VisitInputs inputs)
        {
            try
            {
                int drcode = Convert.ToInt32(inputs.DrCode);
                _basContext.SP_UPDATE_REALIZATION_VISIT_ACTUAL_MOBILE(inputs.VisitId, drcode, inputs.VisitPlan,
                    inputs.VisitReal, inputs.Info, inputs.VisitSp, inputs.RealAmount);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistory(VisitInputs inputs)
        {
            ObjectResult<SP_SELECT_FINISHED_VISIT_MOBILE_Result> dbResult = _basContext.SP_SELECT_FINISHED_VISIT_MOBILE(inputs.RepId,inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO>>(dbResult);
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    viewMapper = viewMapper.Where(r => r.GetType()
                        .GetProperty(arrColumn[i])
                        .GetValue(r, null)
                        .ToString().ToLower()
                        .Contains(arrSearch[i].ToLower()))
                        .ToList();
                }
            }

            return viewMapper;
        }

        public List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistorySearch(VisitInputs inputs)
        {
            ObjectResult<SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE_Result> dbResult =
                _basContext.SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE(inputs.RepId, inputs.Month, inputs.Year,
                    inputs.SearchText);
            var viewMapper = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO>>(dbResult);
            return viewMapper;
        }

        #region SP PLAN

        public List<SP_SELECT_SP_PLAN_DTO> GetSpPlan(SPInputs inputs)
        {
            List<SP_SELECT_SP_PLAN_Result> dbResult =
                _basContext.SP_SELECT_SP_PLAN(inputs.RepId, inputs.Month, inputs.Year).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_DTO>>(dbResult);
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    viewMapper = viewMapper.Where(r => r.GetType()
                        .GetProperty(arrColumn[i])
                        .GetValue(r, null)
                        .ToString().ToLower()
                        .Contains(arrSearch[i].ToLower()))
                        .ToList();
                }
            }

            return viewMapper;
        }

        public List<SP_PRODUCT_SP_PLAN_MOBILE_DTO> GetProductDropdown()
        {
            ObjectResult<SP_PRODUCT_SP_PLAN_MOBILE_Result> dbResult = _basContext.SP_PRODUCT_SP_PLAN_MOBILE();
            return Mapper.Map<List<SP_PRODUCT_SP_PLAN_MOBILE_DTO>>(dbResult);
        }

        public void AddDetailProduct(string sprid, string productid)
        {
            _basContext.SP_INSERT_PRODUCT_SP_PLAN(sprid, productid);
        }

        public void DeleteProduct(int sppid)
        {
            string id = Convert.ToString(sppid);
            _basContext.SP_DELETE_PRODUCT_SP_PLAN(id);
        }

        public List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO> GetDoctor(SPInputs inputs)
        {
            List<SP_SELECT_DOCTOR_LIST_SPEAKER_Result> dbResult =
                _basContext.SP_SELECT_DOCTOR_LIST_SPEAKER(inputs.RepId, inputs.RepPosition).ToList();
            var mapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO>>(dbResult);
            return mapper;
        }

        public List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorListFiltered(SPInputs inputs)
        {
            List<SP_SELECT_DOCTOR_LIST_SP_Result> dbResult =
                _basContext.SP_SELECT_DOCTOR_LIST_SP(inputs.RepId, inputs.RepPosition).ToList();
            //var mapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_SP_DTO>>(dbResult);
            var res = new List<SP_SELECT_DOCTOR_LIST_SP_DTO>();
            foreach (SP_SELECT_DOCTOR_LIST_SP_Result viewMapper in dbResult)
            {
                var data = new SP_SELECT_DOCTOR_LIST_SP_DTO
                {
                    dr_code = viewMapper.dr_code,
                    sbo_id = viewMapper.sbo_id,
                    dr_sbo = viewMapper.dr_sbo,
                    dr_bo = viewMapper.dr_bo,
                    dr_region = viewMapper.dr_region,
                    dr_rep = viewMapper.dr_rep,
                    rep_name = viewMapper.rep_name,
                    dr_am = viewMapper.dr_am,
                    am_name = viewMapper.am_name,
                    rep_position = viewMapper.rep_position,
                    dr_name = viewMapper.dr_name,
                    dr_spec = viewMapper.dr_spec,
                    dr_sub_spec = viewMapper.dr_sub_spec,
                    dr_quadrant = viewMapper.dr_quadrant,
                    dr_monitoring = viewMapper.dr_monitoring,
                    dr_address = viewMapper.dr_address,
                    dr_area_mis = viewMapper.dr_area_mis,
                    dr_sum = viewMapper.dr_sum.HasValue?viewMapper.dr_sum.Value:0,
                    dr_category = viewMapper.dr_category,
                    dr_sub_category = viewMapper.dr_sub_category,
                    dr_chanel = viewMapper.dr_chanel,
                    dr_day_visit = viewMapper.dr_day_visit,
                    dr_visiting_hour = viewMapper.dr_visiting_hour,
                    dr_number_patient = viewMapper.dr_number_patient.HasValue ? viewMapper.dr_number_patient.Value : 0,
                    dr_kol_not = viewMapper.dr_kol_not,
                    dr_gender = viewMapper.dr_gender,
                    dr_phone = viewMapper.dr_phone,
                    dr_email = viewMapper.dr_email,
                    dr_birthday = viewMapper.dr_birthday,
                    dr_dk_lk = viewMapper.dr_dk_lk,
                    dr_used_session = viewMapper.dr_used_session,
                    is_used = viewMapper.is_used,
                    dr_used_remaining = viewMapper.dr_used_remaining,
                    dr_used_month_session = viewMapper.dr_used_month_session,
                    dr_status = viewMapper.dr_status,
                    is_used_on_sales = viewMapper.is_used_on_sales,
                    dr_sales_session = viewMapper.dr_sales_session,
                    dr_sales_month_session = viewMapper.dr_sales_month_session,
                    dr_ppm = viewMapper.dr_ppm,
                    ppm_name = viewMapper.ppm_name,
                    cust_id = viewMapper.cust_id,
                    dr_rm = viewMapper.dr_rm,
                    dr_rm_name = viewMapper.dr_rm_name
                };
                res.Add(data);
            }
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    res =
                        res.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString().ToLower()
                                    .Contains(arrSearch[i].ToLower())).ToList();
            }
            return res;
        }

        public void AddDetailParticipant(SPInputs inputs)
        {
            _basContext.SP_INSERT_PARTICIPANT_EDIT(inputs.SprId, inputs.SpType, inputs.SpId, inputs.DrCode,
                inputs.SponsorId, inputs.BudgetPlanValue);
        }

        public void DeleteSpeaker(SPInputs inputs)
        {
            _basContext.SP_DELETE_SPEAKER_PLAN(inputs.SpdsId);
        }

        public void UpdateDetailSpeaker(SPInputs inputs)
        {
            _basContext.SP_UPDATE_SPEAKER_EDIT(inputs.SpdsId, inputs.SponsorId, inputs.BudgetPlanValue);
        }

        public List<SP_PRODUCT_VISIT_MOBILE_DTO> GetDataProductSPList()
        {
            List<SP_PRODUCT_VISIT_MOBILE_Result> dbResult = _basContext.SP_PRODUCT_VISIT_MOBILE().ToList();
            var viewMapper = Mapper.Map<List<SP_PRODUCT_VISIT_MOBILE_DTO>>(dbResult);
            return viewMapper;
        }

        public void InsertSP1(SPInputs inputs)
        {
            try
            {
                string listProduct = Convert.ToString(inputs.ProductXmlDoc);
                _basContext.SP_INSERT_SP_1(inputs.SprId, inputs.RepId, inputs.SpType, inputs.DateStart, inputs.DateEnd,
                    inputs.EName, inputs.ETopic, inputs.EPlace, inputs.GeneralSponsor, inputs.BAmount,
                    inputs.Participant, inputs.Notes,
                    listProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertSP2(SPInputs inputs)
        {
            try
            {
                string listProduct = Convert.ToString(inputs.ProductXmlDoc);
                _basContext.SP_INSERT_SP_2(inputs.SprId, inputs.RepId, inputs.SpType, inputs.DateStart, inputs.DateEnd,
                    inputs.Products,
                    inputs.EName, inputs.ETopic, inputs.EPlace, inputs.GeneralSponsor, inputs.BAmount,
                    inputs.Participant, inputs.Notes, inputs.BAllocation,
                    listProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSprId()
        {
            ObjectResult<string> dbResult = _basContext.SP_GET_NEW_SPR_ID();
            return dbResult.FirstOrDefault();
        }

        public void DeleteSPPlan(SPInputs inputs)
        {
            _basContext.SP_DELETE_SP_PLAN(inputs.SprId);
        }

        public bool UpdateEventDetail(SPInputs inputs)
        {
            try
            {
                _basContext.SP_UPDATE_SP_PLAN(inputs.SprId, inputs.EName, inputs.ETopic, inputs.EPlace, inputs.DateStart,
                    inputs.DateEnd, inputs.Notes);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public bool UpdateBudgetAllocationOnPlan(SPInputs inputs)
        {
            try
            {
                _basContext.SP_UPDATE_BA_ON_PLAN(inputs.SprId, inputs.SpBa);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public string GetOwnBudgetRemaining(SPInputs inputs)
        {
            try
            {
                var res = _basContext.SP_GET_OWN_BUDGET_REMAINING(inputs.Year, inputs.Month, inputs.SpType,inputs.RepId).FirstOrDefault();
                var formats = string.Format(System.Globalization.CultureInfo.GetCultureInfo("de-DE"), "{0:N2}", res);
                return formats;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion

        #region SP Realization

        public List<SP_SELECT_SP_REALIZATION_DTO> GetSPRealization(SPInputs inputs)
        {
            List<SP_SELECT_SP_REALIZATION_Result> dbResult =
                _basContext.SP_SELECT_SP_REALIZATION(inputs.RepId, inputs.Month, inputs.Year).ToList();
            var viewMapper = Mapper.Map<List<SP_SELECT_SP_REALIZATION_DTO>>(dbResult);
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    viewMapper = viewMapper.Where(r => r.GetType()
                        .GetProperty(arrColumn[i])
                        .GetValue(r, null)
                        .ToString().ToLower()
                        .Contains(arrSearch[i].ToLower()))
                        .ToList();
            }


            return viewMapper;
        }

        public List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorList(SPInputs inputs)
        {
            ObjectResult<SP_SELECT_DOCTOR_LIST_SP_Result> dbResult = _basContext.SP_SELECT_DOCTOR_LIST_SP(inputs.RepId,
                inputs.RepPosition);
            var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_SP_DTO>>(dbResult);
            return viewMapper;
        }

        public bool AddSpeakerAdditional(SPInputs inputs)
        {
            try
            {
                int dbResult = _basContext.SP_INSERT_SPEAKER_ADDITIONAL(inputs.SprId, inputs.SpId, inputs.DrCode,
                    inputs.SponsorId, inputs.BudgetRealValue);
                if (dbResult > 0 || dbResult != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return false;
        }

        public bool UpdateRealization(Collection inputs)
        {
            try
            {
                int dbResult = _basContext.SP_SAVE_SP_REALIZATION(inputs.SpdsId, inputs.SponsorId, inputs.DrActual,
                    inputs.BudgetRealValue);
                if (dbResult > 0 || dbResult != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return false;
        }

        public bool SubmitRealization(SPInputs inputs)
        {
            try
            {
                int dbResult = _basContext.SP_SUBMIT_SP_REALIZATION(inputs.SpId, inputs.RealDateTime,
                    inputs.RealizationState, inputs.EPlace);
                if (dbResult > 0 || dbResult != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return false;
        }

        #endregion

        #region User

        public void InsertSalesProductActual(UserInputs inputs)
        {
            _basContext.SP_INSERT_SALES_PRODUCT_ACTUAL_MOBILE(inputs.SpId, inputs.SpaDate, inputs.SpaQuantity,
                inputs.SpaNote);
        }

        public void UpdateSalesProductActual(UserInputs inputs)
        {
            _basContext.SP_UPDATE_SALES_PRODUCT_ACTUAL_MOBILE(inputs.SpaId, inputs.SpId, inputs.SpaDate,
                inputs.SpaQuantity, inputs.SpaNote);
        }

        public void DeleteSalesProductActual(UserInputs inputs)
        {
            string spaid = Convert.ToString(inputs.SpaId);
            _basContext.SP_DELETE_SALES_PRODUCT_ACTUAL_MOBILE(spaid);
        }

        public List<v_sales_product_DTO> GetUserActualSearch(UserInputs inputs)
        {
            ObjectResult<SP_SELECT_USER_ACTUAL_SEARCH_MOBILE_Result> dbResult =
                _basContext.SP_SELECT_USER_ACTUAL_SEARCH_MOBILE(inputs.RepId, inputs.Month, inputs.Year,
                    inputs.TextSearch);
            var viewMapper = Mapper.Map<List<v_sales_product_DTO>>(dbResult);
            return viewMapper;
        }

        #endregion

        #region download data offline

        public List<t_visit_DTO> OfflineTVisit(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_visit_Result> dbResult = _basContext.sp_offline_t_visit(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_visit_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_customer_aso_DTO> OfflineMCustomerAso(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_customer_aso_Result> dbResult = _basContext.sp_offline_m_customer_aso(
                inputs.RepId, inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<m_customer_aso_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_doctor_DTO> OfflineMDoctor(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_doctor_Result> dbResult = _basContext.sp_offline_m_doctor(inputs.RepId,
                inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<m_doctor_DTO>>(dbResult);
            return viewMapper;
        }

        public List<sp_offline_t_visit_product_DTO> OfflineTVisitProduct(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_visit_product_Result> dbResult =
                _basContext.sp_offline_t_visit_product(inputs.RepId, inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<sp_offline_t_visit_product_DTO>>(dbResult);
            return viewMapper;
        }

        public List<sp_offline_t_visit_product_topic_DTO> OfflineTVisitProductTopic(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_visit_product_topic_Result> dbResult =
                _basContext.sp_offline_t_visit_product_topic(inputs.RepId, inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<sp_offline_t_visit_product_topic_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_bo_DTO> OfflineMBo(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_bo_Result> dbResult = _basContext.sp_offline_m_bo(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_bo_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_regional_DTO> OfflineMRegional(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_regional_Result> dbResult = _basContext.sp_offline_m_regional(inputs.RepId,
                inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<m_regional_DTO>>(dbResult);
            return viewMapper;
        }

        public List<Karyawan_DTO> OfflineKaryawan(VisitInputs inputs)
        {
            ObjectResult<sp_offline_Karyawan_Result> dbResult = _basContext.sp_offline_Karyawan(inputs.RepId,
                inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<Karyawan_DTO>>(dbResult);
            return viewMapper;
        }

        public List<Bagian_DTO> OfflineBagian(VisitInputs inputs)
        {
            ObjectResult<sp_offline_Bagian_Result> dbResult = _basContext.sp_offline_Bagian(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<Bagian_DTO>>(dbResult);
            return viewMapper;
        }

        public List<Jabatan_DTO> OfflineJabatan(VisitInputs inputs)
        {
            ObjectResult<sp_offline_Jabatan_Result> dbResult = _basContext.sp_offline_Jabatan(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<Jabatan_DTO>>(dbResult);
            return viewMapper;
        }

        public List<HeadQuarter_DTO> OfflineHeadQuarter(VisitInputs inputs)
        {
            ObjectResult<sp_offline_HeadQuarter_Result> dbResult = _basContext.sp_offline_HeadQuarter(inputs.RepId,
                inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<HeadQuarter_DTO>>(dbResult);
            return viewMapper;
        }

        public List<Departemen_DTO> OfflineDepartemen(VisitInputs inputs)
        {
            ObjectResult<sp_offline_Departemen_Result> dbResult = _basContext.sp_offline_Departemen(inputs.RepId,
                inputs.Month, inputs.Year);
            var viewMapper = Mapper.Map<List<Departemen_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_rep_DTO> OfflineMRep(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_rep_Result> dbResult = _basContext.sp_offline_m_rep(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_rep_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_sbo_DTO> OfflineMSbo(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_sbo_Result> dbResult = _basContext.sp_offline_m_sbo(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_sbo_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_topic_DTO> OfflineMTopic(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_topic_Result> dbResult = _basContext.sp_offline_m_topic(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_topic_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_gl_cs_DTO> OfflineMGlCs(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_gl_cs_Result> dbResult = _basContext.sp_offline_m_gl_cs(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_gl_cs_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_product_DTO> OfflineMProduct(VisitInputs inputs)
        {
            ObjectResult<sp_offline_m_product_Result> dbResult = _basContext.sp_offline_m_product(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_product_DTO>>(dbResult);
            return viewMapper;
        }

        public List<sp_offline_t_signature_mobile_DTO> OfflineTSignature(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_signature_mobile_Result> dbResult = _basContext.sp_offline_t_signature_mobile(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<sp_offline_t_signature_mobile_DTO>>(dbResult);
            return viewMapper;
        }

        public List<t_spr_DTO> OfflineTSpr(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_spr_Result> dbResult = _basContext.sp_offline_t_spr(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_spr_DTO>>(dbResult);
            return viewMapper;
        }

        public List<t_sp_DTO> OfflineTSp(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_t_sp(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_sp_DTO>>(dbResult);
            return viewMapper;
        }

        public List<t_sp_doctor_DTO> OfflineTSpDoctor(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_t_sp_doctor(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_sp_doctor_DTO>>(dbResult);
            return viewMapper;
        }

        public List<t_sp_sponsor_DTO> OfflineTSpSponsor(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_t_sp_sponsor(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_sp_sponsor_DTO>>(dbResult);
            return viewMapper;
        }

        public List<sp_offline_t_gps_mobile_DTO> OfflineTGps(VisitInputs inputs)
        {
            ObjectResult<sp_offline_t_gps_mobile_Result> dbResult = _basContext.sp_offline_t_gps_mobile(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<sp_offline_t_gps_mobile_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_event_DTO> OfflineMEvent(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_m_event(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_event_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_sponsor_DTO> OfflineMSponsor(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_m_sponsor(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_sponsor_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_status_DTO> OfflineMStatus(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_m_status(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_status_DTO>>(dbResult);
            return viewMapper;
        }

        public List<t_info_feedback_topic_mapping_mobile_DTO> OfflineInfoFeedbackMapping(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_t_info_feedback_topic_mapping_mobile(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<t_info_feedback_topic_mapping_mobile_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_info_feedback_mobile_DTO> OfflineInfoFeedback(VisitInputs inputs)
        {
            var dbResult = _basContext.sp_offline_m_info_feedback_mobile(inputs.RepId, inputs.Month,
                inputs.Year);
            var viewMapper = Mapper.Map<List<m_info_feedback_mobile_DTO>>(dbResult);
            return viewMapper;
        }

        public int CheckSignatureMessage(VisitInputs inputs)
        {
            var result = _basContext.SP_CHECK_TOPIC_AVAIL(inputs.VisitId).FirstOrDefault();
            return (int) result;
        }

        #endregion

        public string GetVisitNumber()
        {
            return _basContext.SP_GET_NEW_VISIT_NUMBER().FirstOrDefault();
        }

    }
}