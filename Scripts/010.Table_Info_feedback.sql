USE [bas_trial]
GO
/****** Object:  Table [dbo].[m_info_feedback_mobile]    Script Date: 4/14/2019 11:17:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_info_feedback_mobile](
	[info_feedback_id] [int] IDENTITY(1,1) NOT NULL,
	[info_feedback_type] [int] NULL,
	[info_description] [varchar](250) NULL,
 CONSTRAINT [PK_m_info_feedback_mobile] PRIMARY KEY CLUSTERED 
(
	[info_feedback_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_info_feedback_topic_mapping_mobile]    Script Date: 4/14/2019 11:17:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_info_feedback_topic_mapping_mobile](
	[info_feedback_topic_mapping_id] [int] IDENTITY(1,1) NOT NULL,
	[topic_id] [int] NULL,
	[info_feedback_id] [int] NULL,
 CONSTRAINT [PK_t_info_feedback_topic_mapping_mobile] PRIMARY KEY CLUSTERED 
(
	[info_feedback_topic_mapping_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[m_info_feedback_mobile] ON 

GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (1, 1, N'Pasien tidak batuk')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (2, 1, N'Ada perbaikan Dysphagianya
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (3, 1, N'Proteksi terhadap fungsi ginjal
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (4, 1, N'Bagus untuk pasien DM tipe 2
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (5, 1, N'Mencapai target TD yang diinginkan
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (6, 1, N'Bagus utk pasiem DM, minimal dalam peningkatan glukosa darah
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (7, 1, N'Pasien lebih nyaman
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (8, 1, N'Proteksi pembuluh darah
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (9, 1, N'Bagus dalam memurunkan & mengontrol kolesterol
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (10, 1, N'Mempunyai hasil yang baik, khususnya diberikan pada populasi Asia
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (11, 1, N'Antihistamin memang butuh kerja cepat untuk atasi gejala yang mengganggu
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (12, 1, N'Tidak perlu khawatir mengantuk
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (13, 1, N'Efikasi talion baik
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (14, 1, N'Tidak khawatir kombinasi dengan obat lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (15, 1, N'Bisa dikonsumsi kapan saja
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (16, 1, N'Ya itu kelebihannya Aspar-K
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (17, 1, N'Kasus hipokalemia membutuhkan suplemen kalium
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (18, 1, N'Pemberian secara infus langsung memberikan efek yang cepat
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (19, 1, N'Pasien mudah mengingat jadwal terapi
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (20, 1, N'Pasien yang sudah severe harus menggunakan biologic agent seperti Remicade
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (21, 1, N'Semua obat harus dibawah pengawasan dokter
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (22, 1, N'Saya sudah tahu Remicade lama 
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (23, 1, N'Dengan komposisi Fully human meminimalisir reaksi alergi
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (24, 1, N'Canggih sediaannya
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (25, 1, N'Dosisnya mudah diingat dan simple
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (26, 1, N'Relatif lebih murah
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (27, 1, N'bleeding risk lebih minimal
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (28, 1, N'kualitas hidup pasien jadi lebih baik
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (29, 1, N'kenyamanan pasien krn Once daily
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (30, 1, N'kelas 1B untuk ACS-NSTEMI
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (31, 1, N'Ubiquinone berperan dalam pembentukan energi
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (32, 1, N'mempercepat penyembuhan dengan memperbaiki sel jantung
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (33, 1, N'pasien pengguna statin pasti mengalamin myalgia jadi ubiq untuk mengurangi efek samping tersebut
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (34, 1, N'absorbsinya dan efeknya bagus
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (36, 2, N'masih ada batuk dan cukup mengganggu
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (37, 2, N'Jarang ditemui kasusnya
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (38, 2, N'Semua Ace-I punya efek yg sama
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (39, 2, N'Tidak pengaruh, sudah ada antidiabetic
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (40, 2, N'Masih bagus ARB dan tidak batuk
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (41, 2, N'No respon')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (42, 2, N'Tidak masalah/individualis')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (43, 2, N'Anti inflamasinya lebih kuat atorvastatin dan rosuvastatin
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (44, 2, N'Penurunan LDL-C kurang agresif dibanding Atorvasstatin dan Rosuvastatin
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (45, 2, N'Tidak dibandingkan dengan statin lainnya
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (46, 2, N'Sama saja dengan yang lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (47, 2, N'Harganya mahal
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (48, 2, N'Kerja obat alergi hampir sama
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (49, 2, N'harga relatif mahal dibanding antihistamin lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (50, 2, N'Efikasinya bagaimana
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (51, 2, N'Pasien saya tidak masalah menggunakan sediaan KCl
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (52, 2, N'Suplemen kalium sama saja
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (53, 2, N'Bisa didapatkan dari yang lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (54, 2, N'lama menunggu 2-3jam
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (55, 2, N'Pasien sering ke rumah sakit  untuk infus
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (56, 2, N'Harga mahal, tidak semua pasien mampu beli
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (57, 2, N'lama menunggu 2-3jam
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (58, 2, N'Obat biologic lama, banyak generasi baru
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (59, 2, N'Efek sampingnya banyak yg lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (60, 2, N'Seberapa kuat efikasinya?
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (61, 2, N'Harga biologic sama saja, mahal
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (62, 2, N'tergantung dengan BB & CrCl
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (63, 2, N'individual, tergantung dg BB & CrCl
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (64, 2, N'tergantung dengan BB & CrCl
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (65, 2, N'anti dotnya belum ada
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (66, 2, N'Terbatas hanya untuk pasien ACS-NSTEMI
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (67, 2, N'Hanya suplemen saja
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (68, 2, N'Obat jantung sudah banyak dan mahal, kasian pasien
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (69, 2, N'bisa menggunakan suplemen lain
')
GO
INSERT [dbo].[m_info_feedback_mobile] ([info_feedback_id], [info_feedback_type], [info_description]) VALUES (70, 2, N'sama saja, yang penting obatnya
')
GO
SET IDENTITY_INSERT [dbo].[m_info_feedback_mobile] OFF
GO
SET IDENTITY_INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ON 

GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (1, 1, 1)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (2, 2, 2)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (3, 3, 3)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (4, 4, 4)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (5, 5, 5)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (6, 21, 6)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (7, 22, 4)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (8, 23, 3)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (9, 24, 8)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (10, 25, 9)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (11, 26, 11)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (12, 27, 12)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (13, 28, 13)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (14, 29, 14)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (15, 30, 15)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (16, 31, 16)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (17, 32, 16)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (18, 33, 17)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (19, 34, 18)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (20, 35, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (21, 36, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (22, 37, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (23, 38, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (24, 39, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (25, 40, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (26, 41, NULL)
GO
INSERT [dbo].[t_info_feedback_topic_mapping_mobile] ([info_feedback_topic_mapping_id], [topic_id], [info_feedback_id]) VALUES (27, 42, NULL)
GO
SET IDENTITY_INSERT [dbo].[t_info_feedback_topic_mapping_mobile] OFF
GO
