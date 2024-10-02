
ALTER VIEW [dbo].[v_visit_plan_new]
AS
	SELECT *,
	(
		select CASE WHEN COUNT(b.topic_id) > 0 THEN 1 ELSE 0 END
		from t_visit_product a
		INNER JOIN t_visit_product_topic b on a.vd_id = b.vd_id
		where a.visit_id = c.visit_id
	) as isShownTopic 
	FROM [dbo].[v_visit_plan] c


