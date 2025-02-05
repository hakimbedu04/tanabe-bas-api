USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SyncTrans]    Script Date: 2/18/2020 2:25:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[SyncTrans]

@listProduct xml,
@listTopic xml,
@listSign xml
AS

DECLARE @temp_t_visit_product TABLE(
	[vd_id] [bigint] NOT NULL,
	[m_vd_id] [int] NULL,
	[visit_id] [varchar](50) NULL,
	[visit_code] [nchar](10) NULL,
	[vd_value] [int] NULL,
	[vd_date_saved] [datetime] NULL,
	[sp_sp] [tinyint] NULL,
	[sp_percentage] [int] NULL,
	is_inserted int,
	is_updated int
);	

DECLARE @temp_t_visit_product_topic TABLE(
	[vpt_id] [int] NOT NULL,
	[m_vpt_id] [int] NULL,
	[vd_id] [int] NULL,
	[m_vd_id] [int] NULL,
	[topic_id] [int] NULL,
	[vpt_feedback] [int] NULL,
	[vpt_feedback_date] [date] NULL,
	[note_feedback] [varchar](250) NULL,
	[info_feedback_id] [int] NULL,
	is_inserted int,
	is_updated int
);	

DECLARE @temp_t_signature_mobile TABLE(
	[signature_id] [int] NOT NULL,
	[m_signature_id] [int] NULL,
	[visit_id] [nvarchar](50) NULL,
	[rep_id] [nvarchar](50) NULL,
	[dr_code] [int] NULL,
	[sign] [bit] NULL,
	[file_upload] [varchar](100) NULL,
	[reason] [varchar](1000) NULL,
	[created_at] [date] NULL,
	[updated_at] [date] NULL
);

-- ================================================== BEGIN : bagian Product ==================================================

--BEGIN TRY
--	SET DEADLOCK_PRIORITY HIGH	
--	BEGIN TRANSACTION

INSERT INTO @temp_t_visit_product
SELECT tab.col.value('vd_id[1]','bigint'),
		tab.col.value('m_vd_id[1]','int'),
		tab.col.value('visit_id[1]','varchar(50)'),
		tab.col.value('visit_code[1]','varchar(10)'),
		tab.col.value('vd_value[1]','int'),
		tab.col.value('vd_date_saved[1]','datetime'),
		tab.col.value('sp_sp[1]','tinyint'),
		tab.col.value('sp_percentage[1]','int'),
		tab.col.value('is_inserted[1]','int'),
		tab.col.value('is_updated[1]','int')
FROM @listProduct.nodes('//Products/ListProduct') tab(col);
INSERT INTO @temp_t_visit_product_topic
SELECT tab.col.value('vpt_id[1]','int'),
		tab.col.value('m_vpt_id[1]','int'),
		tab.col.value('vd_id[1]','int'),
		tab.col.value('m_vd_id[1]','int'),
		tab.col.value('topic_id[1]','int'),
		tab.col.value('vpt_feedback[1]','int'),
		tab.col.value('vpt_feedback_date[1]','date'),
		tab.col.value('note_feedback[1]','varchar(250)'),
		tab.col.value('info_feedback_id[1]','int'),
		tab.col.value('is_inserted[1]','int'),
		tab.col.value('is_updated[1]','int')
FROM @listTopic.nodes('//Topics/ListTopic') tab(col);

INSERT INTO @temp_t_signature_mobile
	SELECT tab.col.value('signature_id[1]','int'),
	tab.col.value('m_signature_id[1]','int'),
	tab.col.value('visit_id[1]','varchar(50)'),
	tab.col.value('rep_id[1]','varchar(50)'),
	tab.col.value('dr_code[1]','int'),
	tab.col.value('sign[1]','bit'),
	tab.col.value('file_upload[1]','varchar(100)'),
	tab.col.value('reason[1]','varchar(1000)'),
	tab.col.value('created_at[1]','date'),
	tab.col.value('updated_at[1]','date')
FROM @listSign.nodes('//Sign/ListSign') tab(col);
						
DECLARE @countInsertOrUpdate int
DECLARE @vd_id int
DECLARE @rep_id varchar(5)
DECLARE @visit_id varchar(25)
DECLARE @datePlan date;
DECLARE @dr_code int , @countproductori int, @countproductnew int

