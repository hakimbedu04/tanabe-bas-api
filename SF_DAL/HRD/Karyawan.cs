//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SF_DAL.HRD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Karyawan
    {
        public string Nomor_Induk { get; set; }
        public string Nama { get; set; }
        public string Title { get; set; }
        public string TitelBelakang { get; set; }
        public string Nomor_Astek { get; set; }
        public string Jenis_Kelamin { get; set; }
        public string Tempat_Lahir { get; set; }
        public Nullable<System.DateTime> Tanggal_Lahir { get; set; }
        public string Alamat { get; set; }
        public string Alamat2 { get; set; }
        public string Nomor_Telepon { get; set; }
        public string Agama { get; set; }
        public string Pendidikan { get; set; }
        public Nullable<System.DateTime> Tanggal_Kerja { get; set; }
        public Nullable<System.DateTime> Tanggal_Diangkat { get; set; }
        public Nullable<System.DateTime> Tanggal_Keluar { get; set; }
        public string Kode_Jabatan { get; set; }
        public string Kode_Departemen { get; set; }
        public string Nama_Seksi { get; set; }
        public string Nama_Cabang { get; set; }
        public string Status { get; set; }
        public string Nama_Pasangan { get; set; }
        public string Foto { get; set; }
        public string Nama_Ibu { get; set; }
        public string Nama_Bapak { get; set; }
        public string Nomor_KTP { get; set; }
        public string Alasan_Keluar { get; set; }
        public string Status_Kerja { get; set; }
        public string Tinggi_Badan { get; set; }
        public string Berat_Badan { get; set; }
        public string Golongan_Darah { get; set; }
        public string Email { get; set; }
        public string Tingkat_Pendidikan { get; set; }
        public string Nomor_Urut { get; set; }
        public string Kategori { get; set; }
        public string Jabatan_k3lh { get; set; }
        public string Bagian_k3lh { get; set; }
        public string Unit_k3lh { get; set; }
        public string Kode_Bagian { get; set; }
        public Nullable<System.DateTime> Tanggal_Verifikasi { get; set; }
        public Nullable<System.DateTime> Tgl_Sertifikasi_MR { get; set; }
        public string No_Sertifikat_MR { get; set; }
        public string UserID { get; set; }
        public string Pwd { get; set; }
        public Nullable<short> Aktif { get; set; }
        public string NIK_Atasan { get; set; }
        public string No_Peserta_DPLK { get; set; }
        public string No_Kartu_DPLK { get; set; }
        public Nullable<System.DateTime> Awal_Kepesertaan { get; set; }
        public Nullable<System.DateTime> Akhir_Kepesertaan { get; set; }
        public string Keterangan { get; set; }
        public string Nama_DPLK { get; set; }
        public string NPWP { get; set; }
        public string ANGGOTA_SP { get; set; }
        public Nullable<decimal> IURAN_SP { get; set; }
        public Nullable<int> TUNJANGAN_ISTRI { get; set; }
        public Nullable<int> TUNJANGAN_ANAK { get; set; }
        public Nullable<int> S_OR_M { get; set; }
        public Nullable<int> JML_ANAK_PPH { get; set; }
        public Nullable<decimal> STRATEGIC_ALLOWANCE { get; set; }
        public Nullable<int> FASILITAS_KENDARAAN { get; set; }
        public Nullable<int> Attandance { get; set; }
        public string PIN { get; set; }
        public string Warga_Negara { get; set; }
        public string No_Handphone { get; set; }
        public string Status_Tempat_Tinggal { get; set; }
        public string Alamat_Darurat { get; set; }
        public string No_Telepon_Darurat { get; set; }
        public string No_Handphone_Darurat { get; set; }
        public string Nama_Yang_Dihubungi { get; set; }
        public string Hubungan_Keluarga { get; set; }
        public string Status_Pernikahan { get; set; }
        public string No_Identitas_Diri { get; set; }
        public Nullable<System.DateTime> Masa_Berlaku_Identitas_Diri { get; set; }
        public string Tempat_Lahir_Bapak { get; set; }
        public string Tempat_Lahir_Ibu { get; set; }
        public Nullable<System.DateTime> Tanggal_Lahir_Bapak { get; set; }
        public Nullable<System.DateTime> Tanggal_Lahir_Ibu { get; set; }
        public string Alamat_Bapak { get; set; }
        public string Alamat_Ibu { get; set; }
        public string Pekerjaan_Bapak { get; set; }
        public string Pekerjaan_Ibu { get; set; }
        public string Jabatan_Penugasan { get; set; }
        public Nullable<int> Status_Aktif { get; set; }
        public string Alasan_Non_Aktif { get; set; }
        public string No_Ref_Job_Desc { get; set; }
        public Nullable<int> Revisi_Job_Desc_Ke { get; set; }
        public string Tgl_Berlaku_Job_Desc { get; set; }
        public string Nama_File_Job_Desc { get; set; }
        public Nullable<int> Fasilitas { get; set; }
        public string Alasan_Keluar2 { get; set; }
        public string Kode_Divisi { get; set; }
        public string Email2 { get; set; }
        public string ProfilePicture { get; set; }
    
        public virtual Departeman Departeman { get; set; }
    }
}