USE [bas_trial]
GO

/****** Object:  View [dbo].[v_info_feedback]    Script Date: 4/24/2019 6:20:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER VIEW [dbo].[v_info_feedback]
AS
	SELECT b.info_feedback_id, b.info_feedback_type, b.info_description, c.topic_id,c.topic_title,c.topic_group_product, c.topic_filepath, c.topic_file_name 
	FROM t_info_feedback_topic_mapping_mobile a
	INNER JOIN  m_info_feedback_mobile b ON a.info_feedback_id = b.info_feedback_id
	INNER JOIN m_topic c ON a.topic_id = c.topic_id

GO


