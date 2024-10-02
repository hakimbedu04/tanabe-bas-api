USE [bas]
GO

/****** Object:  View [dbo].[v_visit_plan_new]    Script Date: 7/16/2019 9:20:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER VIEW [dbo].[v_visit_plan_new]
AS
-- author : hakim
-- date : 2019-07-10
-- desc : add latitude longitude and signature doctor

-- author : hakim
-- date : 2019-07-15
-- desc : optimize query, delete isshowntopic flag
	SELECT
		ISNULL(tgm.latitude,'') as latitude, ISNULL(tgm.longitude,'') as longitude,
		ISNULL(tsm.[sign],0) as is_sign,ISNULL(tsm.file_upload,'') as ttd_file_path,
		--(
		--	select CASE WHEN COUNT(b.topic_id) > 0 THEN 1 ELSE 0 END
		--	from t_visit_product a
		--	INNER JOIN t_visit_product_topic b on a.vd_id = b.vd_id
		--	where a.visit_id = t_visit.visit_id
		--) 
		0 as isShownTopic,    
		RTRIM(dbo.t_visit.visit_id) AS visit_id, dbo.t_visit.rep_id, dbo.t_visit.visit_date_plan, dbo.t_visit.visit_plan, dbo.t_visit.visit_realization, dbo.t_visit.dr_code, 
		dbo.m_doctor.dr_name, dbo.m_doctor.dr_spec, dbo.m_doctor.dr_sub_spec, dbo.m_doctor.dr_quadrant, dbo.m_customer_aso.cust_name AS dr_monitoring, 
		dbo.m_doctor.dr_dk_lk, dbo.m_doctor.dr_area_mis, dbo.m_doctor.dr_category, dbo.m_doctor.dr_chanel, dbo.t_visit.visit_date_realization_saved, 
		dbo.t_visit.visit_date_plan_saved, dbo.t_visit.visit_date_plan_updated, dbo.t_visit.visit_info, dbo.t_visit.visit_sp, dbo.t_visit.visit_sp_value, 
		ISNULL(dbo.t_visit.visit_plan_verification_status, 0) AS visit_plan_verification_status, dbo.t_visit.visit_plan_verification_by, dbo.t_visit.visit_plan_verification_date, 
		ISNULL(dbo.t_visit.visit_real_verification_status, 0) AS visit_real_verification_status, dbo.t_visit.visit_real_verification_by, dbo.t_visit.visit_real_verification_date, 
		dbo.m_doctor.dr_address, dbo.t_visit.visit_code
	FROM         dbo.m_customer_aso RIGHT OUTER JOIN
	dbo.m_doctor ON dbo.m_customer_aso.cust_id = dbo.m_doctor.cust_id RIGHT OUTER JOIN
	dbo.t_visit ON dbo.m_doctor.dr_code = dbo.t_visit.dr_code
	LEFT JOIN t_gps_mobile tgm ON t_visit.visit_id = tgm.visit_id
	LEFT JOIN t_signature_mobile tsm ON t_visit.visit_id = tsm.visit_id






GO


