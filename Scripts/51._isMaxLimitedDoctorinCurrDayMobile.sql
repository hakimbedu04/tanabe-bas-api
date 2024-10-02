USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[_isMaxLimitedDoctorinCurrDay]    Script Date: 10/3/2019 9:30:56 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[_isMaxLimitedDoctorinCurrDayMobile]
@rep_id char(5),
@visit_date_plan DATE
-- author : hakim
-- date of change : 03 oct 2019
-- description : modify data type of parameter @visit_date_plan vrom varchar to date
AS
declare @rep_position varchar(4) = (select rep_position from m_rep where rep_id = @rep_id and rep_status = 1)
declare @max_visit int = (select pos_max_visit from m_position where pos_description = RTRIM(@rep_position))
declare @count_visit int = (SELECT count(*) as count_doc FROM t_visit WITH(NOLOCK) WHERE CONVERT(date,visit_date_plan) = @visit_date_plan and rep_id = @rep_id)

IF @count_visit >= @max_visit
	BEGIN
		SELECT 1 as result
	END
ELSE
	BEGIN
		SELECT 0 as result
	END