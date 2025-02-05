USE [bas]
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_Bagian]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_Bagian]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	select RTRIM([Kode Bagian]) as [Kode Bagian]
      ,[Nama Bagian]
      ,[Singkatan Bagian]
      ,RTRIM([Kode Seksi]) as [Kode Seksi]
      ,[hq]
	from hrd.dbo.Bagian;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_Departemen]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_Departemen]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT
	[Kode_Departemen]
	,[Nama_Departemen]
	,[Kode_Headquarter]
	,RTRIM([NIK]) As NIK
	,[kd_dept_asal]
	,[nama_dept_asal]
	,[kd_hq_asal]
	,[kd_hq_contrary]
	,[ParentID]
	,[IndexID]
	,[isActive]
	,[Disabled]
	FROM hrd.dbo.Departemen;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_HeadQuarter]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_HeadQuarter]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	select [Kode_Headquarter],[Nama_Headquarter],RTRIM([NIK]) as NIK from hrd.dbo.HeadQuarter;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_Jabatan]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_Jabatan]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	select * from hrd.dbo.Jabatan;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_Karyawan]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_Karyawan]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	
	DECLARE @nik_atasan VARCHAR(5) = (SELECT nik_atasan FROM hrd.dbo.Karyawan where RTRIM([Nomor_Induk]) = @rep_id)
	select 
	RTRIM([Nomor_Induk]) as [Nomor_Induk]
      ,[Nama]
      ,[Title]
      ,[TitelBelakang]
      ,[Nomor_Astek]
      ,[Jenis_Kelamin]
      ,[Tempat_Lahir]
      ,[Tanggal_Lahir]
      ,[Alamat]
      ,[Alamat2]
      ,[Nomor_Telepon]
      ,[Agama]
      ,[Pendidikan]
      ,[Tanggal_Kerja]
      ,[Tanggal_Diangkat]
      ,[Tanggal_Keluar]
      ,[Kode_Jabatan]
      ,[Kode_Departemen]
      ,[Nama_Seksi]
      ,[Nama_Cabang]
      ,[Status]
      ,[Nama_Pasangan]
      ,[Foto]
      ,[Nama_Ibu]
      ,[Nama_Bapak]
      ,[Nomor_KTP]
      ,[Alasan_Keluar]
      ,[Status_Kerja]
      ,[Tinggi_Badan]
      ,[Berat_Badan]
      ,[Golongan_Darah]
      ,[Email]
      ,[Tingkat_Pendidikan]
      ,[Nomor_Urut]
      ,[Kategori]
      ,[Jabatan k3lh]
      ,[Bagian k3lh]
      ,[Unit k3lh]
      ,RTRIM([Kode Bagian]) as [Kode Bagian]
      ,[Tanggal Verifikasi]
      ,[Tgl Sertifikasi MR]
      ,[No Sertifikat MR]
      ,[UserID]
      ,[Pwd]
      ,[Aktif]
      ,RTRIM([NIK_Atasan]) as [NIK_Atasan]
      ,RTRIM([No_Peserta_DPLK]) as [No_Peserta_DPLK]
      ,RTRIM([No_Kartu_DPLK]) as [No_Kartu_DPLK]
      ,[Awal_Kepesertaan]
      ,[Akhir_Kepesertaan]
      ,[Keterangan]
      ,[Nama_DPLK]
      ,RTRIM([NPWP]) as [NPWP]
      ,[ANGGOTA_SP]
      ,[IURAN_SP]
      ,[TUNJANGAN_ISTRI]
      ,[TUNJANGAN_ANAK]
      ,[S_OR_M]
      ,[JML_ANAK_PPH]
      ,[STRATEGIC_ALLOWANCE]
      ,[FASILITAS_KENDARAAN]
      ,[Attandance]
      ,RTRIM([PIN]) as [PIN]
      ,[Warga_Negara]
      ,[No_Handphone]
      ,[Status_Tempat_Tinggal]
      ,[Alamat_Darurat]
      ,[No_Telepon_Darurat]
      ,[No_Handphone_Darurat]
      ,[Nama_Yang_Dihubungi]
      ,[Hubungan_Keluarga]
      ,[Status_Pernikahan]
      ,[No_Identitas_Diri]
      ,[Masa_Berlaku_Identitas_Diri]
      ,[Tempat_Lahir_Bapak]
      ,[Tempat_Lahir_Ibu]
      ,[Tanggal_Lahir_Bapak]
      ,[Tanggal_Lahir_Ibu]
      ,[Alamat_Bapak]
      ,[Alamat_Ibu]
      ,[Pekerjaan_Bapak]
      ,[Pekerjaan_Ibu]
      ,[Jabatan_Penugasan]
      ,[Status_Aktif]
      ,[Alasan_Non_Aktif]
      ,[No_Ref_Job_Desc]
      ,[Revisi_Job_Desc_Ke]
      ,RTRIM([Tgl_Berlaku_Job_Desc]) as [Tgl_Berlaku_Job_Desc]
      ,[Nama_File_Job_Desc]
      ,[Fasilitas]
      ,[Alasan_Keluar2]
      ,[Kode_Divisi]
      ,[Email2]
	from hrd.dbo.Karyawan where Nomor_Induk in (@rep_id,@nik_atasan);
	--DECLARE @nik_atasan VARCHAR(5) = (SELECT nik_atasan FROM hrd.dbo.Karyawan where RTRIM([Nomor_Induk]) = @rep_id)
	--select 
	--RTRIM([Nomor_Induk]) as [Nomor_Induk]
 --     ,[Nama]
 --     ,[Title]
 --     ,[TitelBelakang]
 --     ,[Nomor_Astek]
 --     ,[Jenis_Kelamin]
 --     ,[Tempat_Lahir]
 --     ,[Tanggal_Lahir]
 --     ,[Alamat]
 --     ,[Alamat2]
 --     ,[Nomor_Telepon]
 --     ,[Agama]
 --     ,[Pendidikan]
 --     ,[Tanggal_Kerja]
 --     ,[Tanggal_Diangkat]
 --     ,[Tanggal_Keluar]
 --     ,[Kode_Jabatan]
 --     ,[Kode_Departemen]
 --     ,[Nama_Seksi]
 --     ,[Nama_Cabang]
 --     ,[Status]
 --     ,[Nama_Pasangan]
 --     ,[Foto]
 --     ,[Nama_Ibu]
 --     ,[Nama_Bapak]
 --     ,[Nomor_KTP]
 --     ,[Alasan_Keluar]
 --     ,[Status_Kerja]
 --     ,[Tinggi_Badan]
 --     ,[Berat_Badan]
 --     ,[Golongan_Darah]
 --     ,[Email]
 --     ,[Tingkat_Pendidikan]
 --     ,[Nomor_Urut]
 --     ,[Kategori]
 --     ,[Jabatan k3lh]
 --     ,[Bagian k3lh]
 --     ,[Unit k3lh]
 --     ,RTRIM([Kode Bagian]) as [Kode Bagian]
 --     ,[Tanggal Verifikasi]
 --     ,[Tgl Sertifikasi MR]
 --     ,[No Sertifikat MR]
 --     ,[UserID]
 --     ,[Pwd]
 --     ,[Aktif]
 --     ,RTRIM([NIK_Atasan]) as [NIK_Atasan]
 --     ,RTRIM([No_Peserta_DPLK]) as [No_Peserta_DPLK]
 --     ,RTRIM([No_Kartu_DPLK]) as [No_Kartu_DPLK]
 --     ,[Awal_Kepesertaan]
 --     ,[Akhir_Kepesertaan]
 --     ,[Keterangan]
 --     ,[Nama_DPLK]
 --     ,RTRIM([NPWP]) as [NPWP]
 --     ,[ANGGOTA_SP]
 --     ,[IURAN_SP]
 --     ,[TUNJANGAN_ISTRI]
 --     ,[TUNJANGAN_ANAK]
 --     ,[S_OR_M]
 --     ,[JML_ANAK_PPH]
 --     ,[STRATEGIC_ALLOWANCE]
 --     ,[FASILITAS_KENDARAAN]
 --     ,[Attandance]
 --     ,RTRIM([PIN]) as [PIN]
 --     ,[Warga_Negara]
 --     ,[No_Handphone]
 --     ,[Status_Tempat_Tinggal]
 --     ,[Alamat_Darurat]
 --     ,[No_Telepon_Darurat]
 --     ,[No_Handphone_Darurat]
 --     ,[Nama_Yang_Dihubungi]
 --     ,[Hubungan_Keluarga]
 --     ,[Status_Pernikahan]
 --     ,[No_Identitas_Diri]
 --     ,[Masa_Berlaku_Identitas_Diri]
 --     ,[Tempat_Lahir_Bapak]
 --     ,[Tempat_Lahir_Ibu]
 --     ,[Tanggal_Lahir_Bapak]
 --     ,[Tanggal_Lahir_Ibu]
 --     ,[Alamat_Bapak]
 --     ,[Alamat_Ibu]
 --     ,[Pekerjaan_Bapak]
 --     ,[Pekerjaan_Ibu]
 --     ,[Jabatan_Penugasan]
 --     ,[Status_Aktif]
 --     ,[Alasan_Non_Aktif]
 --     ,[No_Ref_Job_Desc]
 --     ,[Revisi_Job_Desc_Ke]
 --     ,RTRIM([Tgl_Berlaku_Job_Desc]) as [Tgl_Berlaku_Job_Desc]
 --     ,[Nama_File_Job_Desc]
 --     ,[Fasilitas]
 --     ,[Alasan_Keluar2]
 --     ,[Kode_Divisi]
 --     ,[Email2]
	--from hrd.dbo.Karyawan --where Nomor_Induk in (@rep_id,@nik_atasan);
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_bo]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_bo]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	[bo_id]
      ,RTRIM([bo_code]) as [bo_code]
      ,[reg_id]
      ,[bo_description]
      ,[bo_address]
      ,[bo_sequence_code]
      ,RTRIM([bo_am]) as [bo_am]
      ,[bo_status]
      ,[bo_area]
	 FROM m_bo;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_customer_aso]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_customer_aso]