DECLARE  @m_vd_id int, @visit_code varchar(10) ,@vd_value int,@vd_date_saved datetime ,@sp_sp tinyint ,@sp_percentage int, @is_inserted int, @is_updated int
DECLARE @m_vpt_id int , @m_vd_id_topic int ,@topic_id int, @vpt_feedback int, @vpt_feedback_date date, @note_feedback VARCHAR(250), @info_feedback_id int --, @is_inserted int, @is_updated int
DECLARE @sp_percentage_other int 
DECLARE @vd_id_topic int
DECLARE @vpt_id int

-- ============================================== BEGIN : PRODUCT  =============================================================================
		-- ====================== BEGIN - bagian delete product===========================
		-- ambil semua data temp yg vd_id != 0 artinya data sama dengan data online (bukan tambahan di offline)
		
		----IF EXISTS(SELECT vd_id FROM @temp_t_visit_product WHERE vd_id != 0)
		----BEGIN
		--SET @visit_id = (SELECT visit_id FROM @temp_t_visit_product GROUP BY visit_id);
		--SET @countproductori = (SELECT COUNT(*) FROM t_visit_product WHERE visit_id = @visit_id) -- => 2)
		--SET @countproductnew = (SELECT COUNT(vd_id) FROM @temp_t_visit_product WHERE vd_id != 0) -- => 0)
		----2 - 0 != 0 maka delete, jika 0 maka skip
		--IF (@countproductori - @countproductnew != 0)
		--BEGIN
		--	DECLARE cursor_delete_product CURSOR FOR 
		--	SELECT vd_id FROM t_visit_product where vd_id not in (SELECT vd_id FROM @temp_t_visit_product WHERE vd_id != 0) where MONTH(vd_date_saved) = MONTH(GETDATE()) AND YEAR(vd_date_saved) = YEAR(vd_date_saved)

		--	OPEN cursor_delete_product  
		--	FETCH NEXT FROM cursor_delete_product INTO @vd_id

		--	WHILE @@FETCH_STATUS = 0  
		--	BEGIN  
		

		--			IF EXISTS(SELECT * FROM v_doctor_sponsor WHERE spr_initiator = @rep_id AND dr_code = @dr_code AND sp_type = 'SP2' AND sp_ba = 'PMD' AND e_dt_start = @datePlan)
		--			BEGIN
		--				select 'Cant add or delete product,because you have made SP Plan related to this product, if you want to add or delete the product, please delete plan SP for first';
		--				return -1
		--			END
		--			ELSE
		--			BEGIN
		--				DELETE t_visit_product WHERE vd_id = @vd_id
 
		--				update t_visit set visit_product_count = (select count(*) FROM t_visit_product where visit_id = t_visit.visit_id)
		--				where visit_id = @visit_id
		--			END

		--			FETCH NEXT FROM cursor_delete_product INTO @vd_id
		--	END 
		--	CLOSE cursor_delete_product  
		--	DEALLOCATE cursor_delete_product 
		--END
