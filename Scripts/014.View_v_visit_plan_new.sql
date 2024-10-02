USE [bas_trial]
GO

/****** Object:  View [dbo].[v_visit_plan]    Script Date: 4/24/2019 11:37:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_visit_plan_new]
AS

	SELECT  a.*,CASE WHEN c.topic_id IS NULL THEN 0 ELSE 1 END as isShownTopic
	FROM [bas_trial].[dbo].[v_visit_plan] a
	left join t_visit_product b on a.visit_id = b.visit_id
	left join t_visit_product_topic c on b.vd_id = c.vd_id
GO