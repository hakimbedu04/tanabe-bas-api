USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SyncTrans]    Script Date: 26/07/2019 10.20.18 ******/
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
	[vd_id] [bigint] NULL,
	[m_vd_id] [int] NULL,
	[topic_id] [int] NULL,
	[vpt_feedback] [int] NULL,
	[vpt_feedback_date] [date] NULL,
	[note_feedback] [varchar](250) NULL,
	[info_feedback_id] [int] NULL
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
-- setelah test di komen kembali
--DECLARE @t_visit_product TABLE(
--	[vd_id] [bigint] NOT NULL,
--	[m_vd_id] [int] NULL,
--	[visit_id] [varchar](50) NULL,
--	[visit_code] [nchar](10) NULL,
--	[vd_value] [int] NULL,
--	[vd_date_saved] [datetime] NULL,
--	[sp_sp] [tinyint] NULL,
--	[sp_percentage] [int] NULL
--);		

--DECLARE @t_visit_product_topic TABLE(
--	[vpt_id] [int] NOT NULL,
--	[m_vpt_id] [int] NULL,
--	[vd_id] [bigint] NULL,
--	[m_vd_id] [int] NULL,
--	[topic_id] [int] NULL,
--	[vpt_feedback] [int] NULL,
--	[vpt_feedback_date] [date] NULL,
--	[note_feedback] [varchar](250) NULL,
--	[info_feedback_id] [int] NULL
--);	

-- =========================================== test case ========================================================
--delete from @t_visit_product
--delete from @temp_t_visit_product

--insert into @t_visit_product
--select * from t_visit_product where month(vd_date_saved) = 6 and year(vd_date_saved) = 2019 and visit_id = 'V19105470';

--insert into @temp_t_visit_product
--select *,0,0 from t_visit_product where month(vd_date_saved) = 6 and year(vd_date_saved) = 2019 and visit_id = 'V19105470'
--UNION ALL
--select *,1 as is_inserted,0 as is_updated from t_visit_product where month(vd_date_saved) = 6 and year(vd_date_saved) = 2019 and visit_id = 'V19105470' and visit_code = 'T7';



--SET @listProduct = 
--  '<Products>
--  <ListProduct>
--    <vd_id>1844618</vd_id>
--    <m_vd_id>1844618</m_vd_id>
--    <visit_id>V19114289</visit_id>
--    <visit_code>B2</visit_code>
--    <vd_value>1</vd_value>
--    <vd_date_saved>2019-06-12 00:00:00</vd_date_saved>
--    <sp_sp />
--    <sp_percentage />
--    <is_inserted>0</is_inserted>
--    <is_updated>0</is_updated>
--  </ListProduct>
--</Products>'
--  select @listProduct
-- ================================================== BEGIN : bagian Product ==================================================
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

DECLARE @visitid varchar(50);
SET @visitid = (SELECT DISTINCT visit_id FROM @temp_t_visit_product GROUP BY visit_id);

DECLARE @countOnline int, @countOffline int, @countInsertOrUpdate int
--SET @countOnline = (SELECT COUNT(*) FROM @t_visit_product where visit_id = @visitid);
--SET @countOffline = (SELECT COUNT(*) FROM @temp_t_visit_product where visit_id = @visitid);

SET @countInsertOrUpdate = (SELECT COUNT(*) FROM @temp_t_visit_product WHERE (is_inserted = 1 OR is_updated = 1));
-- ====================== BEGIN - bagian update di lewat dl===========================
--select @countOnline,@countOffline
--IF @countOnline > @countOffline
--BEGIN
--   -- memastikan data selisih tidak ada perubahan inser or update
--	IF @countInsertOrUpdate = 0
--	BEGIN
--		DECLARE @vdid int;
--		DECLARE DeleteProductCursor CURSOR FOR
--		SELECT vd_id FROM @t_visit_product WHERE vd_id not in (SELECT vd_id FROM @temp_t_visit_product);
		
--		OPEN DeleteProductCursor  
--		FETCH NEXT FROM db_cursor INTO @vdid

--		-- @vdid kebawah nanti di looping
--		SET @vdid = ( SELECT vd_id FROM @t_visit_product WHERE vd_id not in (SELECT vd_id FROM @temp_t_visit_product))

--		-- jalankan delete 
--		-- EXEC SP_DELETE_PRODUCT_VISIT @vdid
--	END
--END

