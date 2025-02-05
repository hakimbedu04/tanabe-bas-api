USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_SALES_PIVOT]    Script Date: 18/07/2019 14.15.15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_SELECT_SALES_PIVOT_MOBILE]
@dateStart  DATETIME,
@dateEnd DATETIME,
@position varchar(50),
@rep_id	varchar(50)

WITH RECOMPILE
AS
SET NOCOUNT ON;
			IF @position = 'MR'
			BEGIN
				IF @dateStart <> NULL AND @dateEnd <> NULL
					BEGIN
						SELECT  tsp.sp_id, tsp.sales_id, tsp.prd_code, mp.prd_aso_desc prd_name, mpp.price_regular prd_price, mpp.price_bpjs prd_price_bpjs,
							  mp.visit_category AS prd_category, tsp.sp_target_qty, tsp.sp_target_value, tsp.sp_sales_qty, 
							  tsp.sp_sales_value, tsp.sp_note, ts.rep_id, ts.sales_date_plan, ts.sales_year_plan, 
							  CAST((SELECT (ts.sales_plan / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)) as [sales_plan],
							  CAST((SELECT (ts.sales_realization / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)) as [sales_realization],
							  --FORMAT(CAST((SELECT (ts.sales_plan / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_plan],
							  --FORMAT(CAST((SELECT (ts.sales_realization / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_realization],
							  ts.sales_info, ts.dr_code, ts.sales_plan_verification_status, 
							  ts.sales_plan_verification_by, ts.sales_plan_verification_date, ts.sales_real_verification_status, 
							  ts.sales_real_verification_by, ts.sales_real_verification_date, ts.sales_date_plan_saved, ts.sales_date_plan_updated, 
							  ts.sales_date_realization_saved, md.dr_name, md.dr_quadrant, c.cust_name as dr_monitoring, md.dr_spec, 
							  md.dr_sub_spec, md.dr_area_mis, md.dr_chanel, md.dr_category, md.dr_sub_category, md.dr_dk_lk, 
							  tsp.sp_plan, tsp.sp_real, vrf.rep_name, vrf.nama_am, vrf.rep_region, vrf.rep_bo, 
							  vrf.rep_sbo, vrf.rep_position, vrf.rep_division, vrf.nama_rm, vrf.rep_ppm, vrf.nama_ppm,
							  (CASE WHEN tsp.sp_target_qty > 10 THEN 'A' WHEN tsp.sp_target_qty >= 6 AND 
							  tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_target_qty >= 2 AND 
							  tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_target_qty = 1 THEN 'D' END) AS target_class_user, 
							  (CASE WHEN tsp.sp_sales_qty > 10 THEN 'A' WHEN tsp.sp_sales_qty >= 6 AND 
							  tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_sales_qty >= 2 AND 
							  tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_sales_qty = 1 THEN 'D' END) AS real_class_user
						FROM  dbo.m_product_aso_price mpp RIGHT OUTER JOIN
							  dbo.m_product AS mp ON mpp.price_id = mp.price_id RIGHT OUTER JOIN
							  dbo.t_sales_product AS tsp WITH(NOLOCK) INNER JOIN
							  dbo.t_sales AS ts WITH(NOLOCK) ON tsp.sales_id = ts.sales_id INNER JOIN
							  dbo.m_doctor AS md ON ts.dr_code = md.dr_code ON mp.prd_code = tsp.prd_code RIGHT OUTER JOIN
							  dbo.v_rep_full_2 AS vrf ON ts.rep_id = vrf.rep_id LEFT OUTER JOIN
							  dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
						WHERE	ts.rep_id = @rep_id AND (CONVERT (DATE,ts.sales_date_plan_saved) >= @dateStart AND CONVERT (DATE,ts.sales_date_plan_saved) <= @dateEnd)
						AND ts.sales_plan_verification_status = 1
					END
				ELSE
					BEGIN
						SELECT  tsp.sp_id, tsp.sales_id, tsp.prd_code, mp.prd_aso_desc prd_name, mpp.price_regular prd_price, mpp.price_bpjs prd_price_bpjs,
							  mp.visit_category AS prd_category, tsp.sp_target_qty, tsp.sp_target_value, tsp.sp_sales_qty, 
							  tsp.sp_sales_value, tsp.sp_note, ts.rep_id, ts.sales_date_plan, ts.sales_year_plan, 
							  FORMAT(CAST((SELECT (ts.sales_plan / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_plan],
							  FORMAT(CAST((SELECT (ts.sales_realization / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_realization],
							  --FORMAT(CAST((SELECT (ts.sales_plan / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_plan],
							  --FORMAT(CAST((SELECT (ts.sales_realization / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_realization],
							  ts.sales_info, ts.dr_code, ts.sales_plan_verification_status, 
							  ts.sales_plan_verification_by, ts.sales_plan_verification_date, ts.sales_real_verification_status, 
							  ts.sales_real_verification_by, ts.sales_real_verification_date, ts.sales_date_plan_saved, ts.sales_date_plan_updated, 
							  ts.sales_date_realization_saved, md.dr_name, md.dr_quadrant, c.cust_name as dr_monitoring, md.dr_spec, 
							  md.dr_sub_spec, md.dr_area_mis, md.dr_chanel, md.dr_category, md.dr_sub_category, md.dr_dk_lk, 
							  tsp.sp_plan, tsp.sp_real, vrf.rep_name, vrf.nama_am, vrf.rep_region, vrf.rep_bo, 
							  vrf.rep_sbo, vrf.rep_position, vrf.rep_division, vrf.nama_rm, vrf.rep_ppm, vrf.nama_ppm,
							  (CASE WHEN tsp.sp_target_qty > 10 THEN 'A' WHEN tsp.sp_target_qty >= 6 AND 
							  tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_target_qty >= 2 AND 
							  tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_target_qty = 1 THEN 'D' END) AS target_class_user, 
							  (CASE WHEN tsp.sp_sales_qty > 10 THEN 'A' WHEN tsp.sp_sales_qty >= 6 AND 
							  tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_sales_qty >= 2 AND 
							  tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_sales_qty = 1 THEN 'D' END) AS real_class_user
						FROM  dbo.m_product_aso_price mpp RIGHT OUTER JOIN
							  dbo.m_product AS mp ON mpp.price_id = mp.price_id RIGHT OUTER JOIN
							  dbo.t_sales_product AS tsp WITH(NOLOCK) INNER JOIN
							  dbo.t_sales AS ts WITH(NOLOCK) ON tsp.sales_id = ts.sales_id INNER JOIN
							  dbo.m_doctor AS md ON ts.dr_code = md.dr_code ON mp.prd_code = tsp.prd_code RIGHT OUTER JOIN
							  dbo.v_rep_full_2 AS vrf ON ts.rep_id = vrf.rep_id LEFT OUTER JOIN
							  dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
						WHERE	ts.rep_id = @rep_id AND ts.sales_plan_verification_status = 1 
						AND sales_date_plan = MONTH(getdate()) AND sales_year_plan = YEAR(getdate())
					END
			END
		ELSE
			BEGIN
				SELECT  tsp.sp_id, tsp.sales_id, tsp.prd_code, mp.prd_aso_desc prd_name, mpp.price_regular prd_price, mpp.price_bpjs prd_price_bpjs,
                      mp.visit_category AS prd_category, tsp.sp_target_qty, tsp.sp_target_value, tsp.sp_sales_qty, 
                      tsp.sp_sales_value, tsp.sp_note, ts.rep_id, ts.sales_date_plan, ts.sales_year_plan, 
					  FORMAT(CAST((SELECT (ts.sales_plan / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_plan],
					  FORMAT(CAST((SELECT (ts.sales_realization / cast(NULLIF((ts.sales_product_count),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_realization],
					  --FORMAT(CAST((SELECT (ts.sales_plan / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_plan],
					  --FORMAT(CAST((SELECT (ts.sales_realization / cast(NULLIF((select count(*) from t_sales_product where sales_id = ts.sales_id),0) as decimal))) as decimal(18,4)),'G','id-ID') as [sales_realization],
					  ts.sales_info, ts.dr_code, ts.sales_plan_verification_status, 
                      ts.sales_plan_verification_by, ts.sales_plan_verification_date, ts.sales_real_verification_status, 
                      ts.sales_real_verification_by, ts.sales_real_verification_date, ts.sales_date_plan_saved, ts.sales_date_plan_updated, 
                      ts.sales_date_realization_saved, md.dr_name, md.dr_quadrant, c.cust_name as dr_monitoring, md.dr_spec, 
                      md.dr_sub_spec, md.dr_area_mis, md.dr_chanel, md.dr_category, md.dr_sub_category, md.dr_dk_lk, 
                      tsp.sp_plan, tsp.sp_real, vrf.rep_name, vrf.nama_am, vrf.rep_region, vrf.rep_bo, 
                      vrf.rep_sbo, vrf.rep_position, vrf.rep_division, vrf.nama_rm, vrf.rep_ppm, vrf.nama_ppm,
                      (CASE WHEN tsp.sp_target_qty > 10 THEN 'A' WHEN tsp.sp_target_qty >= 6 AND 
                      tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_target_qty >= 2 AND 
                      tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_target_qty = 1 THEN 'D' END) AS target_class_user, 
                      (CASE WHEN tsp.sp_sales_qty > 10 THEN 'A' WHEN tsp.sp_sales_qty >= 6 AND 
                      tsp.sp_target_qty <= 10 THEN 'B' WHEN tsp.sp_sales_qty >= 2 AND 
                      tsp.sp_target_qty <= 5 THEN 'C' WHEN tsp.sp_sales_qty = 1 THEN 'D' END) AS real_class_user
				FROM  dbo.t_sales_product AS tsp WITH(NOLOCK) RIGHT OUTER JOIN
                      dbo.m_doctor AS md RIGHT OUTER JOIN
                      dbo.t_sales AS ts WITH(NOLOCK) ON md.dr_code = ts.dr_code LEFT OUTER JOIN
                      dbo.v_rep_full_2 AS vrf ON ts.rep_id = vrf.rep_id ON tsp.sales_id = ts.sales_id LEFT OUTER JOIN
                      dbo.m_product_aso_price AS mpp RIGHT OUTER JOIN
                      dbo.m_product AS mp ON mpp.price_id = mp.price_id ON tsp.prd_code = mp.prd_code LEFT OUTER JOIN
                      dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
				--FROM  dbo.m_product_aso_price mpp RIGHT OUTER JOIN
    --                  dbo.m_product AS mp ON mpp.price_id = mp.price_id RIGHT OUTER JOIN
    --                  dbo.t_sales_product AS tsp WITH (UPDLOCK) INNER JOIN
    --                  dbo.t_sales AS ts WITH (UPDLOCK) ON tsp.sales_id = ts.sales_id INNER JOIN
    --                  dbo.m_doctor AS md ON ts.dr_code = md.dr_code ON mp.prd_code = tsp.prd_code RIGHT OUTER JOIN
    --                  dbo.v_rep_full AS vrf ON ts.rep_id = vrf.rep_id LEFT OUTER JOIN
    --                  dbo.m_customer_aso AS c ON c.cust_id = md.cust_id
				WHERE	(CONVERT (DATE,ts.sales_date_plan_saved) >= @dateStart AND CONVERT (DATE,ts.sales_date_plan_saved) <= @dateEnd)
				AND ts.sales_plan_verification_status = 1
			END

	


--SELECT  * FROM @tb_coverage



