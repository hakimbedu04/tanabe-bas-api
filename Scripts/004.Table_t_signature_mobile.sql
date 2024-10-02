USE [bas_trial]
GO

/****** Object:  Table [dbo].[t_signature_mobile]    Script Date: 4/1/2019 3:41:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[t_signature_mobile](
	[signature_id] [int] IDENTITY(1,1) NOT NULL,
	[visit_id] [nvarchar](50) NULL,
	[rep_id] [nvarchar](50) NULL,
	[dr_code] [int] NULL,
	[sign] [bit] NULL,
	[file_upload] [varchar](100) NULL,
	[reason] [varchar](1000) NULL,
	[created_at] [date] NULL,
	[updated_at] [date] NULL,
 CONSTRAINT [PK_t_signature_mobile] PRIMARY KEY CLUSTERED 
(
	[signature_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