@rep_id char(50),
@month int,
@year int

AS
BEGIN

	SELECT distinct m_customer_aso.* 
	FROM  m_customer_aso RIGHT OUTER JOIN
	m_doctor ON m_customer_aso.cust_id = m_doctor.cust_id
	WHERE dr_code in (
						SELECT DISTINCT m_doctor.dr_code FROM m_doctor
						RIGHT OUTER JOIN t_visit ON m_doctor.dr_code = t_visit.dr_code
						where rep_id = @rep_id AND
						MONTH(visit_date_plan) = @month AND 
						YEAR(visit_date_plan) = @year
					 );
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_doctor]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_doctor]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT DISTINCT m_doctor.[dr_code]
      ,RTRIM(m_doctor.[dr_sbo]) as dr_sbo
      ,m_doctor.[dr_name]
      ,RTRIM(m_doctor.[dr_spec]) as dr_spec
      ,RTRIM(m_doctor.[dr_sub_spec]) as dr_sub_spec
      ,RTRIM(m_doctor.[dr_quadrant]) as dr_quadrant
      ,m_doctor.[cust_id]
      ,m_doctor.[dr_address]
      ,RTRIM(m_doctor.[dr_area_mis]) as [dr_area_mis]
      ,m_doctor.[dr_sum]
      ,m_doctor.[dr_category]
      ,m_doctor.[dr_sub_category]
      ,m_doctor.[dr_chanel]
      ,m_doctor.[dr_day_visit]
      ,m_doctor.[dr_visiting_hour]
      ,m_doctor.[dr_number_patient]
      ,m_doctor.[dr_kol_not]
      ,RTRIM(m_doctor.[dr_gender]) as [dr_gender]
      ,m_doctor.[dr_phone]
      ,m_doctor.[dr_email]
      ,m_doctor.[dr_birthday]
      ,RTRIM(m_doctor.[dr_dk_lk]) as [dr_dk_lk]
      ,m_doctor.[dr_used_session]
      ,m_doctor.[dr_used_remaining]
      ,m_doctor.[dr_used_month_session]
      ,m_doctor.[dr_status]
      ,m_doctor.[dr_sales_session]
      ,m_doctor.[dr_sales_month_session]
	FROM m_doctor
	RIGHT OUTER JOIN t_visit ON m_doctor.dr_code = t_visit.dr_code
	where rep_id = @rep_id AND
	MONTH(visit_date_plan) = @month AND 
	YEAR(visit_date_plan) = @year
	UNION ALL
	SELECT DISTINCT 
		m_doctor.[dr_code]
      ,RTRIM(m_doctor.[dr_sbo]) as dr_sbo
      ,m_doctor.[dr_name]
      ,RTRIM(m_doctor.[dr_spec]) as dr_spec
      ,RTRIM(m_doctor.[dr_sub_spec]) as dr_sub_spec
      ,RTRIM(m_doctor.[dr_quadrant]) as dr_quadrant
      ,m_doctor.[cust_id]
      ,m_doctor.[dr_address]
      ,RTRIM(m_doctor.[dr_area_mis]) as [dr_area_mis]
      ,m_doctor.[dr_sum]
      ,m_doctor.[dr_category]
      ,m_doctor.[dr_sub_category]
      ,m_doctor.[dr_chanel]
      ,m_doctor.[dr_day_visit]
      ,m_doctor.[dr_visiting_hour]
      ,m_doctor.[dr_number_patient]
      ,m_doctor.[dr_kol_not]
      ,RTRIM(m_doctor.[dr_gender]) as [dr_gender]
      ,m_doctor.[dr_phone]
      ,m_doctor.[dr_email]
      ,m_doctor.[dr_birthday]
      ,RTRIM(m_doctor.[dr_dk_lk]) as [dr_dk_lk]
      ,m_doctor.[dr_used_session]
      ,m_doctor.[dr_used_remaining]
      ,m_doctor.[dr_used_month_session]
      ,m_doctor.[dr_status]
      ,m_doctor.[dr_sales_session]
      ,m_doctor.[dr_sales_month_session]
	FROM m_doctor
	where dr_code = 100005;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_event]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_event]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
		[event_id]
		,[event_description]
		,[event_detail_description]
		,[event_scale]
		,[event_place]
		,RTRIM([event_sp]) as event_sp
		,RTRIM([event_budget]) as event_budget
		,[event_date_saved]
		,RTRIM([event_saved_by]) as event_saved_by
		,[event_last_updated]
		,RTRIM([event_last_updated_by]) as [event_last_updated_by]
		,[event_status]
	FROM m_event
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_gl_cs]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_gl_cs]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
SELECT * FROM m_gl_cs
WHERE o_id in (
	SELECT visit_budget_ownership
    FROM   m_product
)
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_product]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_product]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT DISTINCT
	[prd_aso_id]
	,RTRIM([prd_code]) as [prd_code]
	,RTRIM([prd_focus]) as [prd_focus]
	,[prd_type]
	,RTRIM([visit_code]) as [visit_code]
	,[visit_product]
	,[visit_category]
	,RTRIM([visit_group]) as [visit_group]
	,RTRIM([visit_team]) as [visit_team]
	,[visit_budget_ownership]
	,[prd_aso_desc]
	,[prd_aso_type]
	,RTRIM([prd_aso_category]) as [prd_aso_category]
	,[prd_aso_program]
	,[prd_aso_join_desc]
	,[price_id]
	,RTRIM([prd_aso_gp]) as [prd_aso_gp]
	,RTRIM([prd_aso_ose]) as [prd_aso_ose]
	,RTRIM([prd_aso_group]) as [prd_aso_group]
	,RTRIM([prd_aso_group_fin]) as [prd_aso_group_fin]
	,[prd_aso_tab]
	,[prd_aso_dossage]
	,[prd_aso_dostime]
	,[prd_aso_tc]
	,[prd_status]
	,RTRIM([prd_saf_code]) as [prd_saf_code] 
	FROM m_product WHERE prd_status = 1 and visit_code not in ('t0','t00')
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_regional]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_regional]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	[reg_id]
    ,RTRIM([reg_code]) as reg_code
    ,[reg_description]
    ,RTRIM([reg_functionary]) as reg_functionary
    ,[reg_sequence_code]
    ,[reg_status]
	FROM m_regional;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_rep]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_rep]
