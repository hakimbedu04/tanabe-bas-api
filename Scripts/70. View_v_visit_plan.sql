USE [bas]
GO
/****** Object:  View [dbo].[v_visit_plan]    Script Date: 08/09/2023 16.57.11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[v_visit_plan]
AS
SELECT        RTRIM(dbo.t_visit.visit_id) AS visit_id, dbo.t_visit.rep_id, dbo.t_visit.visit_date_plan, dbo.t_visit.visit_plan, dbo.t_visit.visit_realization, dbo.t_visit.dr_code, dbo.m_doctor.dr_name, dbo.m_doctor.dr_spec, 
                         dbo.m_doctor.dr_sub_spec, dbo.m_doctor.dr_quadrant, dbo.m_customer_aso.cust_name AS dr_monitoring, dbo.m_doctor.dr_dk_lk, dbo.m_doctor.dr_area_mis, dbo.m_doctor.dr_category, dbo.m_doctor.dr_chanel, 
                         dbo.t_visit.visit_date_realization_saved, dbo.t_visit.visit_date_plan_saved, dbo.t_visit.visit_date_plan_updated, dbo.t_visit.visit_info, dbo.t_visit.visit_sp, dbo.t_visit.visit_sp_value, 
                         ISNULL(dbo.t_visit.visit_plan_verification_status, 0) AS visit_plan_verification_status, dbo.t_visit.visit_plan_verification_by, dbo.t_visit.visit_plan_verification_date, ISNULL(dbo.t_visit.visit_real_verification_status, 0) 
                         AS visit_real_verification_status, dbo.t_visit.visit_real_verification_by, dbo.t_visit.visit_real_verification_date, dbo.m_doctor.dr_address, dbo.t_visit.visit_code, dbo.t_gps_mobile.latitude, dbo.t_gps_mobile.longitude, 
                         dbo.t_gps_mobile.address, dbo.t_visit.visit_type, dbo.t_visit.visit_associate_status
FROM            dbo.t_gps_mobile RIGHT OUTER JOIN
                         dbo.t_visit ON dbo.t_gps_mobile.visit_id = dbo.t_visit.visit_id LEFT OUTER JOIN
                         dbo.m_customer_aso RIGHT OUTER JOIN
                         dbo.m_doctor ON dbo.m_customer_aso.cust_id = dbo.m_doctor.cust_id ON dbo.t_visit.dr_code = dbo.m_doctor.dr_code
GO