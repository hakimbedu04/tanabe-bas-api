USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_topic]    Script Date: 8/23/2019 1:52:03 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER Procedure [dbo].[sp_offline_m_topic]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	--SELECT 
	--[topic_id]
 --     ,[topic_title]
 --     ,RTRIM([topic_group_product]) as [topic_group_product]
 --     ,[topic_filepath]
 --     ,[topic_file_name]
 --     ,[topic_status]
	-- FROM m_topic;
	SELECT 
	[topic_id]
      ,[topic_title]
      ,RTRIM([topic_group_product]) as [topic_group_product]
      ,REPLACE(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'),' ','%20') as topic_filepath
	  --,ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg') as topic_filepath
      ,REPLACE((RIGHT(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'), CHARINDEX('/', REVERSE(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'))) -1)), '%20',' ') as topic_file_name
      ,[topic_status]
	 FROM m_topic;
END;


