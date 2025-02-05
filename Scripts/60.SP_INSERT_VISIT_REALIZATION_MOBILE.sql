USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_VISIT_REALIZATION_MOBILE]    Script Date: 1/20/2022 11:23:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_INSERT_VISIT_REALIZATION_MOBILE]
@visit_id varchar(50),
@rep_id char(10),
@visit_info text,
@visit_realization int,
@sp_realization_stat int,
@sp_real_amount float,
@e_name varchar(40),
@e_place varchar(250),
@visit_type int

AS

DECLARE @realizationDate date = (SELECT visit_date_plan FROM t_visit WHERE visit_id = @visit_id)
DECLARE @dr_code int 
DECLARE @datePlan date
DECLARE @spds_id int
DECLARE @spr_id varchar(15)
SELECT @dr_code = dr_code, @datePlan = visit_date_plan FROM t_visit WHERE visit_id = @visit_id
DECLARE @sp_id int
DECLARE @spdr_id int
DECLARE @sp_ba char(3)
SELECT @spr_id = spr_id, @sp_id = sp_id, @spds_id = spds_id, @spdr_id = spdr_id, @sp_ba = sp_ba FROM v_doctor_sponsor 
WHERE spr_initiator = @rep_id AND dr_code = @dr_code AND sp_type = 'SP2' AND e_dt_start = @datePlan
DECLARE @rep_position char(10) = (SELECT rep_position FROM m_rep WHERE rep_id = @rep_id)

--cek apakah posisi memerlukan verifikasi 
DECLARE @v_ver_state tinyint = ISNULL((SELECT pos_visit_verification FROM m_position WHERE RTRIM(pos_description) = RTRIM(@rep_position)),0)
-- author : hakim
-- date : 2019-10-11
-- desc : update visit_comment = ipad when realized from ipad at table t_visit 
IF EXISTS(SELECT * from t_visit WHERE visit_id = @visit_id  AND visit_plan_verification_status = 0)
	BEGIN
		RAISERROR('visitplan_not_verified', 16, 1)
		return -1
	END

IF @visit_realization = 1
	BEGIN
		IF NOT EXISTS(SELECT * FROM v_visit_product_topic WHERE visit_id = @visit_id)
			BEGIN
				RAISERROR('null_topic', 16, 1)
				return -1
			END
		ELSE
			BEGIN
				IF EXISTS(SELECT * FROM v_visit_product_topic WHERE visit_id = @visit_id AND vpt_feedback is null)
					BEGIN
						RAISERROR('null_feedback', 16, 1)
						return -1
					END
			END
	END

BEGIN TRY
	SET DEADLOCK_PRIORITY HIGH	
	BEGIN TRANSACTION

	IF @v_ver_state = 0
		BEGIN
			UPDATE t_visit SET visit_realization = @visit_realization, 
								visit_info = @visit_info, 
								visit_real_verification_status = 1,
								visit_real_verification_by = @rep_id,
								visit_real_verification_date = GETDATE(),
								visit_comment = 'IPAD',
								visit_date_realization_saved = getdate() ,
								visit_type = @visit_type
								WHERE visit_id = @visit_id;
			--IF RTRIM(@rep_position) <> 'AM' 
			IF EXISTS(SELECT * FROM m_position WHERE RTRIM(pos_description) = RTRIM(@rep_position) and pos_visit_verification = 0 and pos_dr_quota = 1 )
			BEGIN
				if @dr_code > 100006
					BEGIN
						UPDATE m_doctor SET [dr_used_session] = [dr_used_session] - 1, 
								[dr_used_remaining] = [dr_used_remaining] + 1,
								[dr_used_month_session] = 
								(case when dr_used_session = 1 THEN NULL ELSE [dr_used_month_session] END),
								dr_last_updated = getdate()  WHERE dr_code = @dr_code;
					END
			END
		END
	ELSE
		BEGIN
			UPDATE t_visit SET visit_realization = @visit_realization, 
								visit_info = @visit_info, 
								visit_date_realization_saved = 
								(case when dr_code > 100005 then getdate() else null end) ,
								visit_comment = 'IPAD',
								visit_type = @visit_type
								WHERE visit_id = @visit_id;
		END

	IF @visit_realization = 0
		BEGIN
			DELETE FROM t_visit_product where visit_id = @visit_id;
			INSERT INTO [dbo].[t_visit_product]
				([visit_id]
				,[visit_code]
				,[vd_value]
				,[vd_date_saved])
			VALUES
				(@visit_id, 
				'T0',1, 
				getdate());
		END

		update t_visit 
		set visit_product_count = ISNULL((select count(*)
		FROM t_visit_product where visit_id = t_visit.visit_id),0),
		visit_type = @visit_type
		where visit_id = @visit_id

		--jika sp plan-nya juga di completion(realisasi di visit)
		if @sp_realization_stat <> -1 
			BEGIN
				UPDATE t_sp_sponsor SET  budget_real_value = @sp_real_amount WHERE spds_id = @spds_id 
				UPDATE t_sp_doctor set dr_actual = @sp_realization_stat, dr_date_realization = getdate() WHERE spdr_id = @spdr_id

				if @sp_realization_stat = 1
					BEGIN
						IF NOT EXISTS(SELECT * FROM t_sp_attachment WHERE spr_id = @spr_id)
							BEGIN
								RAISERROR('null_attachment', 16, 1)
							END
						ELSE
							BEGIN
								UPDATE t_spr set e_name = RTRIM(@e_name),e_place = @e_place WHERE spr_id = @spr_id
								UPDATE t_sp set sp_status = 4, sp_date_realization = @realizationDate, sp_date_realization_saved = GetDate() where sp_id=@sp_id
							END
					END
				ELSE
					BEGIN
						UPDATE t_sp set sp_status = 13, sp_date_realization = NULL, sp_date_realization_saved = GetDate() where sp_id=@sp_id
					END

			END

	COMMIT TRANSACTION 
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	RAISERROR('error_query', 16, 1)
END CATCH




