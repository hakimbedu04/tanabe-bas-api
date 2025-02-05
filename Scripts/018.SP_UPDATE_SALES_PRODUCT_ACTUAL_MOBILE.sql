CREATE Procedure [dbo].[SP_UPDATE_SALES_PRODUCT_ACTUAL_MOBILE]
@spa_id int,
@sp_id bigint,
@spa_date date,
@spa_quantity bigint,		--sales quantity
@spa_note varchar(500)
-- =============================================
-- Author:		<Hakim>
-- Create date: <2019-04-26>
-- Description:	<add return success ath end of try line>
-- =============================================
AS
BEGIN TRANSACTION

DECLARE @prd_code char(50)
DECLARE @price float 
DECLARE @target_qty bigint
DECLARE @sales_id varchar(50)

SELECT @sales_id = sales_id, @prd_code = prd_code, @target_qty = sp_target_qty FROM t_sales_product WHERE sp_id = @sp_id;
SELECT @price = price_regular FROM v_product_price WHERE prd_code = @prd_code and price_status = 1;

RETRY:
BEGIN TRY
	UPDATE dbo.t_sales_product_actual SET 
           [spa_date] = @spa_date,
           [spa_quantity] = @spa_quantity,
           [spa_note] = @spa_note WHERE spa_id = @spa_id;
	WAITFOR DELAY '00:00:00.05'
	DECLARE @curr_new_sales_qty int
	SET @curr_new_sales_qty = (SELECT SUM(spa_quantity) FROM dbo.t_sales_product_actual WHERE sp_id = @sp_id)

	DECLARE @sales_value float
	SET @sales_value = @price * @curr_new_sales_qty

	Update dbo.t_sales_product SET sp_sales_qty = @curr_new_sales_qty, sp_sales_value = @sales_value WHERE sp_id = @sp_id;
	WAITFOR DELAY '00:00:00.05'
	Update dbo.t_sales SET sales_realization = 1 WHERE sales_id = @sales_id;
	WAITFOR DELAY '00:00:00.05'
	SELECT 'Success';
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	IF ERROR_NUMBER() = 1205 -- Deadlock Error Number
		BEGIN
			WAITFOR DELAY '00:00:00.05' -- Wait for 5 ms
			GOTO RETRY -- Go to Label RETRY
		END
	ELSE
		BEGIN
			SELECT ERROR_MESSAGE()
		END
END CATCH


