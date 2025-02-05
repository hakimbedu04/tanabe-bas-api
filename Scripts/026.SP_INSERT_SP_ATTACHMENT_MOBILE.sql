CREATE Procedure [dbo].[SP_INSERT_SP_ATTACHMENT_MOBILE]
@visit_id char(15),
@file_name nvarchar(150),
@file_path nvarchar(250)

AS
DECLARE @e_dt_start date
DECLARE @datePlan date = CONVERT(date, @e_dt_start)
DECLARE @dr_code int 
SELECT @dr_code = dr_code, @e_dt_start = visit_date_plan FROM t_visit WHERE visit_id=@visit_id

DECLARE @spr_id varchar(15) = (SELECT spr_id FROM v_doctor_sponsor WHERE dr_code = @dr_code AND YEAR(e_dt_start) = YEAR(@e_dt_start) 
AND  MONTH(e_dt_start) = MONTH(@e_dt_start) AND DAY(e_dt_start) = DAY(@e_dt_start) AND  sp_type = 'SP2')

if @spr_id = NULL
	Begin
		set @spr_id = @visit_id
	end

INSERT INTO [dbo].[t_sp_attachment]
           ([spr_id]
           ,[spf_file_name]
           ,[spf_file_path]
           ,[spf_date_uploaded])
     VALUES
           (@spr_id
           ,@file_name
           ,@file_path
           ,GETDATE())

INSERT INTO [dbo].[t_sp_attachment_temp]
           ([visit_id]
		   ,[spr_id]
           ,[spf_file_name]
           ,[spf_file_path]
           ,[spf_date_uploaded])
     VALUES
           (@visit_id
		   ,@spr_id
           ,@file_name
           ,@file_path
           ,GETDATE())