﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DialogPortal.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DialogConfigBLEntities : DbContext
    {
        public DialogConfigBLEntities()
            : base("name=DialogConfigBLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Datenbank> Datenbanks { get; set; }
        public virtual DbSet<Mandant> Mandants { get; set; }
        public virtual DbSet<UserMapping> UserMappings { get; set; }
        public virtual DbSet<Antrag> Antrags { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MIDRequest> MIDRequests { get; set; }
    }
}