-- ======================================================================== END - DELETE PRODUCT ===================================================================
-- ===================================================================== BEGIN - INSERT OR UPDATE PRODUCT ==========================================================
		SET @countInsertOrUpdate = (SELECT COUNT(*) FROM @temp_t_visit_product WHERE (is_inserted = 1 OR is_updated = 1));
		IF @countInsertOrUpdate > 0
		BEGIN
			-- step 1 product (looping)
			-- insert / update
			DECLARE cursor_product CURSOR FOR 
			SELECT * 
			FROM @temp_t_visit_product
			WHERE is_inserted = 1 OR is_updated = 1

			OPEN cursor_product  
			FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated  

			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				--SET @visit_id = (SELECT visit_id FROM t_visit_product WHERE vd_id = @vd_id)
				SELECT @rep_id = rep_id, @dr_code = dr_code FROM t_visit WHERE visit_id = @visit_id
				SET @datePlan = (SELECT visit_date_plan FROM t_visit WHERE visit_id = @visit_id)
				  IF ((@is_inserted = 1 AND @is_updated = 0) OR (@is_inserted = 1 AND @is_updated = 1))
				  BEGIN
					

					IF @sp_sp = 1
					BEGIN
						IF EXISTS(SELECT * FROM v_doctor_sponsor WHERE spr_initiator = @rep_id AND dr_code = @dr_code AND sp_type = 'SP2' AND e_dt_start = @datePlan)
							BEGIN
							select 'Cant add or delete product on visit_id '+@visit_id+',because you have made SP Plan related to this product, if you want to add or delete the product, please delete plan SP for first'
							return -1
						END
					END
					IF EXISTS(SELECT * FROM v_visit_product WHERE visit_id = @visit_id AND visit_code = @visit_code)
					BEGIN
						SET @sp_percentage_other = (SELECT TOP 1 sp_percentage FROM v_visit_product WHERE visit_id = @visit_id AND visit_code = @visit_code)
						IF @sp_percentage <> @sp_percentage_other
							BEGIN
							select 'You have entered the product on visit_id '+@visit_id+', with the same code before but with different percentage'
							return -1
						END
					END

					INSERT INTO t_visit_product ([visit_id],[visit_code],[vd_value],[vd_date_saved],[sp_sp],[sp_percentage])
					VALUES (@visit_id,@visit_code,@vd_value,@vd_date_saved,@sp_sp,@sp_percentage);

					update t_visit set visit_product_count = (select count(*) FROM t_visit_product where visit_id = t_visit.visit_id) where visit_id = @visit_id
				  END
				 
				  
				 -- -- jika data lama di update
				  IF (@is_inserted = 0 AND @is_updated = 1)
				  BEGIN
						
						IF NOT EXISTS(Select * from v_doctor_sponsor where spr_initiator = @rep_id and dr_code = @dr_code and sp_type = 'SP2' and sp_ba = 'PMD')
							BEGIN
							Update t_visit_product set sp_sp = @sp_sp, sp_percentage = @sp_percentage where vd_id = @vd_id;
						END
						
				  END
						
						-- ================================================== BEGIN : bagian Topic & feedback ==================================================
						
						-- ====================== BEGIN - bagian delete topic===========================
						--DECLARE cursor_delete_topic CURSOR FOR 
						--SELECT * FROM t_visit_product_topic where vd_id = @vd_id --vpt_id not in (SELECT vpt_id FROM @temp_t_visit_product_topic WHERE vpt_id != 0)

						--OPEN cursor_delete_topic  
						--FETCH NEXT FROM cursor_delete_topic INTO @vpt_id , @vd_id_topic , @topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id -- ,@is_inserted, @is_updated

						--WHILE @@FETCH_STATUS = 0  
						--BEGIN  
						 
						--	IF NOT EXISTS(SELECT * FROM @temp_t_visit_product_topic WHERE m_vd_id = @vd_id and topic_id = @topic_id)
						--	BEGIN
						--		DELETE FROM t_visit_product_topic WHERE vpt_id = @vpt_id;
						--		--SELECT 'DELETE TOPIC'
						--		FETCH NEXT FROM cursor_delete_topic INTO @vpt_id  ,@vd_id_topic ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id
						--	END
						
						--	FETCH NEXT FROM cursor_delete_topic INTO @vpt_id  ,@vd_id_topic ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id
						--END 
						--CLOSE cursor_delete_topic  
						--DEALLOCATE cursor_delete_topic 
						-- ====================== END - bagian delete topic===========================
						SET @countInsertOrUpdate = (SELECT COUNT(*) FROM @temp_t_visit_product_topic WHERE (is_inserted = 1 OR is_updated = 1));
						IF @countInsertOrUpdate > 0
						BEGIN
							-- step 1 product (looping)
							-- insert / update
							DECLARE cursor_topic CURSOR FOR 
							SELECT * 
							FROM @temp_t_visit_product_topic 
							WHERE is_inserted = 1 OR is_updated = 1

							OPEN cursor_topic  
							FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id ,@is_inserted, @is_updated

							WHILE @@FETCH_STATUS = 0  
							BEGIN  
								SET @vd_id_topic = (SELECT vd_id FROM t_visit_product WHERE visit_id = @visit_id and visit_code = @visit_code )
								--and [vd_value] = @vd_value AND [sp_sp] = @sp_sp and [sp_percentage] = @sp_percentage);
								--jika hanya di insert saja
								IF @m_vd_id = @m_vd_id_topic
								BEGIN
									IF (@is_inserted = 1 AND @is_updated = 0)
									BEGIN
									
										INSERT INTO t_visit_product_topic (vd_id,topic_id)
										VALUES (@vd_id_topic,@topic_id);
									
									--select 'insert 1'
									END
										-- jika sudah insert data baru , lalu di update
									IF (@is_inserted = 1 AND @is_updated = 1)
									BEGIN
										INSERT INTO t_visit_product_topic (vd_id,topic_id,vpt_feedback , vpt_feedback_date , note_feedback ,info_feedback_id )
										VALUES (@vd_id_topic, @topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id );
									--select 'insert 2'
									END
				  
									-- bagian update feedback bukan topic, topic tidak ada update hanya pakai tabel yang sama
									-- jika data lama di update
									IF (@is_inserted = 0 AND @is_updated = 1)
									BEGIN
										UPDATE t_visit_product_topic 
										SET vpt_feedback = @vpt_feedback, 
										vpt_feedback_date = @vpt_feedback_date, 
										note_feedback = @note_feedback, 
										info_feedback_id = @info_feedback_id 
										WHERE vpt_id = @vpt_id;
									--select 'update topic'
									END
								END
								FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id ,@is_inserted ,@is_updated   
							END 
						CLOSE cursor_topic  
						DEALLOCATE cursor_topic 
						END
						---- ================================================== END : bagian Topic & feedback ====================================================
				  FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated   
			END 
			CLOSE cursor_product  
			DEALLOCATE cursor_product 
		END
		ELSE IF @countInsertOrUpdate = 0
		BEGIN
			-- step 1 product (looping)
			-- insert / update
			--DECLARE  @m_vd_id int, @visit_code varchar(10) ,@vd_value int,@vd_date_saved datetime ,@sp_sp tinyint ,@sp_percentage int, @is_inserted int, @is_updated int
			
			DECLARE cursor_product CURSOR FOR 
			SELECT * 
			FROM @temp_t_visit_product
			--WHERE is_inserted = 0 and is_updated = 0

			OPEN cursor_product  
			FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated  

			WHILE @@FETCH_STATUS = 0  
			BEGIN 
					-- ================================================== BEGIN : bagian Topic & feedback ==================================================
						
					-- ====================== BEGIN - bagian delete topic===========================
					--DECLARE cursor_delete_topic CURSOR FOR 
					--SELECT * FROM t_visit_product_topic where vd_id = @vd_id --vpt_id not in (SELECT vpt_id FROM @temp_t_visit_product_topic WHERE vpt_id != 0)

					--OPEN cursor_delete_topic  
					--FETCH NEXT FROM cursor_delete_topic INTO @vpt_id , @vd_id_topic , @topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id -- ,@is_inserted, @is_updated

					--WHILE @@FETCH_STATUS = 0  
					--BEGIN  
						 
					--	IF NOT EXISTS(SELECT * FROM @temp_t_visit_product_topic WHERE m_vd_id = @vd_id and topic_id = @topic_id)
					--	BEGIN
					--		DELETE FROM t_visit_product_topic WHERE vpt_id = @vpt_id;
					--		--SELECT 'DELETE TOPIC'
					--		FETCH NEXT FROM cursor_delete_topic INTO @vpt_id  ,@vd_id_topic ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id
					--	END
						
					--	FETCH NEXT FROM cursor_delete_topic INTO @vpt_id  ,@vd_id_topic ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id
					--END 
					--CLOSE cursor_delete_topic  
					--DEALLOCATE cursor_delete_topic 
					-- ====================== END - bagian delete topic===========================
					SET @countInsertOrUpdate = (SELECT COUNT(*) FROM @temp_t_visit_product_topic WHERE (is_inserted = 1 OR is_updated = 1));
					IF @countInsertOrUpdate > 0
					BEGIN
						-- step 1 product (looping)
						-- insert / update
						DECLARE cursor_topic CURSOR FOR 
						SELECT * 
						FROM @temp_t_visit_product_topic 
						WHERE is_inserted = 1 OR is_updated = 1

						OPEN cursor_topic  
						FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id ,@is_inserted, @is_updated

						WHILE @@FETCH_STATUS = 0  
						BEGIN  
							SET @vd_id_topic = (SELECT vd_id FROM t_visit_product WHERE visit_id = @visit_id and visit_code = @visit_code )
							--and [vd_value] = @vd_value AND [sp_sp] = @sp_sp and [sp_percentage] = @sp_percentage);
							--jika hanya di insert saja
							IF @m_vd_id = @m_vd_id_topic
							BEGIN
								IF (@is_inserted = 1 AND @is_updated = 0)
								BEGIN
									
									INSERT INTO t_visit_product_topic (vd_id,topic_id)
									VALUES (@vd_id_topic,@topic_id);
									
								--select 'insert 1'
								END
									-- jika sudah insert data baru , lalu di update
								IF (@is_inserted = 1 AND @is_updated = 1)
								BEGIN
									INSERT INTO t_visit_product_topic (vd_id,topic_id,vpt_feedback , vpt_feedback_date , note_feedback ,info_feedback_id )
									VALUES (@vd_id_topic, @topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id );
								--select 'insert 2'
								END
				  
								-- bagian update feedback bukan topic, topic tidak ada update hanya pakai tabel yang sama
								-- jika data lama di update
								IF (@is_inserted = 0 AND @is_updated = 1)
								BEGIN
									UPDATE t_visit_product_topic 
									SET vpt_feedback = @vpt_feedback, 
									vpt_feedback_date = @vpt_feedback_date, 
									note_feedback = @note_feedback, 
									info_feedback_id = @info_feedback_id 
									WHERE vpt_id = @vpt_id;
								--select 'update topic'
								END
							END
							FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id ,@is_inserted ,@is_updated   
						END 
					CLOSE cursor_topic  
					DEALLOCATE cursor_topic 
					END
					---- ================================================== END : bagian Topic & feedback ====================================================
				FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated   
			END 
			CLOSE cursor_product  
			DEALLOCATE cursor_product
		END
