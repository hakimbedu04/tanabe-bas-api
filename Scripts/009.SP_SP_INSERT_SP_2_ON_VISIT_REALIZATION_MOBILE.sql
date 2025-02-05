USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_SP_2_ON_VISIT_REALIZATION_MOBILE]    Script Date: 4/9/2019 3:25:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_INSERT_SP_2_ON_VISIT_REALIZATION_MOBILE]
@visit_id varchar(50),
@rep_id char(10),
@visit_info text,
@visit_realization int,
@sp_real_amount float,
@budgetAllocation char(3),
@e_name varchar(40),
@e_place varchar(250),
@listAttachment xml

AS
DECLARE @tb_sprNo TABLE (col1 nvarchar(15))
DECLARE @realizationDate date = (SELECT visit_date_plan FROM t_visit WHERE visit_id = @visit_id)
DECLARE @dr_code int 
DECLARE @datePlan date
DECLARE @spds_id int
DECLARE @spr_id varchar(15)
DECLARE @sp_id int
DECLARE @spdr_id int
DECLARE @sp_ba char(3)

DECLARE @sboId int = (SELECT sbo_id FROM m_sbo WHERE sbo_rep = @rep_id AND sbo_status = 1)
DECLARE @nikSH char(5)
DECLARE @NamaBankSH varchar(50)
DECLARE @RekSH varchar(25)
SELECT @nikSH = sbo_shareholders, @NamaBankSH = sh_bank_code, @RekSH = sh_bank_account FROM v_branch WHERE sbo_id = @sboId
DECLARE @hdr_allocation_key nvarchar(40) = Concat(@nikSH,'/',@NamaBankSH,'/',@RekSH)

DECLARE @rep_position char(10) = (SELECT rep_position FROM m_rep WHERE rep_id = @rep_id)

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

BEGIN TRANSACTION INSERREALIZATIONVISIT
SAVE TRANSACTION STARTPOINT

IF RTRIM(@rep_position) = 'AM' OR RTRIM(@rep_position) = 'KAM' OR RTRIM(@rep_position) = 'KAE'
		BEGIN
			UPDATE t_visit SET visit_realization = @visit_realization, 
								visit_info = @visit_info, 
								visit_real_verification_status = 1,
								visit_real_verification_by = @rep_id,
								visit_real_verification_date = CONVERT(DATE,GETDATE()),
								visit_date_realization_saved = getdate() WHERE visit_id = @visit_id;

			IF RTRIM(@rep_position) = 'KAE'
			BEGIN
				if @dr_code > 100005
					BEGIN
						UPDATE m_doctor SET [dr_used_session] = [dr_used_session] - 1, 
								[dr_used_remaining] = [dr_used_remaining] + 1,
								[dr_used_month_session] = 
								(case when dr_used_session = 1 THEN NULL ELSE [dr_used_month_session] END) WHERE dr_code = @dr_code;
					END
			END
		END
	ELSE
		BEGIN
			UPDATE t_visit SET visit_realization = @visit_realization, 
								visit_info = @visit_info, 
								visit_date_realization_saved = getdate() WHERE visit_id = @visit_id;
		END

	DECLARE @dr_name varchar(25)
	-- ----------------------------------Begin : modified @spr_id --------------------------------------
	DECLARE @tbl_sprId table (spr_id char(10))
	-- select dahulu spr__id yang di temp
	DECLARE @spr_id_temp varchar(15)
	SET @spr_id_temp = (SELECT spr_id from t_sp_attachment_temp where visit_id = @visit_id) --  'V19043859'

	-- kemudian di bawah untuk insert ganti dengan update

	INSERT INTO @tbl_sprId EXEC SP_GET_NEW_SPR_ID
	set @spr_id  = (select rtrim(spr_id) FROM @tbl_sprId)
	-- ----------------------------------End : modified @spr_id --------------------------------------

	DECLARE @DateStart date 
	SELECT @dr_code = dr_code, @dr_name = dr_name, @DateStart = visit_date_plan, @rep_id = rep_id FROM v_visit_plan WHERE visit_id = @visit_id
	DECLARE @GeneralSponsor int = 9
	DECLARE @sql TABLE (col1 nvarchar(MAX))
	declare @spr_no nvarchar(25)
		INSERT INTO @tb_sprNo EXEC SP_GET_NEW_SPR_NO
		SET @spr_no = (SELECT * FROM @tb_sprNo)
	declare @spID int
	declare @spdrID int



