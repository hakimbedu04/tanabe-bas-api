
ALTER Procedure [dbo].[SP_DELETE_SALES_PRODUCT_ACTUAL_MOBILE]
@spa_id varchar(50)

AS

--BEGIN TRANSACTION

DECLARE @sp_id bigint
DECLARE @prd_code char(50)
DECLARE @price float 
DECLARE @target_qty bigint
DECLARE @sales_id varchar(50)
DECLARE @spa_quantity bigint

SELECT @sp_id = sp_id,@spa_quantity = spa_quantity  FROM t_sales_product_actual  WHERE spa_id = @spa_id;
SELECT @sales_id = sales_id, @prd_code = prd_code, @target_qty = sp_target_qty FROM t_sales_product WHERE sp_id = @sp_id;
SELECT @price = price_regular FROM v_product_price WHERE prd_code = @prd_code and price_status = 1;

RETRY:
BEGIN TRY
BEGIN TRANSACTION
	DELETE FROM t_sales_product_actual WHERE spa_id = @spa_id;

	DECLARE @curr_new_sales_qty int
	SET @curr_new_sales_qty = (SELECT SUM(isnull(spa_quantity,0)) FROM dbo.t_sales_product_actual WHERE sp_id = @sp_id)

	DECLARE @sales_value float
	SET @sales_value = @price * isnull(@curr_new_sales_qty,0)

	Update dbo.t_sales_product SET sp_sales_qty = isnull(@curr_new_sales_qty,0), sp_sales_value = @sales_value WHERE sp_id = @sp_id;
	
	COMMIT TRANSACTION 
	SELECT 'Success'
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

RETRY_2:
BEGIN TRY
BEGIN TRANSACTION
	IF isnull(@curr_new_sales_qty,0) = 0 
		BEGIN
			update dbo.t_sales SET sales_realization = 0 WHERE sales_id = @sales_id;
			
			update dbo.t_sales_product SET sp_real = 0 WHERE sp_id = @sp_id;
			
		END
	COMMIT TRANSACTION 
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	IF ERROR_NUMBER() = 1205 -- Deadlock Error Number
		BEGIN
			WAITFOR DELAY '00:00:00.05' -- Wait for 5 ms
			GOTO RETRY_2 -- Go to Label RETRY
		END
	ELSE
		BEGIN
			SELECT ERROR_MESSAGE()
		END
END CATCH


