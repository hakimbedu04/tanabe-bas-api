USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_info_feedback_mobile]    Script Date: 25/07/2019 23.57.10 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_info_feedback_mobile]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	*
	 FROM m_info_feedback_mobile;
END;


GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_info_feedback_topic_mapping_mobile]    Script Date: 25/07/2019 23.57.10 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_info_feedback_topic_mapping_mobile]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	*
	 FROM t_info_feedback_topic_mapping_mobile;
END;


GO
