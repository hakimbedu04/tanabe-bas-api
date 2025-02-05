USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_REJECT_ASSOCIATE_VISIT]    Script Date: 08/09/2023 16.24.23 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_REJECT_ASSOCIATE_VISIT]
@associate_id int

AS

DECLARE @visit_id_initiator varchar(15),@visit_id_invited varchar(15), @rep_id_initiator char(5), @rep_id_invited char(5), @rep_position_initiator char(5), @rep_position_invited char(5),
@dr_code_initiator int, @dr_code_invited int, @visit_code char(3)

SELECT
@visit_id_initiator = visit_id_initiator,
@rep_id_initiator = rep_id_initiator,
@dr_code_initiator = dr_code_initiator,
@visit_id_invited = visit_id_invited,
@rep_id_invited = rep_id_invited,
@dr_code_invited = dr_code_invited,
@visit_code = visit_code
from v_visit_associate WHERE associate_id = @associate_id;

SET @rep_position_initiator = (SELECT rep_position FROM m_rep WHERE rep_id = @rep_id_initiator)
SET @rep_position_invited = (SELECT rep_position FROM m_rep WHERE rep_id = @rep_id_invited)

BEGIN TRANSACTION CONFIRMASSOCIATE
SAVE TRANSACTION STARTPOINT

	-- verified status visit initiator
	UPDATE t_visit SET visit_realization = 0, 
						visit_real_verification_status = 1,
						visit_real_verification_by = @rep_id_invited,
						visit_real_verification_date = GETDATE(),
						visit_associate_status = 0,
						visit_date_realization_saved = getdate() WHERE visit_id = @visit_id_initiator;

	--cek apakah posisi memerlukan management quota 
	IF EXISTS(SELECT * FROM m_position WHERE RTRIM(pos_description) = RTRIM(@rep_position_initiator) and pos_visit_verification = 0 and pos_dr_quota = 1 )
	--IF RTRIM(@rep_position) <> 'AM' 
	BEGIN
		if @dr_code_initiator > 100006
			BEGIN
				UPDATE m_doctor SET [dr_used_session] = [dr_used_session] - 1, 
						[dr_used_remaining] = [dr_used_remaining] + 1,
						[dr_used_month_session] = (case when dr_used_session = 1 THEN NULL ELSE [dr_used_month_session] END),
						dr_last_updated = getdate()  WHERE dr_code = @dr_code_initiator;
			END
	END
		

	DELETE FROM t_visit_product where visit_id = @visit_id_initiator;
	INSERT INTO [dbo].[t_visit_product]
		([visit_id]
		,[visit_code]
		,[vd_value]
		,[vd_date_saved])
	VALUES
		(@visit_id_initiator, 
		'T0',1, 
		getdate());

	update t_visit set visit_product_count = ISNULL((select count(*) FROM t_visit_product where visit_id = t_visit.visit_id),0) where visit_id = @visit_id_initiator;

	IF @visit_code = 'ADD'
		BEGIN
			-- hapus status visit invited additional
			DELETE FROM t_visit WHERE visit_id = @visit_id_invited;
		END
	ELSE 
		BEGIN
			-- kembalikan status visit invited
			UPDATE t_visit SET visit_realization = 0, visit_associate_status = 0,
								visit_info = null, 
								visit_type = null,
								visit_date_realization_saved = null WHERE visit_id = @visit_id_invited;
		END

	DELETE FROM  t_visit_associate where associate_id = @associate_id;


IF @@error <> 0
	BEGIN
		ROLLBACK TRANSACTION STARTPOINT
		RAISERROR('query_error', 16, 1)
	END
ELSE
	BEGIN
		COMMIT TRANSACTION 
	END
GO
