USE [bas]
GO

/****** Object:  View [dbo].[v_visit_product_topic]    Script Date: 10/31/2019 10:44:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- author : hakim
-- date : 2019-10-31
-- desc : Handling null value
ALTER VIEW [dbo].[v_visit_product_topic]
AS
--SELECT     dbo.t_visit_product_topic.vpt_id, dbo.t_visit_product_topic.vd_id, dbo.t_visit_product_topic.topic_id, dbo.m_topic.topic_title, dbo.m_topic.topic_group_product, 
--                      dbo.m_topic.topic_filepath, dbo.m_topic.topic_file_name, dbo.t_visit_product_topic.vpt_feedback, dbo.t_visit_product_topic.vpt_feedback_date, 
--                      dbo.t_visit_product.visit_id
--FROM         dbo.t_visit_product_topic INNER JOIN
--                      dbo.t_visit_product ON dbo.t_visit_product_topic.vd_id = dbo.t_visit_product.vd_id LEFT OUTER JOIN
--                      dbo.m_topic ON dbo.t_visit_product_topic.topic_id = dbo.m_topic.topic_id


SELECT     dbo.t_visit_product_topic.vpt_id, dbo.t_visit_product_topic.vd_id, dbo.t_visit_product_topic.topic_id, 
						ISNULL(dbo.m_topic.topic_title,'') as topic_title, 
						ISNULL(dbo.m_topic.topic_group_product,'') as topic_group_product, 
                      --dbo.m_topic.topic_filepath,
					   --dbo.m_topic.topic_file_name,
					   REPLACE(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'),' ','%20') as topic_filepath,
					   REPLACE((RIGHT(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'), CHARINDEX('/', REVERSE(ISNULL([topic_filepath],'~/Files/SP-Topics/no_available_image.jpg'))) -1)), '%20',' ') as topic_file_name,
					    dbo.t_visit_product_topic.vpt_feedback, dbo.t_visit_product_topic.vpt_feedback_date, 
                      dbo.t_visit_product.visit_id
FROM         dbo.t_visit_product_topic INNER JOIN
                      dbo.t_visit_product ON dbo.t_visit_product_topic.vd_id = dbo.t_visit_product.vd_id LEFT OUTER JOIN
                      dbo.m_topic ON dbo.t_visit_product_topic.topic_id = dbo.m_topic.topic_id
GO


