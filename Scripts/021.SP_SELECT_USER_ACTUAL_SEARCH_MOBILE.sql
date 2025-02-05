CREATE Procedure [dbo].[SP_SELECT_USER_ACTUAL_SEARCH_MOBILE]
@rep_id char(10)
,@month int
,@year int
,@searchtext varchar(250)
AS

SELECT * FROM [v_sales_product] 
WHERE rep_id = @rep_id AND 
sales_date_plan = @month AND sales_year_plan = @year AND 
[sales_plan_verification_status] = 1 AND
[sales_realization] = 1 AND
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
	prd_price like '%'+@searchtext+'%' OR
	sp_target_qty like '%'+@searchtext+'%' OR
	sp_target_value like '%'+@searchtext+'%' OR
	sp_sales_qty like '%'+@searchtext+'%' OR
	sp_sales_value like '%'+@searchtext+'%' OR
	prd_name like '%'+@searchtext+'%' 	
)
ORDER BY dr_code ASC