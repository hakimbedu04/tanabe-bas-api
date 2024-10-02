USE [bas_trial]
GO

/****** Object:  Table [dbo].[t_sp_attachment_temp]    Script Date: 4/9/2019 6:37:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[t_sp_attachment_temp](
	[id_attachment_temp] [int] IDENTITY(1,1) NOT NULL,
	[visit_id] [varchar](50) NULL,
	[spr_id] [varchar](50) NULL,
	[spf_file_name] [varchar](100) NULL,
	[spf_file_path] [varchar](100) NULL,
	[spf_date_uploaded] [date] NULL,
 CONSTRAINT [PK_t_sp_attachment_temp] PRIMARY KEY CLUSTERED 
(
	[id_attachment_temp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