@rep_id char(50),
@month int,
@year int

AS
BEGIN

	DECLARE @nik_atasan VARCHAR(5) = (SELECT nik_atasan FROM hrd.dbo.Karyawan where RTRIM([Nomor_Induk]) = @rep_id)

	SELECT
	RTRIM([rep_id]) as rep_id
    ,[rep_name]
    ,[rep_position]
    ,[rep_division]
    ,[rep_email]
    ,[rep_minvday]
    ,[rep_maxvday]
    ,[rep_maxaddvday]
    ,[rep_bank_account]
    ,[rep_bank_code]
    ,[rep_status]
    ,[effective_datestart]
    ,[rep_inactive_date]
    ,[role_id]
    ,[date_created]
    ,RTRIM([created_by]) as [created_by]
    ,[last_updated]
    ,RTRIM([updated_by]) as [updated_by]
    ,[profile_picture_path]	
	FROM m_rep where rep_id in (@rep_id, @nik_atasan);

	--DECLARE @nik_atasan VARCHAR(5) = (SELECT nik_atasan FROM hrd.dbo.Karyawan where RTRIM([Nomor_Induk]) = @rep_id)

	--SELECT
	--RTRIM([rep_id]) as rep_id
 --   ,[rep_name]
 --   ,[rep_position]
 --   ,[rep_division]
 --   ,[rep_email]
 --   ,[rep_minvday]
 --   ,[rep_maxvday]
 --   ,[rep_maxaddvday]
 --   ,[rep_bank_account]
 --   ,[rep_bank_code]
 --   ,[rep_status]
 --   ,[effective_datestart]
 --   ,[rep_inactive_date]
 --   ,[role_id]
 --   ,[date_created]
 --   ,RTRIM([created_by]) as [created_by]
 --   ,[last_updated]
 --   ,RTRIM([updated_by]) as [updated_by]
 --   ,[profile_picture_path]	
	--FROM m_rep --where rep_id in (@rep_id,@nik_atasan);
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_sbo]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_sbo]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT
	[sbo_id]
    ,RTRIM([sbo_code]) as [sbo_code]
    ,[bo_id]
    ,[sbo_description]
    ,[sbo_address]
    ,[sbo_sequence_code]
    ,RTRIM([sbo_shareholders]) as [sbo_shareholders]
    ,RTRIM([sbo_rep]) as [sbo_rep]
    ,RTRIM([sbo_ppm]) as [sbo_ppm]
    ,[sbo_status]
    ,[effective_datestart]
    ,[effective_dateend]
    ,[date_created]
    ,RTRIM([created_by]) as [created_by]
    ,[last_updated]
    ,RTRIM([updated_by] ) as [updated_by]
	FROM m_sbo;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_sponsor]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_sponsor]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT
	[sponsor_id]
    ,[sponsor_description]
    ,[sponsor_group]
    ,[sponsor_status]
    ,RTRIM([sponsor_sp_type]) as [sponsor_sp_type]
	FROM m_sponsor;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_status]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_status]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT * FROM m_status;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_m_topic]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_m_topic]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	[topic_id]
      ,[topic_title]
      ,RTRIM([topic_group_product]) as [topic_group_product]
      ,[topic_filepath]
      ,[topic_file_name]
      ,[topic_status]
	 FROM m_topic;
