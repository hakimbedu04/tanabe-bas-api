using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AutoMapper;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_WebApi.Models.BAS;
using SF_WebApi.Models.BAS.SP;
using SF_WebApi.Models.BAS.User;
using SF_WebApi.Models.HRD;

namespace SF_WebApi.MapperViewModel
{
    public class ViewMappingProfiles : Profile
    {
        public ViewMappingProfiles()
        {
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("host", typeof(String));
            #region hrd
            CreateMap<tUserModel, hrd_tUserDTO>().ReverseMap();
            
            #endregion
            #region bas
            CreateMap<m_rep_DTO, m_rep_viewmodel>().ReverseMap();
            CreateMap<SP_SelectVisitPlanViewModel, SP_SelectVisitPlanDTO>().ReverseMap();
            CreateMap<v_visit_plan_mobileViewModel, v_visit_plan_mobileDTO>().ReverseMap();
            CreateMap<SP_SELECT_VISIT_USER_PRODUCT_ViewModel, SP_SELECT_VISIT_USER_PRODUCT_DTO>().ReverseMap();
            CreateMap<v_visit_product_ViewModel, v_visit_product_DTO>().ReverseMap();
            CreateMap<v_visit_plan_ViewModel, v_visit_plan_DTO>().ReverseMap();
            CreateMap<SP_SELECT_VISIT_SP_REALIZATION_ViewModel, SP_SELECT_VISIT_SP_REALIZATION_DTO>().ReverseMap();
            CreateMap<v_visit_product_topic_ViewModel, v_visit_product_topic_DTO>().ReverseMap();
            CreateMap<t_visit_product_topic_ViewModel, t_visit_product_topic_DTO>().ReverseMap();
            CreateMap<SP_SELECT_SP_ATTACHMENT_ViewModel, SP_SELECT_SP_ATTACHMENT_DTO>().ReverseMap();
            CreateMap<SP_SELECT_PRODUCT_VISIT_ViewModel, SP_SELECT_PRODUCT_VISIT_DTO>().ReverseMap();
            CreateMap<m_product_DTO, m_product_ViewModel>().ReverseMap();
            CreateMap<m_product_custom_DTO, m_product_custom_ViewModel>().ReverseMap();
            CreateMap<SP_SELECT_PRODUCT_USER_DTO, SP_SELECT_PRODUCT_USER_ViewModel>().ReverseMap();
            CreateMap<m_event_DTO, m_event_ViewModel>().ReverseMap();
            CreateMap<SummaryDoctor_DTO, SummaryDoctor_ViewModel>().ReverseMap();
            CreateMap<v_info_feedback_DTO, v_info_feedback_ViewModel>().ReverseMap();
            CreateMap<TopRankDTO, TopRankDTO>().ReverseMap();
            CreateMap<SP_SELECT_FINISHED_VISIT_DTO, SP_SELECT_FINISHED_VISIT_ViewModel>().ReverseMap();
            CreateMap<v_visit_plan_new_ViewModel, v_visit_plan_new_DTO>()
                                .ReverseMap();
            

            #region SP PLAN
            CreateMap<SP_SELECT_SP_PLAN_ViewModel, SP_SELECT_SP_PLAN_DTO>().ReverseMap();
            CreateMap<t_sales_product_actual_ViewModel, t_sales_product_actual_DTO>().ReverseMap();
            CreateMap<t_sp_approval_ViewModel, t_sp_approval_DTO>().ReverseMap();
            CreateMap<SP_PRODUCT_SP_PLAN_MOBILE_ViewModel, SP_PRODUCT_SP_PLAN_MOBILE_DTO>().ReverseMap();
            CreateMap<v_doctor_sponsor_ViewModel, v_doctor_sponsor_DTO>().ReverseMap();
            CreateMap<SP_SELECT_DOCTOR_LIST_SPEAKER_ViewModel, SP_SELECT_DOCTOR_LIST_SPEAKER_DTO>().ReverseMap();
            CreateMap<m_sponsor_ViewModel, m_sponsor_DTO>().ReverseMap();
            CreateMap<v_auth_sp_ViewModel, v_auth_sp_DTO>().ReverseMap();
            CreateMap<SP_PRODUCT_VISIT_MOBILE_ViewModel, SP_PRODUCT_VISIT_MOBILE_DTO>().ReverseMap();
            CreateMap<m_topic_ViewModel, m_topic_DTO>().ReverseMap();
            CreateMap<SP_SELECT_MASTER_DOCTOR_PIVOT_ViewModel, SP_SELECT_MASTER_DOCTOR_PIVOT_DTO>().ReverseMap();
            
            
            #endregion

            #endregion

            #region prod

            #endregion
        }
    }
}