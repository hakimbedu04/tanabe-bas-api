﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SF_DAL.Prod
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProdEntities : DbContext
    {
        public ProdEntities()
            : base("name=ProdEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aplikai_User> Aplikai_User { get; set; }
        public virtual DbSet<tUser> tUsers { get; set; }
        public virtual DbSet<All_Karyawan> All_Karyawan { get; set; }
    }
}