END;


GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_gps_mobile]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_gps_mobile]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT ts.[gps_id]
	  ,ts.[gps_id] as m_gps_id
      ,ts.[visit_id]
      ,ts.[rep_id]
      ,ts.[dr_code]
      ,ts.[latitude]
      ,ts.[longitude]
	FROM t_visit tv
	INNER JOIN t_gps_mobile ts on tv.visit_id = ts.visit_id
	WHERE tv.rep_id = @rep_id
	and MONTH(visit_date_plan) = @month
	and YEAR(visit_date_plan) = @year
	ORDER BY ts.gps_id ASC
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_signature_mobile]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_signature_mobile]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT ts.[signature_id]
	  ,ts.[signature_id] as m_signature_id
      ,ts.[visit_id]
      ,ts.[rep_id]
      ,ts.[dr_code]
      ,ts.[sign]
      ,ts.[file_upload]
      ,ts.[reason]
      ,ts.[created_at]
      ,ts.[updated_at]
	FROM t_visit tv
	INNER JOIN t_signature_mobile ts on tv.visit_id = ts.visit_id
	WHERE tv.rep_id = @rep_id
	and MONTH(visit_date_plan) = @month
	and YEAR(visit_date_plan) = @year
	ORDER BY ts.signature_id ASC
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_sp]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_sp]
@rep_id char(50),
@month int,
@year int

