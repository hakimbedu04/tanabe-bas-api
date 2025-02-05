USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_VISIT_REALIZATION_NO_PLAN]    Script Date: 1/20/2022 6:13:49 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER Procedure [dbo].[SP_INSERT_VISIT_REALIZATION_NO_PLAN]
@visit_id varchar(50),
@rep_id char(5),
@visit_date_plan date,
@dr_code int,
@visit_info text,
@rep_position char(10),
@visit_type int


AS
declare @PointPlan int = 0

IF EXISTS (SELECT * FROM t_visit where rep_id = @rep_id AND convert(date,visit_date_plan) = @visit_date_plan 
			AND (RTRIM(CONVERT(varchar(250),visit_info)) = 'Half Leave'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Half Day Meeting'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Half Day UC'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Half Day AV')
			AND visit_plan = 0 AND visit_realization = 0)
			BEGIN
				SET @PointPlan = 1
			END

IF EXISTS (SELECT * FROM t_visit where rep_id = @rep_id AND convert(date,visit_date_plan) = @visit_date_plan
			AND (RTRIM(CONVERT(varchar(250),visit_info)) = 'Sick Leave'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Full Leave'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Full Day Meeting'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Full Day UC'
			OR  RTRIM(CONVERT(varchar(250),visit_info)) = 'Full Day Training')
			AND visit_plan = 0 AND visit_realization = 0)
			BEGIN
				SET @PointPlan = 1
			END

--IF EXISTS (SELECT * FROM t_additional_visit_transaction WHERE rep_id = @rep_id AND convert(date,visit_date_plan) =  @visit_date_plan AND [status] = 1)
--			BEGIN
--				SET @PointPlan = 1
--			END

IF NOT EXISTS(SELECT * FROM t_visit WITH(NOLOCK) WHERE rep_id = @rep_id AND dr_code = @dr_code AND cast(visit_date_plan as date) = @visit_date_plan)
BEGIN
	INSERT INTO [dbo].[t_visit]
			   ([visit_id]
			   ,[rep_id]
			   ,[visit_date_plan]
			   ,[visit_plan]
			   ,[visit_code]
			   ,[visit_realization]
			   ,[visit_info]
			   ,[visit_sp]
			   ,[visit_sp_value]
			   ,[dr_code]
			   ,[visit_date_realization_saved]
			   ,[visit_plan_verification_status]
			   ,[visit_plan_verification_by]
			   ,[visit_plan_verification_date]
			   ,[visit_real_verification_status]
			   ,[visit_real_verification_by]
			   ,[visit_real_verification_date]
			   ,[visit_date_plan_saved]
			   ,[visit_type]
			   )
	VALUES
			   (@visit_id,
			   @rep_id, 
			   @visit_date_plan, 
			   @PointPlan, 
			   'ADD', 
			   1,
			   @visit_info,
			   NULL,
			   NULL, 
			   @dr_code,
			   NULL,
			   1,
			   @rep_id,
			   GETDATE(),
			   0,		
			   NULL,
			   NULL,
			   GETDATE(),
			   @visit_type)

	declare @quadrant_info char(10)
	declare @max_value int
	declare @current_used_sess int

	SELECT @quadrant_info = rtrim(dr_quadrant),@current_used_sess = dr_used_session FROM m_doctor WITH(NOLOCK) WHERE dr_code = @dr_code;
	SET @max_value = (SELECT [max_value_per_month] FROM m_quadrant WHERE quadrant = @quadrant_info);


	IF @max_value <> @current_used_sess
		BEGIN
			--if RTRIM(@rep_position) <> 'AM' AND RTRIM(@rep_position) <> 'PPM' AND RTRIM(@rep_position) <> 'RM' 
			IF EXISTS(SELECT * FROM m_position WHERE RTRIM(pos_description) = RTRIM(@rep_position) and pos_dr_quota = 1)
				BEGIN
					IF CAST(@dr_code as bigint) > 100005
						BEGIN
							UPDATE m_doctor SET [dr_used_session] = [dr_used_session] + 1, 
												[dr_used_remaining] = [dr_used_remaining] - 1,
												[dr_used_month_session] = DATEPART(MONTH,@visit_date_plan),
												dr_last_updated = getdate() WHERE dr_code = @dr_code;
						END
				END
		END
END