-- ============================================== BEGIN : PRODUCT  =============================================================================
		-- ================================================== BEGIN : bagian ttd/sign ==================================================
		

		DECLARE @visit_id_sign varchar(50)
		SET @visit_id_sign = (SELECT DISTINCT visit_id FROM @temp_t_signature_mobile GROUP BY visit_id);
		-- step1 : delete semua topic yg vdid ny sesuai param
		IF EXISTS(SELECT DISTINCT visit_id FROM @temp_t_signature_mobile GROUP BY visit_id)
		BEGIN
			DELETE FROM t_signature_mobile where visit_id = @visit_id_sign;
		END


		DECLARE @signature_id int, @m_signature_id int, @sign bit, @file_upload varchar(100), @reason varchar(1000)
		,@created_at date, @updated_at date;
		DECLARE cursor_sign CURSOR FOR 
		SELECT * 
		FROM @temp_t_signature_mobile

		OPEN cursor_sign  
		FETCH NEXT FROM cursor_sign INTO @signature_id , @m_signature_id , @visit_id_sign ,@rep_id , @dr_code , @sign , @file_upload , @reason ,@created_at , @updated_at

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			-- step 2 : insert semua yang baru dikirim
			INSERT INTO t_signature_mobile ([visit_id],[rep_id],[dr_code],[sign],[file_upload],[reason],[created_at],[updated_at])
			VALUES (@visit_id_sign ,@rep_id , @dr_code , @sign , '~/Asset/Images/Sign/'+@file_upload , 'Offline : '+@reason ,@created_at , @updated_at);

			FETCH NEXT FROM cursor_sign INTO @signature_id , @m_signature_id , @visit_id_sign ,@rep_id , @dr_code , @sign , @file_upload , @reason ,@created_at , @updated_at
		END 
		CLOSE cursor_sign  
		DEALLOCATE cursor_sign 
		-- ================================================== END : bagian ttd/sign ====================================================
		SELECT 'success';

--	COMMIT TRANSACTION 
--END TRY
--BEGIN CATCH
--	ROLLBACK TRANSACTION
--	RAISERROR('error', 16, 1)
--END CATCH
