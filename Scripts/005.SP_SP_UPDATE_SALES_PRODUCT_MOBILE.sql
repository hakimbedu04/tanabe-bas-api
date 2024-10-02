USE [bas_trial]
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_SALES_PRODUCT]    Script Date: 4/4/2019 1:50:53 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_UPDATE_SALES_PRODUCT_MOBILE]
@sp_id int,
@sp_target_qty  int,
@sp_sp int,
@sp_percentage int

AS

--DECLARE @sp_id int,
--@sp_target_qty  int,
--@sp_sp int,
--@sp_percentage int

--SET @sp_id = 1028203;
--SET @sp_target_qty = 95;
--SET @sp_sp = 1;
--SET @sp_percentage = 100;

DECLARE @rep_id char(5)
DECLARE @sales_id varchar(25) = (SELECT sales_id FROM t_sales_product WHERE sp_id = @sp_id)
DECLARE @dr_code int 
SELECT  @rep_id = rep_id, @dr_code = dr_code FROM t_sales WHERE sales_id = @sales_id
DECLARE @visit_id varchar(15) 
DECLARE @prd_code varchar(15) 
select @visit_id = visit_id, @prd_code = prd_code FROM t_sales_product WHERE sp_id=@sp_id

DECLARE @datePlan date = (SELECT visit_date_plan FROM t_visit WHERE visit_id = @visit_id)
DECLARE @price float 
DECLARE @sp_target_value float
SELECT @price = price_regular FROM v_product_price WHERE prd_code = @prd_code and price_status = 1;
SET @sp_target_value = @price * @sp_target_qty

UPDATE [dbo].[t_sales_product]
SET [sp_target_qty] = @sp_target_qty
	,[sp_target_value] = @sp_target_value
	,[sp_sp] = @sp_sp
	,[sp_percentage] = @sp_percentage
WHERE sp_id = @sp_id



