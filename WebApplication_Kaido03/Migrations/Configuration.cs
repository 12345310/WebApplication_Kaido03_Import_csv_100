namespace WebApplication_Kaido03.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication_Kaido03.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication_Kaido03.Models.WebApplication_Kaido03Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication_Kaido03.Models.WebApplication_Kaido03Context context)
        {
            //  This method will be called after migrating to the latest version.

            var sexes = new List<Sex>
{
new Sex { Name = "�j" },
new Sex { Name = "��" }
};
            sexes.ForEach(s => context.Sexes.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var parents = new List<Parent>
{
new Parent { Name = "�ؓĎu", SexId = sexes.Single(s => s.Name == "�j").Id, Email = "aoki.atsushi@example.com" },
new Parent { Name = "����v", SexId = sexes.Single(s => s.Name == "�j").Id, Email = "inoue.ikuo@example.com", Children = new List<Child>
{
new Child { Name = "���b���q", SexId = sexes.Single(s => s.Name == "��").Id, Birthday = DateTime.Parse("2015-01-16") }
}
},
new Parent { Name = "�F�����i�q", SexId = sexes.Single(s => s.Name == "��").Id, Email = "usami.keiko@example.com", Children = new List<Child>
{
new Child { Name = "�F��������", SexId = sexes.Single(s => s.Name == "�j").Id, Birthday = DateTime.Parse("2010-02-01") },
new Child { Name = "�F�����M��", SexId = sexes.Single(s => s.Name == "�j").Id, Birthday = DateTime.Parse("2013-11-08") },
new Child { Name = "�F������", SexId = sexes.Single(s => s.Name == "�j").Id, Birthday = DateTime.Parse("2015-04-05") }
}
}
};
            parents.ForEach(s => context.Parents.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
