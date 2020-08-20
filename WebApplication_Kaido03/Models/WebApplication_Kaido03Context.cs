using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication_Kaido03.Models
{
    public class WebApplication_Kaido03Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WebApplication_Kaido03Context() : base("name=WebApplication_Kaido03Context")
        {
        }

        public System.Data.Entity.DbSet<WebApplication_Kaido03.Models.Sex> Sexes { get; set; }

        public System.Data.Entity.DbSet<WebApplication_Kaido03.Models.Child> Children { get; set; }

        public System.Data.Entity.DbSet<WebApplication_Kaido03.Models.Parent> Parents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parent>()
                .HasRequired(c => c.Sex)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Child>()
                .HasRequired(c => c.Sex)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
