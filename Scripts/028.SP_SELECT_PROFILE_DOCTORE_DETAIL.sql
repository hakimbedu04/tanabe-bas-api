USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_PROFILE_DOCTORE_DETAIL]    Script Date: 7/3/2019 9:11:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_SELECT_PROFILE_DOCTORE_DETAIL] @dr_code int, @years int
AS
BEGIN
	SET NOCOUNT ON;

DECLARE @visit_plan INT 
DECLARE @visit_real INT 

SELECT * FROM (
	SELECT DISTINCT sales_id, 
		   dr_code, 
		   dr_name, 
		   dr_quadrant, 
		   dr_monitoring, 
		   dr_spec, 
		   dr_sub_spec, 
		   Year(sales_date_plan_saved) AS [year], 
		   Month(sales_date_plan_saved) AS [month], 
		   prd_code, 
		   prd_name, 
		   sales_plan, 
		   sales_realization, 
		   (( COALESCE(sales_realization / NULLIF(sales_plan,0),0)) * 100 ) AS ach_user, 
		   --0 AS ach_user,
		   (SELECT Sum(visit_plan) 
			FROM   v_visit_plan_product 
			WHERE  dr_code = vdf.dr_code 
				   AND Year(visit_date_plan) = Year(vdf.sales_date_plan_saved) 
				   AND Month(visit_date_plan) = Month(vdf.sales_date_plan_saved) 
				   AND visit_code = (SELECT visit_code 
									 FROM   m_product 
									 WHERE  prd_code = vdf.prd_code 
									 GROUP  BY visit_code)) AS visit_plan, 
		   (SELECT Sum(visit_realization) 
			FROM   v_visit_plan_product 
			WHERE  dr_code = vdf.dr_code 
				   AND Year(visit_date_plan) = Year(vdf.sales_date_plan_saved) 
				   AND Month(visit_date_plan) = Month(vdf.sales_date_plan_saved) 
				   AND visit_code = (SELECT visit_code 
									 FROM   m_product 
									 WHERE  prd_code = vdf.prd_code 
									 GROUP  BY visit_code)) AS visit_realization, 
			
		   ( Cast(
		   
		   COALESCE((Cast((SELECT Sum(visit_realization) 
						FROM   v_visit_plan_product 
						WHERE  dr_code = vdf.dr_code 
							   AND Year(visit_date_plan) = 
								   Year(vdf.sales_date_plan_saved) 
							   AND Month(visit_date_plan) = 
								   Month(vdf.sales_date_plan_saved) 
							   AND visit_code = (SELECT visit_code 
												 FROM   m_product 
												 WHERE  prd_code = vdf.prd_code 
												 GROUP  BY visit_code)) AS DECIMAL) )
				  / 
						 NULLIF(((SELECT Sum(visit_plan) 
						  FROM 
						 v_visit_plan_product 
						 WHERE 
						 dr_code = vdf.dr_code 
						 AND Year(visit_date_plan) = Year(vdf.sales_date_plan_saved) 
						 AND Month(visit_date_plan) = 
							 Month(vdf.sales_date_plan_saved) 
						 AND visit_code = (SELECT visit_code 
										   FROM   m_product 
										   WHERE  prd_code = vdf.prd_code 
										   GROUP  BY visit_code)) * 100),0),0) AS DECIMAL(18, 2) )
		   )                                                
		   
		   AS ach_visit,
		   --CAST(0 AS DECIMAL(18, 2)) AS ach_visit, 
		   sp_type, 
		   (SELECT Sum(budget_plan_value) 
			FROM   v_doctor_sponsor vds WITH(nolock) 
			WHERE  vds.sp_status IN ( 1, 12 ) 
				   AND vds.dr_code = @dr_code 
				   AND sp_type = vdf.sp_type 
				   AND ( CONVERT (DATE, sp_date_realization) > 
						 Dateadd(year, -1, Getdate()) 
					   ))                                   AS sp_plan, 
		   Isnull((SELECT Sum(budget_real_value) 
				   FROM   v_doctor_sponsor vds WITH(nolock) 
				   WHERE  vds.sp_status IN ( 1, 12 ) 
						  AND vds.dr_code = @dr_code 
						  AND sp_type = vdf.sp_type 
						  AND ( CONVERT (DATE, sp_date_realization) > 
								Dateadd(year, -1, Getdate()) 
							  )), 0)                        AS sp_real, 

		   COALESCE(ROUND(Cast(Isnull((SELECT Sum(budget_real_value) 
						FROM   v_doctor_sponsor vds WITH(nolock) 
						WHERE  vds.sp_status IN ( 1, 12 ) 
							   AND vds.dr_code = @dr_code 
							   AND sp_type = vdf.sp_type 
							   AND ( CONVERT (DATE, sp_date_realization) > 
									 Dateadd(year, -1, Getdate()) 
								   )), 0) AS DECIMAL(18, 2)) 
								   / 
		   NULLIF((SELECT Sum(budget_plan_value) 
			FROM 
		   v_doctor_sponsor vds WITH(nolock) 
																WHERE 
		   vds.sp_status IN ( 1, 12 ) 
		   AND vds.dr_code = @dr_code 
		   AND sp_type = vdf.sp_type 
		   AND ( CONVERT (DATE, sp_date_realization) > Dateadd(year, -1, Getdate()) 
			   )) * 100,2),0)  , 0)                                   AS ach_sp 
			--CAST(0 AS DECIMAL(18, 2)) AS ach_sp 
	FROM   v_doctor_profile vdf 
	WHERE  dr_code = @dr_code
	AND ( CONVERT (DATE, sales_date_plan_saved) > Dateadd(year, -1, Getdate()) ) ) AS tbl WHERE year = @years
	ORDER  BY year, month 
END
