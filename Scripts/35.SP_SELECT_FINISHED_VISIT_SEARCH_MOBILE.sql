USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE]    Script Date: 7/10/2019 3:25:27 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER Procedure [dbo].[SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE]
@rep_id char(10)
,@month int
,@year int
,@searchtext varchar(250)
AS
-- author : hakim
-- date : 2019-07-10
-- desc : add latitude longitude and signature doctor
SELECT c.*,ISNULL(tgm.latitude,'') as latitude, ISNULL(tgm.longitude,'') as longitude,
	ISNULL(tsm.[sign],0) as is_sign,ISNULL(tsm.file_upload,'') as ttd_file_path  FROM [v_visit_plan] c
	LEFT JOIN t_gps_mobile tgm ON c.visit_id = tgm.visit_id
	LEFT JOIN t_signature_mobile tsm ON c.visit_id = tsm.visit_id
	WHERE c.rep_id = @rep_id AND 
	(DATEPART(month,visit_date_plan) = @month AND DATEPART(year,visit_date_plan) = @year) AND 
	[visit_date_realization_saved] IS NOT NULL AND 
	[visit_real_verification_status] = 1 AND
		(
			c.dr_code like '%'+@searchtext+'%' OR
			dr_name like '%'+@searchtext+'%' OR
			dr_spec like '%'+@searchtext+'%' OR
			dr_sub_spec like '%'+@searchtext+'%' OR
			dr_quadrant like '%'+@searchtext+'%' OR
			dr_monitoring like '%'+@searchtext+'%' OR
			dr_dk_lk like '%'+@searchtext+'%' OR
			dr_area_mis  like '%'+@searchtext+'%' OR
			dr_category like '%'+@searchtext+'%' OR
			dr_chanel like '%'+@searchtext+'%' OR
			visit_info like '%'+@searchtext+'%'
		)
	ORDER BY visit_date_plan,visit_id ASC