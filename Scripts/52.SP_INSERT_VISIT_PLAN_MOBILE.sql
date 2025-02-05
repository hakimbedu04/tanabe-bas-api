USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_VISIT_PLAN]    Script Date: 10/11/2019 1:32:47 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_INSERT_VISIT_PLAN_MOBILE]
@visit_id varchar(50),
@rep_id char(5),
@visit_date_plan datetime,
@dr_code int,
@rep_position char(3)
AS
DECLARE @dr_visit_category nchar(10)
SET @dr_visit_category = (SELECT dr_category FROM m_doctor WHERE dr_code = @dr_code);

-- author : hakim
-- date : 2019-10-10
-- desc : hanya menambahkan hardcode ipad pada kolom visit_comment saat insert visit baru

DECLARE @visit_info varchar(100)
select @visit_info = Rtrim(dr_name) FROM m_doctor WHERE dr_code = @dr_code;

--BEGIN TRANSACTION INSERTGL
--SAVE TRANSACTION STARTPOINT

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
		,[visit_plan_verification_status]
		,[visit_plan_verification_by]
		,[visit_plan_verification_date]
		,[visit_real_verification_status]
		,[visit_real_verification_by]
		,[visit_real_verification_date]
		,[visit_date_realization_saved]
		,[visit_date_plan_saved]
		,[visit_comment])
VALUES
        (@visit_id,
		@rep_id, 
		@visit_date_plan, 
		CASE WHEN @dr_code < 100005 THEN 0 ELSE 1 END, 
		NULL, 
		0,
		CASE WHEN @dr_code < 100005 THEN @visit_info ELSE NULL END,
		NULL,
		NULL, 
		@dr_code,
		--[visit_plan_verification_status]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN 1 ELSE 0 END,
		--[visit_plan_verification_by]				
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN @rep_id ELSE null END,
		--[visit_plan_verification_date]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN CONVERT(DATE, GETDATE()) ELSE null END,
		--[visit_real_verification_status]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN (
											CASE WHEN @dr_code < 100005 THEN 1 ELSE 0 END) ELSE 0 END,
		--[visit_real_verification_by]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN (
											CASE WHEN @dr_code < 100005 THEN @rep_id ELSE NULL END) ELSE NULL END,
        --[visit_real_verification_date]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN (
											CASE WHEN @dr_code < 100005 THEN CONVERT(DATE, GETDATE()) ELSE NULL END) ELSE NULL END,
		--[visit_date_realization_saved]
		CASE WHEN RTRIM(@rep_position) in ('AM','KAM','KAE') THEN (
											CASE WHEN @dr_code < 100005 THEN CONVERT(DATE, GETDATE()) ELSE NULL END) ELSE NULL END,
		--[visit_date_plan_saved]
		CONVERT(DATE, GETDATE())
		, 'IPAD');

		IF RTRIM(@rep_position) <> 'AM' AND RTRIM(@rep_position) <> 'PPM' 
		BEGIN
			IF CAST(@dr_code as bigint) > 100005
				BEGIN
					UPDATE m_doctor SET [dr_used_session] = [dr_used_session] + 1, 
					[dr_used_remaining] = [dr_used_remaining] - 1,
					[dr_used_month_session] = DATEPART(MONTH,@visit_date_plan)
					WHERE dr_code = @dr_code;
				END
		END

		IF EXISTS(SELECT 1 FROM v_sales_product WITH(NOLOCK) WHERE rep_id = @rep_id AND sales_year_plan = YEAR(@visit_date_plan) 
				AND sales_date_plan = MONTH(@visit_date_plan) AND dr_code = @dr_code)
			BEGIN
				--IF NOT EXISTS(SELECT 1 FROM t_visit_product  WHERE visit_id=@visit_id)
				--	BEGIN
						INSERT INTO [dbo].[t_visit_product]
								([visit_id]
								,[visit_code]
								,[vd_value]
								,[vd_date_saved]
								,sp_sp
								,sp_percentage)
						SELECT @visit_id,visit_code,1,getdate(),sp_sp,sp_percentage FROM v_sales_product WHERE rep_id = @rep_id AND sales_year_plan = YEAR(@visit_date_plan) 
						AND sales_date_plan = MONTH(@visit_date_plan) AND dr_code = @dr_code AND (visit_code is not null) GROUP BY visit_code,sp_sp,sp_percentage
					--END
			END