AS
BEGIN

	SELECT DISTINCT t_sp.[sp_id]
      ,t_sp.[spr_id]
      ,t_sp.[sp_type]
      ,t_sp.[sp_status]
      ,t_sp.[sp_date_realization]
      ,t_sp.[sp_date_realization_saved]
      ,t_sp.[sp_verified_by]
      ,t_sp.[sp_date_verified]
      ,t_sp.[sp_ba]
      ,t_sp.[sp_posted_by]
      ,t_sp.[sp_posted_date]
	FROM
	-- m_doctor
	--RIGHT OUTER JOIN t_visit ON m_doctor.dr_code = t_visit.dr_code
	--INNER  JOIN 
	t_spr --ON t_spr.spr_initiator = t_visit.rep_id
	RIGHT OUTER JOIN t_sp ON dbo.t_spr.spr_id = dbo.t_sp.spr_id
	where t_spr.spr_initiator = @rep_id 
	and DATEPART(month,t_spr.spr_date_created) = @month and
	--DATEPART(month,t_visit.visit_date_plan) = @month AND 
	DATEPART(year,t_spr.spr_date_created) = @year;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_sp_doctor]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_sp_doctor]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT DISTINCT 
			t_sp_doctor.[spdr_id]
			,t_sp_doctor.[sp_id]
			,t_sp_doctor.[dr_code]
			,t_sp_doctor.[dr_as]
			,t_sp_doctor.[dr_plan]
			,t_sp_doctor.[dr_actual]
			,t_sp_doctor.[dr_date_realization]
	FROM t_spr --ON t_spr.spr_initiator = t_visit.rep_id
	RIGHT OUTER JOIN t_sp ON dbo.t_spr.spr_id = dbo.t_sp.spr_id
	RIGHT OUTER JOIN t_sp_doctor ON t_sp.sp_id = t_sp_doctor.sp_id
	where t_spr.spr_initiator = @rep_id
	and DATEPART(month,spr_date_created) = @month and
	DATEPART(year,t_spr.spr_date_created) = @year;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_sp_sponsor]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_sp_sponsor]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
		SELECT DISTINCT t_sp_sponsor.*
		FROM t_spr 
		RIGHT OUTER JOIN t_sp ON dbo.t_spr.spr_id = dbo.t_sp.spr_id
		RIGHT OUTER JOIN t_sp_doctor ON t_sp.sp_id = t_sp_doctor.sp_id
		RIGHT OUTER JOIN t_sp_sponsor ON t_sp_doctor.spdr_id = t_sp_sponsor.spdr_id
		where t_spr.spr_initiator = @rep_id
		and DATEPART(month,spr_date_created) = @month and
		DATEPART(year,t_spr.spr_date_created) = @year;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_spr]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_spr]
