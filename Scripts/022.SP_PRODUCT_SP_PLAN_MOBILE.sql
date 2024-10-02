CREATE Procedure SP_PRODUCT_SP_PLAN_MOBILE
AS
BEGIN
	select max(visit_code) as visit_code, visit_group, o_description, o_name 
	from v_product_visit 
	where visit_status = 1 and visit_code not in ('T0','T00')
	group by visit_group,o_description,o_name order by visit_group
END