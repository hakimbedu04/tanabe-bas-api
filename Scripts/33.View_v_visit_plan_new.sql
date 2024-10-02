USE [bas]
GO

/****** Object:  View [dbo].[v_visit_plan_new]    Script Date: 7/10/2019 2:38:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[v_visit_plan_new]
AS
-- author : hakim
-- date : 2019-07-10
-- desc : add latitude longitude and signature doctor
	SELECT 
	c.*,
	(
		select CASE WHEN COUNT(b.topic_id) > 0 THEN 1 ELSE 0 END
		from t_visit_product a
		INNER JOIN t_visit_product_topic b on a.vd_id = b.vd_id
		where a.visit_id = c.visit_id
	) as isShownTopic ,
	ISNULL(tgm.latitude,'') as latitude, ISNULL(tgm.longitude,'') as longitude,
	ISNULL(tsm.[sign],0) as is_sign,ISNULL(tsm.file_upload,'') as ttd_file_path
	FROM [dbo].[v_visit_plan] c
	LEFT JOIN t_gps_mobile tgm ON c.visit_id = tgm.visit_id
	LEFT JOIN t_signature_mobile tsm ON c.visit_id = tsm.visit_id



GO


