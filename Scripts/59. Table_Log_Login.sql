USE [bas]
GO

/****** Object:  Table [dbo].[log_login]    Script Date: 9/21/2022 1:19:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[log_login](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[rep_id] [char](5) NULL,
	[hostname] [varchar](50) NULL,
	[ip_addressv4] [varchar](20) NULL,
	[latitude] [varchar](250) NULL,
	[longitude] [varchar](250) NULL,
	[address] [varchar](800) NULL,
	[log_date] [datetime] NULL,
	[date_created] [datetime] NULL,
	[created_by] [varchar](50) NULL,
	[status] [varchar](50) NULL,
	[notes] [varchar](256) NULL,
	[source] [varchar](10) NULL,
 CONSTRAINT [PK_log_login] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


