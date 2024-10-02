USE [bas]
GO

/****** Object:  View [dbo].[v_rep_full_new]    Script Date: 7/3/2019 2:10:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_rep_full_new]
AS
SELECT        rep_id, rep_full_name, phone, birth_place, birth_date, address, rep_region, rep_sbo, ProfilePicture, rep_position
FROM            (SELECT        mr.rep_id, mr.rep_name, m_sbo.sbo_id, RTRIM(m_sbo.sbo_code) AS rep_sbo, m_sbo.bo_id, dbo.m_bo.bo_code AS rep_bo, mr.rep_position, 
                                                    mr.rep_division, mr.rep_email, mr.rep_status, mr.rep_inactive_date, ak.Nama AS rep_full_name, m_sbo.sbo_description, dbo.m_bo.bo_description, 
                                                    dbo.m_bo.bo_am AS rep_am, akam.Nama AS nama_am, akam.Email AS email_am, CONVERT(nchar(5), dbo.m_regional.reg_functionary) AS rep_rm, 
                                                    akrm.Nama AS nama_rm, akrm.Email AS email_rm, m_sbo.sbo_ppm AS rep_ppm, akppm.Nama AS nama_ppm, m_sbo.sbo_status, dbo.m_bo.reg_id, 
                                                    dbo.m_regional.reg_code AS rep_region, mr.rep_bank_account, mr.rep_bank_code, akrm.No_Handphone AS phone, akrm.Tempat_Lahir AS birth_place, 
                                                    akrm.Tanggal_Lahir AS birth_date, akrm.Alamat AS address,
                                                        (SELECT        profile_picture_path
                                                          FROM            dbo.m_rep
                                                          WHERE        (rep_id = mr.rep_id)) AS ProfilePicture
                          FROM            dbo.m_bo RIGHT OUTER JOIN
                                                        (SELECT        sbo_id, sbo_code, bo_id, sbo_description, sbo_address, sbo_sequence_code, sbo_rep, sbo_ppm, sbo_status
                                                          FROM            dbo.m_sbo AS m_sbo_1
                                                          WHERE        (sbo_status = 1)) AS m_sbo ON dbo.m_bo.bo_id = m_sbo.bo_id LEFT OUTER JOIN
                                                    dbo.m_regional LEFT OUTER JOIN
                                                    hrd.dbo.Karyawan AS akrm ON dbo.m_regional.reg_functionary = akrm.Nomor_Induk ON dbo.m_bo.reg_id = dbo.m_regional.reg_id LEFT OUTER JOIN
                                                    hrd.dbo.Karyawan AS akam ON dbo.m_bo.bo_am = akam.Nomor_Induk LEFT OUTER JOIN
                                                    hrd.dbo.Karyawan AS akppm ON m_sbo.sbo_ppm = akppm.Nomor_Induk RIGHT OUTER JOIN
                                                    dbo.m_rep AS mr LEFT OUTER JOIN
                                                    hrd.dbo.Karyawan AS ak ON mr.rep_id = CONVERT(nchar(5), ak.Nomor_Induk) ON m_sbo.sbo_rep = mr.rep_id) AS tbl

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "tbl"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
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
         Alias = 900
         Table = 1170
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_rep_full_new'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_rep_full_new'
GO


