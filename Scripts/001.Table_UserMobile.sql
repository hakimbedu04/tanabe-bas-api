USE [bas_trial]
GO

/****** Object:  Table [dbo].[usermobile]    Script Date: 3/14/2019 11:04:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[usermobile](
	[IdUserMobile] [int] IDENTITY(1,1) NOT NULL,
	[rep_id] [char](5) NULL,
	[UserToken] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_usermobile] PRIMARY KEY CLUSTERED 
(
	[IdUserMobile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[usermobile]  WITH CHECK ADD  CONSTRAINT [FK_usermobile_usermobile] FOREIGN KEY([rep_id])
REFERENCES [dbo].[m_rep] ([rep_id])
GO

ALTER TABLE [dbo].[usermobile] CHECK CONSTRAINT [FK_usermobile_usermobile]
GO