--Cek apakah SP sudah dibuatkan untuk dokter tersebut atau belum
IF EXISTS(SELECT * FROM v_doctor_sponsor WHERE spr_initiator = @rep_id AND dr_code = @dr_code AND CONVERT(Date,e_dt_start) = @DateStart)
	BEGIN
		RAISERROR('exists_sp', 16, 1)
		return -1
	END

IF @budgetAllocation = 'PMD'
	BEGIN
		DECLARE @tblSPProduct table(vbo int,sp_percentage int)
		DECLARE @sumPercent int

		IF EXISTS(SELECT visit_code,sp_percentage FROM t_visit_product WITH(NOLOCK) WHERE visit_id = @visit_id AND (sp_percentage = 0 OR sp_percentage is null) GROUP BY visit_code,sp_percentage)
					BEGIN
						RAISERROR('product_incomplete', 16, 1)
						return -1
					END

		INSERT INTO @tblSPProduct
		SELECT visit_budget_ownership,sp_percentage FROM v_visit_product WITH(NOLOCK) WHERE visit_id = @visit_id GROUP BY visit_budget_ownership,sp_percentage
		SET @sumPercent = (SELECT SUM(ISNULL(sp_percentage,0)) from @tblSPProduct)
		IF @sumPercent <> 100
			BEGIN
				RAISERROR('product_incomplete', 16, 1)
				return -1
			END
	END

INSERT INTO [dbo].[t_spr]
           (spr_id
		   ,spr_no
           ,spr_initiator
           ,spr_status
           ,spr_date_created
		   ,spr_note
		   ,e_dt_start
           ,e_dt_end
		   ,e_name
		   ,e_topic
		   ,e_place
		   ,spr_allocation_key
		   ,input_origin)
     VALUES
           (@spr_id
		   ,@spr_no
           ,@rep_id
           ,1
           ,GETDATE()
		   ,NULL
           ,@DateStart   
           ,@DateStart
		   ,@e_name
		   ,NULL
		   ,@e_place
		   ,@hdr_allocation_key
		   ,'Visit')
INSERT INTO [dbo].[t_sp]
           (spr_id
           ,sp_type
		   ,sp_ba
		   ,sp_status
		   ,sp_date_realization
		   ,sp_date_realization_saved)
     VALUES
           (@spr_id
           ,'SP2'
		   ,@budgetAllocation
           ,4 --verification
		   ,@DateStart
		   ,GETDATE())
set @spID=@@identity

DECLARE @tblSPAttachment TABLE(spf_id int,spf_file_name varchar(250),spf_file_path varchar(250));				
INSERT INTO @tblSPAttachment
	SELECT tab.col.value('spf_id[1]','INT'),
			tab.col.value('spf_file_name[1]','varchar(250)'),
			tab.col.value('spf_file_path[1]','varchar(250)')
	FROM @listAttachment.nodes('//List') tab(col);

--INSERT INTO [dbo].[t_sp_attachment]
--					(spr_id
--					,spf_file_name
--					,spf_file_path)
--SELECT @spr_id,spf_file_name,spf_file_path FROM @tblSPAttachment
UPDATE [t_sp_attachment]
SET spr_id = @spr_id
WHERE spr_id = @spr_id_temp
		
IF NOT EXISTS(SELECT * FROM t_sp_attachment WHERE spr_id = @spr_id)
	BEGIN
		ROLLBACK TRANSACTION STARTPOINT
		RAISERROR('null_attachment', 16, 1)
		return -1
	END

DECLARE @da_position varchar(50)
DECLARE @da_functionary varchar(50)
DECLARE @da_nik char(5)
DECLARE @da_email varchar(50)
DECLARE @pm_nik char(5)
DECLARE @prod_name varchar(50)

DECLARE @cost_center int
DECLARE @visit_code varchar(15)
DECLARE @PersenAllocation float
DECLARE @bPlan float
DECLARE @bReal float
declare @ba_percentage int
DECLARE @sbo_id int
DECLARE @rep_id_rm char(5)

INSERT INTO [dbo].[t_sp_doctor]
				(sp_id
				,dr_code
				,dr_plan,dr_actual
				,dr_date_realization)
			VALUES
				(@spID
				,@dr_code
				,0,1
				,getdate())
set @spdrID=@@identity

INSERT INTO [dbo].[t_sp_sponsor]
		(spdr_id
		,sponsor_id
		,budget_currency
		,budget_plan_value
		,budget_real_value)
	VALUES
		(@spdrID
		,@GeneralSponsor
		,'Rp'
		,0
		,@sp_real_amount)
	
--INSERT PAX
	EXEC INSERT_PAX @spr_id
--END OF INSERT PAX