-- ====================== END - bagian update di lewat dl===========================
IF @countInsertOrUpdate > 0
BEGIN
	-- step 1 product (looping)
	-- insert / update
	DECLARE @vd_id int, @m_vd_id int ,@visit_id varchar(50), @visit_code varchar(10) ,@vd_value int,@vd_date_saved datetime ,@sp_sp tinyint ,@sp_percentage int, @is_inserted int, @is_updated int

	DECLARE cursor_product CURSOR FOR 
	SELECT * 
	FROM @temp_t_visit_product
	WHERE is_inserted = 1 OR is_updated = 1

	OPEN cursor_product  
	FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		  IF @is_inserted != 0
		  BEGIN
			INSERT INTO t_visit_product ([visit_id],[visit_code],[vd_value],[vd_date_saved],[sp_sp],[sp_percentage])
			VALUES (@visit_id,@visit_code,@vd_value,@vd_date_saved,@sp_sp,@sp_percentage);
		  END

		  IF (@is_updated = 1 and @is_inserted = 0)
		  BEGIN
			UPDATE t_visit_product SET sp_sp = @sp_sp, sp_percentage = @sp_percentage WHERE vd_id = @vd_id;
		  END
		  FETCH NEXT FROM cursor_product INTO @vd_id, @m_vd_id,@visit_id, @visit_code,@vd_value ,@vd_date_saved  ,@sp_sp  ,@sp_percentage,@is_inserted ,@is_updated   
	END 
	CLOSE cursor_product  
	DEALLOCATE cursor_product 
END
-- ================================================== END : bagian Product ==================================================
-- ================================================== BEGIN : bagian Topic & feedback ==================================================
INSERT INTO @temp_t_visit_product_topic
	SELECT tab.col.value('vpt_id[1]','int'),
				tab.col.value('m_vpt_id[1]','int'),
				tab.col.value('vd_id[1]','bigint'),
				tab.col.value('m_vd_id[1]','int'),
				tab.col.value('topic_id[1]','int'),
				tab.col.value('vpt_feedback[1]','int'),
				tab.col.value('vpt_feedback_date[1]','date'),
				tab.col.value('note_feedback[1]','varchar(250)'),
				tab.col.value('info_feedback_id[1]','int')
	FROM @listTopic.nodes('//Topics/ListTopic') tab(col);

DECLARE @vd_id_topic int
SET @vd_id_topic = (SELECT DISTINCT vd_id FROM @temp_t_visit_product_topic GROUP BY vd_id);
-- step1 : delete semua topic yg vdid ny sesuai param
DELETE FROM t_visit_product_topic where vd_id = @vd_id_topic;

DECLARE @vpt_id int, @m_vpt_id int , @m_vd_id_topic int ,@topic_id int, @vpt_feedback int, @vpt_feedback_date date, @note_feedback VARCHAR(250), @info_feedback_id int
DECLARE cursor_topic CURSOR FOR 
SELECT * 
FROM @temp_t_visit_product_topic

OPEN cursor_topic  
FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id 

WHILE @@FETCH_STATUS = 0  
BEGIN  
	-- step 2 : insert semua yang baru dikirim
	INSERT INTO t_visit_product_topic ([vd_id],[topic_id],[vpt_feedback],[vpt_feedback_date],[note_feedback],[info_feedback_id])
	VALUES (@vd_id_topic,@topic_id,@vpt_feedback,@vpt_feedback_date,@note_feedback,@info_feedback_id);

	FETCH NEXT FROM cursor_topic INTO @vpt_id , @m_vpt_id  ,@vd_id_topic , @m_vd_id_topic  ,@topic_id , @vpt_feedback , @vpt_feedback_date , @note_feedback , @info_feedback_id 
END 
CLOSE cursor_topic  
DEALLOCATE cursor_topic 
-- ================================================== END : bagian Topic & feedback ====================================================
-- ================================================== BEGIN : bagian ttd/sign ==================================================
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

DECLARE @visit_id_sign varchar(50)
SET @visit_id_sign = (SELECT DISTINCT visit_id FROM @temp_t_signature_mobile GROUP BY visit_id);
-- step1 : delete semua topic yg vdid ny sesuai param
DELETE FROM t_signature_mobile where visit_id = @visit_id_sign;

DECLARE @signature_id int, @m_signature_id int, @rep_id varchar(50), @dr_code int, @sign bit, @file_upload varchar(100), @reason varchar(1000)
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
	VALUES (@visit_id_sign ,@rep_id , @dr_code , @sign , @file_upload , @reason ,@created_at , @updated_at);

	FETCH NEXT FROM cursor_sign INTO @signature_id , @m_signature_id , @visit_id_sign ,@rep_id , @dr_code , @sign , @file_upload , @reason ,@created_at , @updated_at
END 
CLOSE cursor_sign  
DEALLOCATE cursor_sign 
-- ================================================== END : bagian ttd/sign ====================================================
SELECT 'success';
