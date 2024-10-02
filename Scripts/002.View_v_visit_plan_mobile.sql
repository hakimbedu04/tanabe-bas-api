CREATE VIEW [dbo].[v_visit_plan_mobile]
AS
	select *
	,(CASE WHEN EXISTS(select * from t_visit_product WITH(NOLOCK) WHERE visit_id = v_visit_plan.visit_id) then 1 else 0 end) as prd_visit 
	from v_visit_plan WITH(NOLOCK) 
GO
