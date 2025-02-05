USE [bas_trial]
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_REALIZATION_VISIT_ACTUAL]    Script Date: 5/9/2019 1:43:31 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_UPDATE_REALIZATION_VISIT_ACTUAL_MOBILE]
@visit_id varchar(50),
@dr_code INT,
@visit_plan INT,
@visit_realization INT,
@visit_info text,
@visit_sp nvarchar(50),
@visit_sp_value float

AS
BEGIN TRANSACTION INSERTGL
SAVE TRANSACTION STARTPOINT

DECLARE @CURR_ACTUAL_VISIT INT
SELECT @CURR_ACTUAL_VISIT = visit_realization FROM t_visit WHERE visit_id = @visit_id;

IF @CURR_ACTUAL_VISIT > @visit_realization --jika dari visit ke not visited
	BEGIN
		UPDATE t_visit SET dr_code = @dr_code, visit_plan = @visit_plan, visit_realization = @visit_realization, 
		visit_info = @visit_info, visit_sp = @visit_sp, visit_sp_value = @visit_sp_value WHERE visit_id = @visit_id;

		DELETE FROM t_visit_product WHERE visit_id = @visit_id;

		INSERT INTO [t_visit_product]
           ([visit_id]
           ,[visit_code]
           ,[vd_value]
		   ,[vd_date_saved])
		VALUES
           (@visit_id, 
		   'T0',1, 
		   GETDATE());
		   
		SELECT 'Success'
	END
ELSE
	IF @CURR_ACTUAL_VISIT < @visit_realization --jika dari not visited ke visit
		BEGIN
			IF EXISTS(SELECT * FROM t_visit_product WHERE visit_id = @visit_id AND visit_code <> 'T0')
				BEGIN
					UPDATE t_visit SET dr_code = @dr_code, visit_plan = @visit_plan, visit_realization = @visit_realization, 
					visit_info = @visit_info, visit_sp = @visit_sp, visit_sp_value = @visit_sp_value WHERE visit_id = @visit_id;
				END

			--DELETE FROM t_visit_product WHERE visit_id = @visit_id;
			SELECT 'Success'
		END
ELSE
	BEGIN
		UPDATE t_visit SET dr_code = @dr_code, visit_plan = @visit_plan, visit_realization = @visit_realization, 
			visit_info = @visit_info, visit_sp = @visit_sp, visit_sp_value = @visit_sp_value WHERE visit_id = @visit_id;
		SELECT 'Success'
	END

IF @@error <> 0
	BEGIN
		ROLLBACK TRANSACTION STARTPOINT
		SELECT ERROR_MESSAGE()
	END
ELSE
	BEGIN
		COMMIT TRANSACTION 
	END