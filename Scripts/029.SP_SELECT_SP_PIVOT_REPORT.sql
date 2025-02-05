USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_SP_PIVOT_REPORT]    Script Date: 7/3/2019 11:23:59 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER Procedure [dbo].[SP_SELECT_SP_PIVOT_REPORT]
	@dateStart  varchar(50),
	@dateEnd varchar(50),
	@position varchar(10),
	@nik char(5)

AS

IF @dateStart is not null and @dateEnd is not null 
	BEGIN
		IF @position = 'ADM' Or @position = 'GM' Or @position = 'NSM' Or @position = 'PMD' Or 
		@position = 'PM' Or @position = 'SA' Or @position = 'BSD'
			BEGIN
				IF @dateStart = "ALL" AND @dateStart = "All"
					BEGIN
						SET @dateStart = NULL
						SET @dateStart = NULL
					END				
				SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
						ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description, 
						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
						FORMAT(CAST((spd.dr_plan / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  
					
						FORMAT(CAST((spd.dr_actual / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

						FORMAT(CAST((sps.budget_plan_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

						FORMAT(CAST((sps.budget_real_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
					  
						dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
						dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
						dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
						dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
				FROM    dbo.m_customer_aso RIGHT OUTER JOIN
						dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
						dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						dbo.m_sponsor AS ms RIGHT OUTER JOIN
						(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
						dbo.m_gl_cs AS cs RIGHT OUTER JOIN
						dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
						dbo.m_rep INNER JOIN
						dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
						dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
						dbo.t_sp_doctor AS spd LEFT OUTER JOIN
						dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code
						INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id
					WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
				    AND spr.spr_status = 1 
			END
		ELSE
			IF @position = 'PKAD'
			BEGIN
				IF @dateStart = "ALL" AND @dateStart = "All"
					BEGIN
						SET @dateStart = NULL
						SET @dateStart = NULL
					END
					SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
					ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description,  
					FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
							CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
							ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

					FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
							CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
							ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
					FORMAT(CAST((spd.dr_plan / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  
					
					FORMAT(CAST((spd.dr_actual / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

					FORMAT(CAST((sps.budget_plan_value / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

					FORMAT(CAST((sps.budget_real_value / 
					CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
					ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
					  
					dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
					dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
					dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
					dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
			FROM    dbo.m_customer_aso RIGHT OUTER JOIN
					dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
					dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
					dbo.m_sponsor AS ms RIGHT OUTER JOIN
					(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
					dbo.m_gl_cs AS cs RIGHT OUTER JOIN
					dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
					dbo.m_rep INNER JOIN
					dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
					dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
					dbo.t_sp_doctor AS spd LEFT OUTER JOIN
					dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code
					INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id					
			WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
					AND spr.spr_initiator = @nik AND spr.spr_status  = 1
				    --AND sp.sp_status = 5
			END
		ELSE
			IF @position = 'RM'
			BEGIN
				IF @dateStart = "ALL" AND @dateStart = "All"
					BEGIN
						SET @dateStart = NULL
						SET @dateStart = NULL
					END
					SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
						ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description,
						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
						FORMAT(CAST((spd.dr_plan / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  
					
						FORMAT(CAST((spd.dr_actual / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

						FORMAT(CAST((sps.budget_plan_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

						FORMAT(CAST((sps.budget_real_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
					  
						dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
						dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
						dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
						dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
				FROM    dbo.m_customer_aso RIGHT OUTER JOIN
						dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
						dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						dbo.m_sponsor AS ms RIGHT OUTER JOIN
						(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
						dbo.m_gl_cs AS cs RIGHT OUTER JOIN
						dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
						dbo.m_rep INNER JOIN
						dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
						dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
						dbo.t_sp_doctor AS spd LEFT OUTER JOIN
						dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code
						INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id					
					WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
					AND v_branch.reg_functionary = @nik AND spr.spr_status  = 1
				    --AND sp.sp_status = 5
			END
		ELSE
			IF @position = 'AM'
			BEGIN
				IF @dateStart = "ALL" AND @dateStart = "All"
					BEGIN
						SET @dateStart = NULL
						SET @dateStart = NULL
					END
					SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
						ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description,
						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
						FORMAT(CAST((spd.dr_plan / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  
					
						FORMAT(CAST((spd.dr_actual / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

						FORMAT(CAST((sps.budget_plan_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

						FORMAT(CAST((sps.budget_real_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
					  
						dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
						dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
						dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
						dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
				FROM    dbo.m_customer_aso RIGHT OUTER JOIN
						dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
						dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						dbo.m_sponsor AS ms RIGHT OUTER JOIN
						(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
						dbo.m_gl_cs AS cs RIGHT OUTER JOIN
						dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
						dbo.m_rep INNER JOIN
						dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
						dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
						dbo.t_sp_doctor AS spd LEFT OUTER JOIN
						dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code
					INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id
					WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
					AND v_branch.bo_am = @nik AND spr.spr_status  = 1
				    --AND sp.sp_status = 5
			END
		ELSE
			IF @position = 'PPM'
			BEGIN
				IF @dateStart = "ALL" AND @dateStart = "All"
					BEGIN
						SET @dateStart = NULL
						SET @dateStart = NULL
					END
					SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
						ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description,
						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

						FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
						FORMAT(CAST((spd.dr_plan / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  
					
						FORMAT(CAST((spd.dr_actual / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

						FORMAT(CAST((sps.budget_plan_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

						FORMAT(CAST((sps.budget_real_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
					  
						dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
						dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
						dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
						dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
				FROM    dbo.m_customer_aso RIGHT OUTER JOIN
						dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
						dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						dbo.m_sponsor AS ms RIGHT OUTER JOIN
						(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
						dbo.m_gl_cs AS cs RIGHT OUTER JOIN
						dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
						dbo.m_rep INNER JOIN
						dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
						dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
						dbo.t_sp_doctor AS spd LEFT OUTER JOIN
						dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code
						INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id
					WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
					AND v_branch.sbo_ppm = @nik AND spr.spr_status  = 1
				    --AND sp.sp_status = 5
			END
		ELSE
			BEGIN
					SELECT  sp.sp_id, spr.spr_id, spr.spr_no, sp.sp_type, spr.e_name, spr.e_topic, spr.e_place, spr.e_dt_start, spr.e_dt_end, sp.sp_date_realization date_realization, mv.visit_product AS product, 
						ms.sponsor_description AS sponsor,md.dr_code, md.dr_name, spp.sp_product_ownership, cs.o_description,
						--FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
						--		CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
						--		ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_plan,

						CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)) AS count_sp_plan,

						--FORMAT(CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
						--		CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
						--		ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_sp_real,
                    
						CAST((SELECT(SELECT(SELECT COUNT(*) FROM t_sp WHERE spr_id = spr.spr_id AND sp_date_realization is not null) / 
								CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1
								ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal)) / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_doctor WHERE sp_id = sp.sp_id) END AS decimal))AS decimal(18, 4)) AS count_sp_real,

						--FORMAT(CAST((spd.dr_plan / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_plan,  

						CAST((spd.dr_plan / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)) AS count_dr_plan,
					
						--FORMAT(CAST((spd.dr_actual / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS count_dr_real,  

						CAST((spd.dr_actual / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)) AS count_dr_real,

						--FORMAT(CAST((sps.budget_plan_value / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_plan_value,  

						CAST((sps.budget_plan_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)) AS budget_plan_value,  

						--FORMAT(CAST((sps.budget_real_value / 
						--CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						--ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)),'G', 'id-ID') AS budget_real_value,  
						CAST((sps.budget_real_value / 
						CAST(CASE WHEN (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) = 0 THEN 1 
						ELSE (SELECT COUNT(*) FROM t_sp_product WHERE spr_id = spr.spr_id) END AS decimal))AS decimal(18, 4)) AS budget_real_value,
					  
						dbo.m_rep.rep_name AS initiator_name, dbo.v_branch.rep_name, 
						dbo.v_branch.sbo_code AS rep_sbo, dbo.v_branch.bo_code AS rep_bo, dbo.v_branch.reg_code AS rep_region, dbo.v_branch.bo_am_name AS nama_am, 
						dbo.v_branch.Nama AS nama_rm, dbo.v_branch.ppm_name AS nama_ppm, md.dr_spec, md.dr_sub_spec, md.dr_quadrant, 
						dbo.m_customer_aso.cust_name AS dr_monitoring, st.ms_code AS spr_status,stt.ms_code As sp_status
				FROM    dbo.m_customer_aso RIGHT OUTER JOIN
						dbo.m_doctor AS md ON dbo.m_customer_aso.cust_id = md.cust_id LEFT OUTER JOIN
						dbo.v_branch ON md.dr_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						dbo.m_sponsor AS ms RIGHT OUTER JOIN
						(SELECT visit_code, visit_product, visit_group FROM dbo.m_product GROUP BY visit_code, visit_product, visit_group) AS mv RIGHT OUTER JOIN
						dbo.m_gl_cs AS cs RIGHT OUTER JOIN
						dbo.t_sp_product AS spp ON cs.o_id = spp.sp_product_ownership RIGHT OUTER JOIN
						dbo.m_rep INNER JOIN
						dbo.t_spr AS spr WITH (NOLOCK) ON dbo.m_rep.rep_id = spr.spr_initiator LEFT OUTER JOIN
						dbo.t_sp AS sp ON spr.spr_id = sp.spr_id ON spp.spr_id = spr.spr_id ON mv.visit_code = spp.sp_product LEFT OUTER JOIN
						dbo.t_sp_doctor AS spd LEFT OUTER JOIN
						dbo.t_sp_sponsor AS sps ON spd.spdr_id = sps.spdr_id ON sp.sp_id = spd.sp_id ON ms.sponsor_id = sps.sponsor_id ON md.dr_code = spd.dr_code 
						INNER JOIN dbo.m_status AS st ON spr.spr_status = st.ms_id INNER JOIN m_status stt ON sp.sp_status = stt.ms_id

					WHERE (CONVERT (DATE,spr.e_dt_start) >= @dateStart AND CONVERT (DATE,spr.e_dt_start) <= @dateEnd)
				    AND spr.spr_initiator = @nik AND spr.spr_status  = 1
			END
	END