IF(RTRIM(@rep_position) = 'MR') OR (RTRIM(@rep_position) = 'PS')
	BEGIN
		SELECT @da_nik = bo_am, @da_functionary = bo_am_name,@da_email = bo_am_email FROM v_branch WHERE rep_id = @rep_id and sbo_status = 1
		SET @da_position = 'AM'

		IF RTRIM(@budgetAllocation) = 'OWN'
			BEGIN
				SET @sbo_id  = (SELECT sbo_id FROM v_branch WHERE rep_id=@da_nik and sbo_status = 1)
				SET @cost_center  = (SELECT o_id FROM m_gl_cs WHERE o_sbo = @sbo_id AND o_status = 1 AND o_description not like 'Head%')

				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,@cost_center FROM v_visit_product WHERE visit_id = @visit_id

				--INSERT AM APPROVER
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@da_nik
					,'APPROVED'
					,GETDATE()
					,2
					,'APPROVER'
					,@da_email
					,1)

				--INSERT BUDGET ALLOCATION BASE ON REGION MR
				SET @bReal = (SELECT SUM(budget_real_value) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID)
				IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
									(sp_id,ba_owner,ba_percentage,ba_plan,ba)
								VALUES
									(@spID,@cost_center,100,@bReal,0)
					END
			END
		ELSE
			BEGIN
				--INSERT AM AUTHORIZER
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@da_nik
					,'AUTHORIZED'
					,GETDATE()
					,1
					,'AUTHORIZER'
					,@da_email
					,1)
				
				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,visit_budget_ownership FROM v_visit_product WHERE visit_id = @visit_id

				--INSERT PM APPROVER 
				DECLARE C1 CURSOR READ_ONLY 
				FOR
					SELECT sp_product,sp_product_ownership FROM t_sp_product WHERE spr_id = @spr_id
				OPEN C1
				FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @prod_name = (SELECT TOP 1 visit_group FROM m_product WHERE visit_code = RTRIM(@visit_code))
					SET @pm_nik = (SELECT TOP 1 [o_functionary] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_position = (SELECT TOP 1 [o_description] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_functionary = (SELECT TOP 1 [o_functionary_name] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_email = (SELECT TOP 1 [o_functionary_email] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))

					IF NOT EXISTS(SELECT 1 FROM t_sp_approval WHERE spr_id = @spr_id AND spa_position = @da_position)
					BEGIN
						INSERT INTO [dbo].[t_sp_approval]
							(spr_id
							,spa_position
							,spa_functionary
							,spa_nik
							,spa_approval
							,spa_date_sign
							,spa_level
							,spa_action
							,spa_email
							,spa_ready)
						VALUES
							(@spr_id
							,@da_position
							,@da_functionary
							,@pm_nik
							,'APPROVED'
							,getdate()
							,2
							,'APPROVER'
							,@da_email
							,1)	
					END
					

					SET @ba_percentage = (SELECT sp_percentage FROM v_visit_product WHERE visit_id = @visit_id AND visit_code = RTRIM(@visit_code))
					SET @PersenAllocation  = cast(@ba_percentage as decimal) / 100
					SET @bReal = (SELECT (SELECT CAST(SUM(budget_real_value) as decimal) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID) * @PersenAllocation )


					IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
							(sp_id,ba_owner,ba_percentage,ba_plan,ba)
						VALUES (@spID,@cost_center,@ba_percentage,@bReal,0)
					END

					FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				END
				CLOSE C1
				DEALLOCATE C1		
			END
	END

IF(RTRIM(@rep_position) = 'PPM')
	BEGIN
		SET @sbo_id = (SELECT sbo_id FROM v_m_doctor WHERE dr_code = @dr_code)
		SET @da_nik = (SELECT bo_am FROM v_branch WHERE sbo_id = @sbo_id and sbo_status = 1) 
		SET @da_position = 'AM'
		SET @da_functionary = (SELECT bo_am_name FROM v_branch WHERE sbo_id = @sbo_id and sbo_status = 1) 
		SET @da_email = (SELECT bo_am_email FROM v_branch WHERE sbo_id = @sbo_id and sbo_status = 1) 

		IF RTRIM(@budgetAllocation) = 'OWN'
			BEGIN
				SET @cost_center  = 4609 --Cost Center PPM
				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,@cost_center FROM v_visit_product WHERE visit_id = @visit_id
				--SELECT @spr_id,visit_code,@cost_center FROM v_sales_product WHERE sales_id = @sales_id GROUP BY visit_code,visit_budget_ownership 

				--INSERT AM APPROVER
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@da_nik
					,'APPROVED'
					,getdate()
					,2
					,'APPROVER'
					,@da_email
					,1)

				--INSERT BUDGET ALLOCATION BASE ON PPM BUDGET
				--SET @rep_reg = (SELECT rep_region FROM v_rep_full WHERE rep_id=@rep_id)
				SET @bReal = (SELECT SUM(budget_real_value) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID)
				IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
									(sp_id,ba_owner,ba_percentage,ba_plan,ba)
								VALUES
									(@spID,@cost_center,100,@bReal,0)
					END
			END
		ELSE
			BEGIN
				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,visit_budget_ownership FROM v_visit_product WHERE visit_id = @visit_id

				--INSERT PM APPROVER 
				DECLARE C1 CURSOR READ_ONLY 
				FOR
					SELECT sp_product,sp_product_ownership FROM t_sp_product WHERE spr_id = @spr_id 
				OPEN C1
				FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @prod_name = (SELECT visit_group FROM m_product WHERE visit_code = RTRIM(@visit_code) GROUP BY visit_group)
					SET @pm_nik = (SELECT [o_functionary] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code) GROUP BY [o_functionary])
					SET @da_position = (SELECT [o_description] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code) GROUP BY [o_description])
					SET @da_functionary = (SELECT [o_functionary_name] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code) GROUP BY [o_functionary_name])
					SET @da_email = (SELECT [o_functionary_email] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code) GROUP BY [o_functionary_email])

					IF NOT EXISTS(SELECT 1 FROM t_sp_approval WHERE spr_id = @spr_id AND spa_position = @da_position)
					BEGIN
						INSERT INTO [dbo].[t_sp_approval]
							(spr_id
							,spa_position
							,spa_functionary
							,spa_nik
							,spa_approval
							,spa_date_sign
							,spa_level
							,spa_action
							,spa_email
							,spa_ready)
						VALUES
							(@spr_id
							,@da_position
							,@da_functionary
							,@pm_nik
							,'APPROVED'
							,getdate()
							,2
							,'APPROVER'
							,@da_email
							,1)	
					END
			
					SET @ba_percentage = (SELECT sp_percentage FROM v_visit_product WHERE visit_id = @visit_id AND visit_code = RTRIM(@visit_code))
					SET @PersenAllocation  = cast(@ba_percentage as decimal) / 100
					SET @bReal = (SELECT (SELECT CAST(SUM(budget_real_value) as decimal) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID) * @PersenAllocation )

					IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
							(sp_id,ba_owner,ba_percentage,ba_plan,ba)
						VALUES (@spID,@cost_center,@ba_percentage,@bReal,0)
					END

					FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				END
				CLOSE C1
				DEALLOCATE C1

				SET @da_nik = (SELECT reg_functionary FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1) --15 Reg Id untuk PMD
				SET @sbo_id = (SELECT sbo_id FROM v_branch WHERE rep_id = @da_nik and sbo_status = 1) 
				SET @da_position = (SELECT o_description FROM m_gl_cs WHERE o_sbo = @sbo_id AND o_status = 1 AND o_description LIKE 'Head%')
				SET @da_functionary = (SELECT reg_functionary_name FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1) 
				SET @da_email = (SELECT reg_functionary_email FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1) 

				--INSERT APPROVER HEAD OF PMD
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@da_nik
					,'APPROVED'
					,getdate()
					,3
					,'APPROVER'
					,@da_email
					,1)
			END
	END