@rep_id char(50),
@month int,
@year int

AS
BEGIN

	SELECT DISTINCT t_spr.[spr_id]
      ,t_spr.[spr_no]
      ,t_spr.[spr_initiator]
      ,t_spr.[spr_status]
      ,t_spr.[spr_date_created]
      ,t_spr.[spr_date_updated]
      ,t_spr.[spr_note]
      ,t_spr.[e_name]
      ,t_spr.[e_topic]
      ,t_spr.[e_place]
      ,t_spr.[e_dt_start]
      ,t_spr.[e_dt_end]
      ,t_spr.[e_a_gp]
      ,t_spr.[e_a_gp_pax]
      ,t_spr.[e_a_specialist]
      ,t_spr.[e_a_specialist_pax]
      ,t_spr.[e_a_nurse]
      ,t_spr.[e_a_nurse_pax]
      ,t_spr.[e_a_others]
      ,t_spr.[e_a_others_pax]
      ,t_spr.[spr_allocation_key]
      ,t_spr.[input_origin]
	FROM t_spr
	where t_spr.spr_initiator = @rep_id  
	and DATEPART(month,spr_date_created) = @month and
	DATEPART(year,t_spr.spr_date_created) = @year;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_visit]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_visit]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT 
	   RTRIM([visit_id]) as visit_id
      ,RTRIM([rep_id]) as rep_id
      ,[visit_date_plan]
      ,[visit_plan]
      ,[visit_code]
      ,[visit_realization]
      ,[visit_info]
      ,[visit_sp]
      ,[visit_sp_value]
      ,[dr_code]
      ,[visit_plan_verification_status]
      ,RTRIM([visit_plan_verification_by]) as [visit_plan_verification_by]
      ,[visit_plan_verification_date]
      ,[visit_real_verification_status]
      ,RTRIM([visit_real_verification_by]) as [visit_real_verification_by]
      ,[visit_real_verification_date]
      ,[visit_date_plan_saved]
      ,[visit_date_plan_updated]
      ,[visit_date_realization_saved]
      ,[visit_coverage_plan]
      ,[visit_coverage_real]
      ,[visit_sign_id]
      ,[visit_sign_image]
      ,[visit_ordinat]
      ,[visit_product_count]
      ,[visit_comment]
	FROM t_visit
	WHERE rep_id = @rep_id AND
	MONTH(visit_date_plan) = @month AND 
	YEAR(visit_date_plan) = @year;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_visit_product]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_visit_product]
