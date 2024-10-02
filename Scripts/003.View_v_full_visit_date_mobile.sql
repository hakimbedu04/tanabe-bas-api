CREATE VIEW [dbo].[v_full_visit_date_mobile]
AS
	SELECT DATEPART(DAY, visit_date_plan) as daylist,count(DATEPART(DAY, visit_date_plan)) as cnt
	FROM t_visit WITH(NOLOCK) WHERE DATEPART(MONTH, visit_date_plan) =  DATEPART(MONTH, GETDATE())
	AND DATEPART(YEAR, visit_date_plan) =  DATEPART(YEAR, GETDATE()) GROUP BY visit_date_plan;
GO