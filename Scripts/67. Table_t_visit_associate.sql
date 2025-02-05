USE [bas]
GO
/****** Object:  Table [dbo].[t_visit_associate]    Script Date: 08/09/2023 16.08.08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_visit_associate](
	[associate_id] [int] IDENTITY(1,1) NOT NULL,
	[visit_id] [varchar](15) NOT NULL,
	[visit_id_associated] [varchar](15) NOT NULL,
	[associate_status] [int] NULL,
 CONSTRAINT [PK_t_visit_associate] PRIMARY KEY CLUSTERED 
(
	[associate_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[t_visit_associate] ON 

INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (1, N'V23118611', N'V23111907', 1)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (2, N'V23119033', N'V23111904', 1)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (3, N'V23119045', N'V23119046', 0)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (5, N'V23118614', N'V23113926', 1)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (6, N'V23118615', N'V23114278', 0)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (7, N'V23118616', N'V23114280', 0)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (8, N'V23118617', N'V23114596', 0)
INSERT [dbo].[t_visit_associate] ([associate_id], [visit_id], [visit_id_associated], [associate_status]) VALUES (9, N'V23118618', N'V23114563', 0)
SET IDENTITY_INSERT [dbo].[t_visit_associate] OFF