DECLARE @sbo_id_reg int
DECLARE @rep_reg varchar(15)
IF(RTRIM(@rep_position) = 'AM') OR (RTRIM(@rep_position) = 'KAE')
	BEGIN
		SET @rep_id_rm = (SELECT reg_functionary FROM v_branch WHERE rep_id = @rep_id and sbo_status = 1) 
		SET @sbo_id_reg = (SELECT sbo_id FROM v_branch WHERE rep_id=@rep_id_rm and sbo_status = 1)
		SET @da_position = (SELECT o_description FROM m_gl_cs WHERE o_sbo = @sbo_id_reg AND o_description LIKE 'Head%')
		SET @da_functionary = (SELECT Nama FROM v_branch WHERE rep_id = @rep_id and sbo_status = 1) 
		SET @da_email = (SELECT reg_functionary_email FROM v_branch WHERE rep_id = @rep_id and sbo_status = 1)

		IF RTRIM(@budgetAllocation) = 'OWN'
			BEGIN
				SET @sbo_id  = (SELECT sbo_id FROM v_branch WHERE rep_id=@rep_id and sbo_status = 1)
				SET @cost_center  = (SELECT o_id FROM m_gl_cs WHERE o_sbo = @sbo_id AND o_status = 1)

				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,@cost_center FROM v_visit_product WHERE visit_id = @visit_id
				--SELECT @spr_id,visit_code,@cost_center FROM v_sales_product WHERE sales_id = @sales_id GROUP BY visit_code,visit_budget_ownership 

				--INSERT APPROVER REGION MANAGER
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@rep_id_rm
					,'APPROVED'
					,getdate()
					,2
					,'APPROVER'
					,@da_email
					,1)

				--INSERT BUDGET ALLOCATION BASE ON REGION AM
				SET @bReal = (SELECT SUM(budget_real_value) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID)
				IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
									(sp_id,ba_owner,ba_percentage,ba_plan,ba)
								VALUES
									(@spID,@cost_center,100,@bReal,0)
					END
			END
		ELSE
			BEGIN
				--INSERT PM APPROVER 
				INSERT INTO [dbo].[t_sp_product]
								   (spr_id
								   ,sp_product
								   ,sp_product_ownership)
				SELECT @spr_id,visit_code,visit_budget_ownership FROM v_visit_product WHERE visit_id = @visit_id

				DECLARE C1 CURSOR READ_ONLY 
				FOR
					SELECT sp_product,sp_product_ownership FROM t_sp_product WHERE spr_id = @spr_id 
				OPEN C1
				FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @prod_name = (SELECT visit_group FROM m_product WHERE visit_code = RTRIM(@visit_code))
					SET @pm_nik = (SELECT TOP 1 [o_functionary] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_position = (SELECT TOP 1 [o_description] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_functionary = (SELECT TOP 1 [o_functionary_name] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))
					SET @da_email = (SELECT TOP 1 [o_functionary_email] FROM v_b_ownership WHERE visit_code = RTRIM(@visit_code))

					IF NOT EXISTS(SELECT 1 FROM t_sp_approval WHERE spr_id = @spr_id AND spa_position = @da_position)
					BEGIN
						INSERT INTO [dbo].[t_sp_approval]
							(spr_id
							,spa_position
							,spa_functionary
							,spa_nik
							,spa_approval
							,spa_date_sign
							,spa_level
							,spa_action
							,spa_email
							,spa_ready)
						VALUES
							(@spr_id
							,@da_position
							,@da_functionary
							,@pm_nik
							,'APPROVED'
							,getdate()
							,2
							,'APPROVER'
							,@da_email
							,1)	
					END
					SET @ba_percentage = (SELECT sp_percentage FROM v_visit_product WHERE visit_id = @visit_id AND visit_code = RTRIM(@visit_code))
					SET @PersenAllocation  = cast(@ba_percentage as decimal) / 100
					SET @bReal = (SELECT (SELECT CAST(SUM(budget_real_value) as decimal) FROM dbo.v_doctor_sponsor WHERE sp_id = @spID) * @PersenAllocation )

					IF NOT EXISTS(SELECT 1 FROM t_sp_budget_allocation WHERE sp_id = @spID AND ba_owner = @cost_center)
					BEGIN
						INSERT INTO [dbo].t_sp_budget_allocation
							(sp_id,ba_owner,ba_percentage,ba_plan,ba)
						VALUES (@spID,@cost_center,@ba_percentage,@bReal,0)
					END

					FETCH NEXT FROM C1 INTO
					@visit_code, @cost_center
				END
				CLOSE C1
				DEALLOCATE C1

				SET @da_nik = (SELECT reg_functionary FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1)
				SET @sbo_id = (SELECT sbo_id FROM v_branch WHERE rep_id = @da_nik and sbo_status = 1) 
				SET @da_position = (SELECT o_description FROM m_gl_cs WHERE o_sbo = @sbo_id AND o_status = 1 AND o_description LIKE 'Head%')
				SET @da_functionary = (SELECT reg_functionary_name FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1) 
				SET @da_email = (SELECT reg_functionary_email FROM [v_m_regional] WHERE reg_code = 'PMD' and reg_status = 1) 

				--INSERT APPROVER HEAD OF PMD
				INSERT INTO [dbo].[t_sp_approval]
					(spr_id
					,spa_position
					,spa_functionary
					,spa_nik
					,spa_approval
					,spa_date_sign
					,spa_level
					,spa_action
					,spa_email
					,spa_ready)
				VALUES
					(@spr_id
					,@da_position
					,@da_functionary
					,@da_nik
					,'APPROVED'
					,getdate()
					,3
					,'APPROVER'
					,@da_email
					,1)
			END
	END
IF @@error <> 0
	BEGIN
		ROLLBACK TRANSACTION STARTPOINT
	END
ELSE
	BEGIN
		COMMIT TRANSACTION 
	END




