USE [bas]
GO

/****** Object:  Table [dbo].[t_shift_log]    Script Date: 7/16/2019 2:57:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[t_shift_log](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[visit_id] [varchar](20) NULL,
	[rep_id] [varchar](5) NULL,
	[visit_date_plan] [datetime] NULL,
	[prev_visit_date] [datetime] NULL,
	[created_date] [datetime] NULL,
	[updated_date] [datetime] NULL,
	[is_active] [int] NULL,
 CONSTRAINT [PK_t_shift_log] PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[t_shift_log] ADD  CONSTRAINT [DF__t_shift_l__is_ac__3C9FD11A]  DEFAULT ((1)) FOR [is_active]
GO


