USE [bas_trial]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_FINISHED_VISIT]    Script Date: 4/24/2019 9:38:24 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_SELECT_FINISHED_VISIT_SEARCH_MOBILE]
@rep_id char(10)
,@month int
,@year int
,@searchtext varchar(250)
AS

SELECT * FROM [v_visit_plan] 
WHERE rep_id = @rep_id AND 
(DATEPART(month,visit_date_plan) = @month AND DATEPART(year,visit_date_plan) = @year) AND 
[visit_date_realization_saved] IS NOT NULL AND 
[visit_real_verification_status] = 1 AND
	(
		dr_code like '%'+@searchtext+'%' OR
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