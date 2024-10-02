USE [bas]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [SP_SELECT_IS_ALREADY_PLANNED_DOCTOR_IN_CURR_WEEK_MOBILE]
@rep_id varchar(50),
@visit_date_plan datetime,
@dr_code INT

AS
DECLARE @week_plan INT
SET @week_plan = (SELECT DATEPART(WEEK, @visit_date_plan) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@visit_date_plan), 0))+ 1)

IF @dr_code > 100005
	BEGIN
		SELECT COUNT(*) FROM v_visit_plan WITH(NOLOCK) WHERE DATEPART(year,visit_date_plan) = DATEPART(year,@visit_date_plan) AND 
		 DATEPART(Month,visit_date_plan) = DATEPART(Month,@visit_date_plan) AND
		 DATEPART(WEEK, visit_date_plan) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,visit_date_plan), 0))+ 1 = @week_plan
		 AND rep_id = @rep_id AND dr_code = @dr_code
	END
ELSE
BEGIN
	SELECT 0;
END
