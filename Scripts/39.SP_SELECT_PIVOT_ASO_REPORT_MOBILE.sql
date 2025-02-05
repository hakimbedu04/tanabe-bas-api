USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[SP_SELECT_PIVOT_ASO_REPORT_MOBILE]    Script Date: 18/07/2019 15.32.26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[SP_SELECT_PIVOT_ASO_REPORT_MOBILE]
	@dateStart  varchar(50),
	@dateEnd varchar(50),
	@position char(25),
	@nik char(5)

AS
	IF @dateStart = NULL and @dateEnd = NULL
	BEGIN
		SET @dateStart = (SELECT DATEADD(month, DATEDIFF(month, 0, getdate()), 0));
		SET @dateEnd = (SELECT EOMONTH(getdate()));
	END

	DECLARE @sqlstr as nvarchar(max)
	SET @sqlstr = 'SELECT Month_Name, YEAR , full_date, Branch_Sales_Desc, Channel_Outlet_Group, Channel_Outlet,
		Customer_Id,Customer_Name, City, Territory_Geo_Name, Sales_Category_Name, 
			   Product_Code, Product_Desc, Invoice_No, Hna, Sales_Quantity, Sales_Gross, 
			   Discount_Rp, persen_Disc, DATA, SALES_GROSS_TAN, DISCOUNT_RP_TAN, 
			   CATEGORY_PRODUCT_TAN, SBO_TAN,SALES_VALUE, INC_VALUE, 
			   PLAN_QTY, PLAN_VALUE, PATIENT_MONTH, PATIENT_QUARTER, 
			   PATIENT_YEAR,CATEGORY_OUTLET, GP, PRD, NIK_MR, MR, NIK_AM, AM, NIK_RM, NIK_PPM, PPM,
			   RM, BO_TAN, REGION,UNIT_PRICE, OSE, KAE, KAM, 
			   KAM_CATEGORY, CLASS_OUTLET,AREA_OSE, GROUP_BY_FIN, TABLET, DOSSAGE, 
			   FOCUS, THEURAPTICS, PM, t_aso_id, lot_number, expired_date FROM (
		SELECT [Month Name] AS Month_Name, YEAR , [Full Date] as full_date , [Branch Sales Desc] AS Branch_Sales_Desc, 
			   [Channel Outlet Group] AS Channel_Outlet_Group, [Channel Outlet] AS Channel_Outlet,
			   [Customer Id] AS Customer_Id, [Customer Name] AS Customer_Name,  
			   City, [Territory Geo Name] AS Territory_Geo_Name, [Sales Category Name] AS Sales_Category_Name, 
			   [Product Code] AS Product_Code, [Product Desc] AS Product_Desc, [Invoice No] AS Invoice_No, 
			   Hna, [Sales Quantity] AS Sales_Quantity, [Sales Gross] AS Sales_Gross, 
			   [Discount Rp] AS Discount_Rp, [% Disc] AS persen_Disc, 
			   DATA, [SALES GROSS TAN] AS SALES_GROSS_TAN, [DISCOUNT RP TAN] AS DISCOUNT_RP_TAN, 
			   [CATEGORY PRODUCT TAN] AS CATEGORY_PRODUCT_TAN, [SBO TAN] AS SBO_TAN, 
			   [SALES VALUE] AS SALES_VALUE, [INC VALUE] AS INC_VALUE, 
			   [PLAN QTY] AS PLAN_QTY, [PLAN VALUE] AS PLAN_VALUE, 
			   [PATIENT/MONTH] AS PATIENT_MONTH, [PATIENT/QUARTER] AS PATIENT_QUARTER, 
			   [PATIENT/YEAR] AS PATIENT_YEAR, [CATEGORY OUTLET] AS CATEGORY_OUTLET, 
			   GP, PRD, [NIK MR] AS NIK_MR, MR, [NIK AM] AS NIK_AM, AM, [NIK RM] AS NIK_RM, [NIK PPM] AS NIK_PPM,PPM,
			   RM, [BO TAN] AS BO_TAN, REGION, [UNIT PRICE] AS UNIT_PRICE, OSE, KAE, KAM, 
			   [KAM CATEGORY] AS KAM_CATEGORY, [CLASS OUTLET] AS CLASS_OUTLET,
			   [AREA/OSE] AS AREA_OSE, [GROUP BY FIN] AS GROUP_BY_FIN, TABLET, DOSSAGE, 
			   FOCUS, THEURAPTICS, PM,[Month Num] as month_num, t_aso_id ,[Lot Number] as lot_number,
			   [Expired Date] as expired_date FROM v_aso_target  WHERE DATA IN (''TARGET'')
				UNION ALL
				SELECT [Month Name] AS Month_Name, YEAR,[Full Date] as full_date, [Branch Sales Desc] AS Branch_Sales_Desc, 
			   [Channel Outlet Group] AS Channel_Outlet_Group, [Channel Outlet] AS Channel_Outlet,
			   [Customer Id] AS Customer_Id, [Customer Name] AS Customer_Name,  
			   City, [Territory Geo Name] AS Territory_Geo_Name, [Sales Category Name] AS Sales_Category_Name, 
			   [Product Code] AS Product_Code, [Product Desc] AS Product_Desc, [Invoice No] AS Invoice_No, 
			   Hna, [Sales Quantity] AS Sales_Quantity, [Sales Gross] AS Sales_Gross, 
			   [Discount Rp] AS Discount_Rp, [% Disc] AS persen_Disc, 
			   DATA, [SALES GROSS TAN] AS SALES_GROSS_TAN, [DISCOUNT RP TAN] AS DISCOUNT_RP_TAN, 
			   [CATEGORY PRODUCT TAN] AS CATEGORY_PRODUCT_TAN, [SBO TAN] AS SBO_TAN, 
			   [SALES VALUE] AS SALES_VALUE, [INC VALUE] AS INC_VALUE, 
			   [PLAN QTY] AS PLAN_QTY, [PLAN VALUE] AS PLAN_VALUE, 
			   [PATIENT/MONTH] AS PATIENT_MONTH, [PATIENT/QUARTER] AS PATIENT_QUARTER, 
			   [PATIENT/YEAR] AS PATIENT_YEAR, [CATEGORY OUTLET] AS CATEGORY_OUTLET, 
			   GP, PRD, [NIK MR] AS NIK_MR, MR, [NIK AM] AS NIK_AM, AM, [NIK RM] AS NIK_RM, [NIK PPM] AS NIK_PPM,PPM,
			   RM, [BO TAN] AS BO_TAN, REGION, [UNIT PRICE] AS UNIT_PRICE, OSE, KAE, KAM, 
			   [KAM CATEGORY] AS KAM_CATEGORY, [CLASS OUTLET] AS CLASS_OUTLET,
			   [AREA/OSE] AS AREA_OSE, [GROUP BY FIN] AS GROUP_BY_FIN, TABLET, DOSSAGE, 
			   FOCUS, THEURAPTICS, PM,[Month Num] as month_num, t_aso_id ,[Lot Number] as lot_number,
			   [Expired Date] as expired_date FROM v_aso_sales WHERE DATA IN (''SALES'')
			   ) as t_aso_koin'

		--SET @sqlstr = CONCAT(@sqlstr,' WHERE (CONVERT (DATE,full_date) >= ', @dateStart , ' AND CONVERT (DATE,full_date) <=  ',  @dateEnd ,') AND NIK_RM =', @nik)
		--select @sqlstr

SET @sqlstr = CONCAT(@sqlstr,' WHERE (CONVERT (DATE,full_date) >= ''', @dateStart , ''' AND CONVERT (DATE,full_date) <= ''',  @dateEnd ,''') AND NIK_MR = ''', @nik,'''')
Exec sp_executesql @sqlstr
		