@rep_id char(50),
@month int,
@year int

AS
BEGIN

	SELECT DISTINCT 
	[vd_id]
	,vd_id as m_vd_id
	,RTRIM([visit_id]) as [visit_id]
	,RTRIM([visit_code]) as visit_code
	,[vd_value]
	,[vd_date_saved]
	,[sp_sp]
	,[sp_percentage]
	FROM t_visit_product
	WHERE visit_id in (
		SELECT DISTINCT RTRIM([visit_id]) as visit_id FROM t_visit
		WHERE rep_id = @rep_id AND
		MONTH(visit_date_plan) = @month AND 
		YEAR(visit_date_plan) = @year
	);
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_offline_t_visit_product_topic]    Script Date: 25/07/2019 21.17.31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE Procedure [dbo].[sp_offline_t_visit_product_topic]
@rep_id char(50),
@month int,
@year int

AS
BEGIN
	SELECT DISTINCT 
	[vpt_id]
	,[vpt_id] as m_vpt_id
      ,[vd_id]
	  ,[vd_id] as m_vd_id
      ,[topic_id]
      ,[vpt_feedback]
      ,[vpt_feedback_date]
      ,[note_feedback]
      ,[info_feedback_id]
	 FROM t_visit_product_topic
	WHERE vd_id in (
		SELECT DISTINCT vd_id FROM t_visit_product
		WHERE visit_id in (
			SELECT visit_id FROM t_visit
			WHERE rep_id = @rep_id AND
			MONTH(visit_date_plan) = @month AND 
			YEAR(visit_date_plan) = @year
		)
	);
END;

GO
