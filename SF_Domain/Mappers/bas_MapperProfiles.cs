using System;
using System.Configuration;
using System.Text.RegularExpressions;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;

namespace SF_Domain.Mappers
{
    public class bas_MapperProfiles : Profile
    {
        public Regex GetStringPattern()
        {
            var pattern = new Regex("[~;,\t\r ]|[\n]{2}");
            return pattern;
        }
        public bas_MapperProfiles()
        {
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("host2", typeof(String));
            var keyhttp = (string)settingsReader.GetValue("host", typeof(String));
            CreateMap<TopRankDTO, TopRankDTO>().PreserveReferences().ReverseMap();
            CreateMap<bas_v_rep_fullDTO, v_rep_full>().PreserveReferences().ReverseMap();
            CreateMap<bas_m_repDTO, m_rep>()
                .ForMember(dest => dest.profile_picture_path,
                act => act.MapFrom(src => (String.IsNullOrEmpty(src.profile_picture_path) ? "" : key.Replace("bas_api_mobile", "bas") + src.profile_picture_path.Replace(" ", "%20").Substring(1))))
                .ReverseMap();
            CreateMap<SP_SelectVisitPlanDTO, SP_SELECT_VISIT_PLAN_Result>().ReverseMap();
            CreateMap<v_visit_plan_mobileDTO, v_visit_plan_mobile>()
                .ForMember(dest => dest.dr_spec, act => act.MapFrom(src => (src.dr_spec ?? "")))
                .ForMember(dest => dest.dr_sub_spec, act => act.MapFrom(src => (src.dr_sub_spec ?? "")))
                .ForMember(dest => dest.dr_quadrant, act => act.MapFrom(src => (src.dr_quadrant ?? "")))
                .ForMember(dest => dest.dr_monitoring, act => act.MapFrom(src => (src.dr_monitoring ?? "")))
                .ReverseMap();
            CreateMap<SP_SELECT_DOCTOR_LIST_NEW_DTO, SP_SELECT_DOCTOR_LIST_NEW_Result>()
                .ForMember(dest => dest.dr_spec, act => act.MapFrom(src => (src.dr_spec ?? "")))
                .ForMember(dest => dest.dr_sub_spec, act => act.MapFrom(src => (src.dr_sub_spec ?? "")))
                .ForMember(dest => dest.dr_quadrant, act => act.MapFrom(src => (src.dr_quadrant ?? "")))
                .ForMember(dest => dest.dr_monitoring, act => act.MapFrom(src => (src.dr_monitoring ?? "")))
                .ReverseMap();
            CreateMap<SP_SELECT_VISIT_USER_PRODUCT_DTO, SP_SELECT_VISIT_USER_PRODUCT_Result>().ReverseMap();
            CreateMap<t_sales_DTO, t_sales>().ReverseMap();
            CreateMap<SP_SELECT_VISIT_SP_PLAN_DTO, SP_SELECT_VISIT_SP_PLAN_Result>().ReverseMap();
            CreateMap<v_visit_product, v_visit_product_DTO>()
                .ForMember(src => src.vd_value,
                    opt => opt.MapFrom(dest => dest.vd_value.HasValue ? dest.vd_value.Value : 0))
                .ForMember(src => src.sp_sp, opt => opt.MapFrom(dest => dest.sp_sp.HasValue ? dest.sp_sp.Value : 0))
                .ForMember(src => src.sp_percentage,
                    opt => opt.MapFrom(dest => dest.sp_percentage.HasValue ? dest.sp_percentage.Value : 0))
                .ReverseMap();
            CreateMap<SP_SELECT_VISIT_SP_REALIZATION_DTO, SP_SELECT_VISIT_SP_REALIZATION_Result>().ReverseMap();
            CreateMap<t_visit_product_topic_DTO, t_visit_product_topic>().ReverseMap();
            
            var filePath = (string)settingsReader.GetValue("TopicFiles", typeof(String));
            //CreateMap<v_visit_product_topic_DTO, v_visit_product_topic>()
            //    .ForMember(src => src.vpt_feedback, opt => opt.MapFrom(dest => 0))
            //    //.ForMember(dest => dest.topic_filepath,
            //    //act => act.MapFrom(src => (String.IsNullOrEmpty(src.topic_filepath) ? GetStringPattern().Replace(filePath, "") + "no_available_image.jpg" : (key.Replace("bas_api_mobile", "bas") + GetStringPattern().Replace(src.topic_filepath, "")))))
            //    //act => act.MapFrom(src => (String.IsNullOrEmpty(src.topic_filepath) ? GetStringPattern().Replace(filePath, "") + "no_available_image.jpg" : "")))
            //    .ReverseMap();
            CreateMap<SP_SELECT_SP_ATTACHMENT_DTO, SP_SELECT_SP_ATTACHMENT_Result>().ReverseMap();
            CreateMap<SP_SELECT_PRODUCT_TOPIC_LIST_DTO, SP_SELECT_PRODUCT_TOPIC_LIST_Result>().ReverseMap();
            CreateMap<SP_SELECT_DOCTOR_LIST_DTO, SP_SELECT_DOCTOR_LIST_Result>().ReverseMap();
            CreateMap<SP_SELECT_PRODUCT_VISIT_DTO, SP_SELECT_PRODUCT_VISIT_Result>().ReverseMap();
            CreateMap<t_sp_attachment_DTO, t_sp_attachment>().ReverseMap();
            CreateMap<m_product_DTO, m_product>().ReverseMap();
            CreateMap<m_product_custom_DTO, m_product>()
                .ForMember(src => src.visit_code, opt => opt.MapFrom(dest => dest.visit_code))
                .ForMember(src => src.visit_team, opt => opt.MapFrom(dest => dest.visit_team))
                .ForMember(src => src.visit_product, opt => opt.MapFrom(dest => dest.visit_product))
                .ForMember(src => src.visit_category, opt => opt.MapFrom(dest => dest.visit_category)).ReverseMap();
            CreateMap<SP_SELECT_PRODUCT_USER_DTO, SP_SELECT_PRODUCT_USER_Result>().ReverseMap();
            CreateMap<SP_SELECT_SPR_INFO_DTO, SP_SELECT_SPR_INFO_Result>().ReverseMap();
            CreateMap<v_rep_admin_DTO, v_rep_admin>().ReverseMap();
            CreateMap<m_event_DTO, m_event>().ReverseMap();
            CreateMap<v_info_feedback_DTO, v_info_feedback>().ReverseMap();
            CreateMap<SP_SELECT_FINISHED_VISIT_MOBILE_DTO, SP_SELECT_FINISHED_VISIT_Result>().ReverseMap();
            CreateMap<SP_SELECT_FINISHED_VISIT_MOBILE_DTO, SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE_Result>()
                .ForMember(dest => dest.ttd_file_path,
                act => act.MapFrom(src => (String.IsNullOrEmpty(src.ttd_file_path) ? "" : key.Replace("bas_api_mobile", "bas") + src.ttd_file_path.Replace(" ", "%20").Substring(1))))
                                .ReverseMap();

            CreateMap<v_visit_plan_new, v_visit_plan_new_DTO>()
                .ForMember(dest => dest.ttd_file_path,
                act => act.MapFrom(src => (String.IsNullOrEmpty(src.ttd_file_path) ? "" : keyhttp + src.ttd_file_path.Replace(" ", "%20").Substring(1))))
                                .ReverseMap();

            #region SP PLAN

            CreateMap<SP_SELECT_SP_PLAN_Result, SP_SELECT_SP_PLAN_DTO>()
                .ForMember(dest => dest.sp_type, act => act.MapFrom(src => (src.sp_type ?? "")))
                .ForMember(dest => dest.e_name, act => act.MapFrom(src => (src.e_name ?? "")))
                .ForMember(dest => dest.e_topic, act => act.MapFrom(src => (src.e_topic ?? "")))
                .ForMember(dest => dest.e_place, act => act.MapFrom(src => (src.e_place ?? "")))
                .ForMember(dest => dest.spr_note, act => act.MapFrom(src => (src.spr_note ?? "")))
                //.ForMember(dest => dest.e_dt_start, act => act.MapFrom(src => (src.e_dt_start.HasValue ? src.e_dt_start.Value : "")))
                .ReverseMap();
            CreateMap<t_sp_approval, t_sp_approval_DTO>().ReverseMap();
            CreateMap<v_sp_product, v_sp_product_DTO>().ReverseMap();
            CreateMap<v_doctor_sponsor, v_doctor_sponsor_DTO>().ReverseMap();
            CreateMap<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO, SP_SELECT_DOCTOR_LIST_SPEAKER_Result>().ReverseMap();
            CreateMap<v_auth_sp_DTO, v_auth_sp>().ReverseMap();
            CreateMap<SP_PRODUCT_VISIT_MOBILE_DTO, SP_PRODUCT_VISIT_MOBILE_Result>().ReverseMap();
            CreateMap<m_topic, m_topic_DTO>().ReverseMap();
            CreateMap<SP_SELECT_SP_REALIZATION_Result, SP_SELECT_SP_REALIZATION_DTO>()
                .ForMember(dest => dest.spr_no, act => act.MapFrom(src => (src.spr_no ?? "")))
                .ForMember(dest => dest.sp_type, act => act.MapFrom(src => (src.sp_type ?? "")))
                .ForMember(dest => dest.e_name, act => act.MapFrom(src => (src.e_name ?? "")))
                .ForMember(dest => dest.e_place, act => act.MapFrom(src => (src.e_place ?? "")))
                .ForMember(dest => dest.spr_note, act => act.MapFrom(src => (src.spr_note ?? "")))
                .ForMember(dest => dest.is_sp_date_realization_saved,
                    act => act.MapFrom(src => (src.sp_date_realization_saved.HasValue ? 1 : 0)))
                .ReverseMap();

            #endregion

            #region User

            CreateMap<v_sales_product, v_sales_product_DTO>()
                .ForMember(dest => dest.dr_spec, act => act.MapFrom(src => (src.dr_spec ?? "")))
                .ForMember(dest => dest.dr_sub_spec, act => act.MapFrom(src => (src.dr_sub_spec ?? "")))
                .ForMember(dest => dest.dr_quadrant, act => act.MapFrom(src => (src.dr_quadrant ?? "")))
                .ForMember(dest => dest.dr_monitoring, act => act.MapFrom(src => (src.dr_monitoring ?? "")))
                .ReverseMap();
            CreateMap<t_sales_product_actual, t_sales_product_actual_DTO>().ReverseMap();
            CreateMap<SP_SELECT_USER_ACTUAL_SEARCH_MOBILE_Result, v_sales_product_DTO>().ReverseMap();

            #endregion

            CreateMap<SP_SELECT_FINISHED_VISIT_MOBILE_DTO, SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE_Result>()
                .ForMember(dest => dest.ttd_file_path,
                act => act.MapFrom(src => (String.IsNullOrEmpty(src.ttd_file_path) ? "" : key.Replace("bas_api_mobile", "bas") + src.ttd_file_path.Replace(" ", "%20").Substring(1))))
                                .ReverseMap();

            CreateMap<t_expense_attachment, DataTableGeneralExpenseDetailAttachDTO>()
                .ForMember(dest => dest.gla_file_name, act => act.MapFrom(src => src.gla_file_name.Replace(" ", "_")))
                .ForMember(dest => dest.gla_date_uploaded,
                    act =>
                        act.MapFrom(
                            src =>
                                src.gla_date_uploaded.HasValue ? src.gla_date_uploaded.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
                .ForMember(dest => dest.gla_file_path,
                    act =>
                        act.MapFrom(
                            src =>
                                (String.IsNullOrEmpty(src.gla_file_path)
                                    ? ""
                                    : key.Replace("bas_api_mobile", "bas") +
                                      src.gla_file_path.Replace(" ", "%20").Substring(1))));
                //.ReverseMap();

            CreateMap<t_expense_approval, DataTableGeneralExpenseDetailApproveDTO>()
                .ForMember(dest => dest.ea_date_sign,
                    act =>
                        act.MapFrom(
                            src => src.ea_date_sign.HasValue ? src.ea_date_sign.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
                .ReverseMap();

            CreateMap<SP_SELECT_MASTER_DOCTOR_PIVOT_Result, SP_SELECT_MASTER_DOCTOR_PIVOT_DTO>().ReverseMap();
        }
    }
}