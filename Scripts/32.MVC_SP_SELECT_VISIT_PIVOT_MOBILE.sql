USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[MVC_SP_SELECT_VISIT_PIVOT]    Script Date: 7/8/2019 2:17:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[MVC_SP_SELECT_VISIT_PIVOT_MOBILE]
@rep_id char(10),
@date_start varchar(250),
@date_end varchar(250)

WITH RECOMPILE
AS
BEGIN
SET NOCOUNT ON;

		IF @date_start <> 'All' AND @date_end <> 'All'
			BEGIN
				select v.rep_id as rep_id,mr.rep_full_name as [rep_name],mr.rep_position as [position],mr.rep_division as [division],mr.rep_bo as [bo],mr.rep_sbo as [sbo],
				mr.rep_am rep_am,mr.nama_am as [am],mr.rep_region as [region],mr.rep_rm rep_rm,mr.nama_rm as [rm],mr.rep_ppm rep_ppm,mr.nama_ppm as [ppm], v.visit_id visit_id, 
				ISNULL(CAST((v.visit_plan / cast(ISNULL(NULLIF((v.visit_product_count),0),1)as decimal)) as decimal(18,4)),0) as [plan],	
				ISNULL(CAST((SELECT (v.visit_realization / cast(NULLIF((v.visit_product_count),0) as decimal))) as decimal(18,4)),0) as [realization],
				--FORMAT(CAST((v.visit_plan / cast(ISNULL(NULLIF((select count(*) from t_visit_product where visit_id = v.visit_id),0),1)as decimal)) as decimal(18,4)),'G','id-ID') as [plan],
				--FORMAT(CAST((SELECT (v.visit_realization / cast(NULLIF((select count(*) from t_visit_product where visit_id = v.visit_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [realization],
				DATENAME(month,v.visit_date_plan) AS [month],DATEPART(DAY,v.visit_date_plan) AS [visit_date],v.dr_code as [dr_code],
				md.dr_name as [dr_name], md.dr_spec as [spec],md.dr_sub_spec as [sub_spec],md.dr_quadrant as [quadrant],
				c.cust_name as [monitoring],
				md.dr_dk_lk as [dklk],md.dr_area_mis as [area_mis],md.dr_category as [category],md.dr_chanel as [channel],
				v.visit_info as [info],v.visit_sp as [sp], v.visit_sp_value as [sp_value],mv.visit_product visit_product, vd.vd_value vd_value,
				 dbo.m_topic.topic_title, dbo.t_visit_product_topic.vpt_feedback, dbo.m_feedback.feedback_desc
		FROM  dbo.t_visit_product_topic LEFT OUTER JOIN
				dbo.m_feedback ON dbo.t_visit_product_topic.vpt_feedback = dbo.m_feedback.feedback_id LEFT OUTER JOIN
                dbo.m_topic ON dbo.t_visit_product_topic.topic_id = dbo.m_topic.topic_id RIGHT OUTER JOIN
                dbo.t_visit_product AS vd WITH(NOLOCK) ON dbo.t_visit_product_topic.vd_id = vd.vd_id RIGHT OUTER JOIN
                dbo.t_visit AS v WITH(NOLOCK) ON vd.visit_id = v.visit_id LEFT OUTER JOIN
                    (SELECT     visit_code, visit_product
                    FROM          dbo.m_product
                    GROUP BY visit_code, visit_product) AS mv ON mv.visit_code = vd.visit_code LEFT OUTER JOIN
                dbo.v_rep_full AS mr ON mr.rep_id = v.rep_id LEFT OUTER JOIN
                dbo.m_doctor AS md ON v.dr_code = md.dr_code LEFT OUTER JOIN
                dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
				WHERE (CONVERT (DATE,v.visit_date_plan) >= @date_start AND CONVERT (DATE,v.visit_date_plan) <= @date_end)
				AND mr.rep_id = @rep_id AND v.visit_plan_verification_status= 1 
			END
		ELSE
			BEGIN
				select v.rep_id as rep_id,mr.rep_full_name as [rep_name],mr.rep_position as [position],mr.rep_division as [division],mr.rep_bo as [bo],mr.rep_sbo as [sbo],
				mr.rep_am rep_am,mr.nama_am as [am],mr.rep_region as [region],mr.rep_rm rep_rm,mr.nama_rm as [rm],mr.rep_ppm rep_ppm,mr.nama_ppm as [ppm], v.visit_id visit_id, 
				FORMAT(CAST((v.visit_plan / cast(ISNULL(NULLIF((v.visit_product_count),0),1)as decimal)) as decimal(18,4)),'G','id-ID') as [plan],	
				FORMAT(CAST((SELECT (v.visit_realization / cast(NULLIF((v.visit_product_count),0) as decimal))) as decimal(18,4)),'G','id-ID') as [realization],
				--FORMAT(CAST((v.visit_plan / cast(ISNULL(NULLIF((select count(*) from t_visit_product where visit_id = v.visit_id),0),1)as decimal)) as decimal(18,4)),'G','id-ID') as [plan],
				--FORMAT(CAST((SELECT (v.visit_realization / cast(NULLIF((select count(*) from t_visit_product where visit_id = v.visit_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [realization],
				DATENAME(month,v.visit_date_plan) AS [month],DATEPART(DAY,v.visit_date_plan) AS [visit_date],v.dr_code as [dr_code],
				md.dr_name as [dr_name], md.dr_spec as [spec],md.dr_sub_spec as [sub_spec],md.dr_quadrant as [quadrant],
				c.cust_name as [monitoring],
				md.dr_dk_lk as [dklk],md.dr_area_mis as [area_mis],md.dr_category as [category],md.dr_chanel as [channel],
				v.visit_info as [info],v.visit_sp as [sp], v.visit_sp_value as [sp_value],mv.visit_product visit_product, vd.vd_value vd_value,
				dbo.t_visit_product_topic.topic_id, dbo.m_topic.topic_title, dbo.t_visit_product_topic.vpt_feedback, dbo.m_feedback.feedback_desc
				FROM  dbo.t_visit_product_topic LEFT OUTER JOIN
                dbo.m_feedback ON dbo.t_visit_product_topic.vpt_feedback = dbo.m_feedback.feedback_id LEFT OUTER JOIN
                dbo.m_topic ON dbo.t_visit_product_topic.topic_id = dbo.m_topic.topic_id RIGHT OUTER JOIN
                dbo.t_visit_product AS vd WITH(NOLOCK) ON dbo.t_visit_product_topic.vd_id = vd.vd_id RIGHT OUTER JOIN
                dbo.t_visit AS v WITH(NOLOCK) ON vd.visit_id = v.visit_id LEFT OUTER JOIN
                    (SELECT     visit_code, visit_product
                    FROM          dbo.m_product
                    GROUP BY visit_code, visit_product) AS mv ON mv.visit_code = vd.visit_code LEFT OUTER JOIN
                dbo.v_rep_full AS mr ON mr.rep_id = v.rep_id LEFT OUTER JOIN
                dbo.m_doctor AS md ON v.dr_code = md.dr_code LEFT OUTER JOIN
                dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
				WHERE MONTH(v.visit_date_plan) = MONTH(getdate()) and YEAR(v.visit_date_plan) = YEAR(getdate())
				AND mr.rep_id = @rep_id AND v.visit_plan_verification_status= 1 
			END
END