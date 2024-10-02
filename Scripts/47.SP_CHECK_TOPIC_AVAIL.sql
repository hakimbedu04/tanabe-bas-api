USE [bas]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_CHECK_TOPIC_AVAIL]
@visit_id varchar(25)

AS
BEGIN
	SELECT CASE WHEN COUNT(b.topic_id) > 0 THEN 1 ELSE 0 END as Result
	FROM t_visit_product a
	INNER JOIN t_visit_product_topic b on a.vd_id = b.vd_id
	WHERE a.visit_id = @visit_id;
END;

