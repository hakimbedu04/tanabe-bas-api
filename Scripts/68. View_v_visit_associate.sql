USE [bas]
GO
/****** Object:  View [dbo].[v_visit_associate]    Script Date: 08/09/2023 16.11.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_visit_associate]
AS
SELECT        dbo.t_visit_associate.associate_id, dbo.t_visit_associate.visit_id AS visit_id_initiator, visit_initiator.rep_id AS rep_id_initiator, visit_initiator.dr_code AS dr_code_initiator, 
                         dbo.t_visit_associate.visit_id_associated AS visit_id_invited, visit_invited.rep_id AS rep_id_invited, visit_invited.dr_code AS dr_code_invited, dbo.t_visit_associate.associate_status, visit_initiator.visit_code
FROM            dbo.t_visit_associate LEFT OUTER JOIN
                         dbo.t_visit AS visit_invited ON dbo.t_visit_associate.visit_id_associated = visit_invited.visit_id LEFT OUTER JOIN
                         dbo.t_visit AS visit_initiator ON dbo.t_visit_associate.visit_id = visit_initiator.visit_id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[30] 2[11] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "t_visit_associate"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 212
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "visit_invited"
            Begin Extent = 
               Top = 170
               Left = 559
               Bottom = 300
               Right = 797
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "visit_initiator"
            Begin Extent = 
               Top = 18
               Left = 430
               Bottom = 148
               Right = 668
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2790
         Table = 3225
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_visit_associate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_visit_associate'
GO
