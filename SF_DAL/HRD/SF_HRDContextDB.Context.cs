﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hrdEntities : DbContext
    {
        public hrdEntities()
            : base("name=hrdEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<All_Karyawan> All_Karyawan { get; set; }
        public virtual DbSet<tuser> tusers { get; set; }
        public virtual DbSet<Bagian> Bagians { get; set; }
        public virtual DbSet<Departeman> Departemen { get; set; }
        public virtual DbSet<HeadQuarter> HeadQuarters { get; set; }
        public virtual DbSet<Jabatan> Jabatans { get; set; }
        public virtual DbSet<Karyawan> Karyawans { get; set; }
    }
}