CREATE PROCEDURE SP_PRODUCT_VISIT_MOBILE
AS
BEGIN
	SELECT     MAX(RTRIM(dbo.m_product.visit_code)) as visit_code, RTRIM(dbo.m_product.visit_group) AS visit_group, dbo.m_gl_cs.o_description, dbo.v_branch.rep_name AS o_name
	FROM         dbo.m_gl_cs INNER JOIN
						  dbo.v_branch ON dbo.m_gl_cs.o_sbo = dbo.v_branch.sbo_id RIGHT OUTER JOIN
						  dbo.m_product ON dbo.m_gl_cs.o_id = dbo.m_product.visit_budget_ownership
	WHERE
				dbo.m_product.prd_status = 1 and visit_code not in ('T0','T00')
	GROUP BY	dbo.m_product.visit_group,dbo.m_gl_cs.o_description,dbo.v_branch.rep_name 
	order by dbo.m_product.visit_group
END