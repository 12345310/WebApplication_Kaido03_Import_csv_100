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
new Sex { Name = "íj" },
new Sex { Name = "èó" }
};
            sexes.ForEach(s => context.Sexes.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var parents = new List<Parent>
{
new Parent { Name = "ê¬ñÿìƒéu", SexId = sexes.Single(s => s.Name == "íj").Id, Email = "aoki.atsushi@example.com" },
new Parent { Name = "à‰è„àËïv", SexId = sexes.Single(s => s.Name == "íj").Id, Email = "inoue.ikuo@example.com", Children = new List<Child>
{
new Child { Name = "à‰è„åbî¸éq", SexId = sexes.Single(s => s.Name == "èó").Id, Birthday = DateTime.Parse("2015-01-16") }
}
},
new Parent { Name = "âFç≤î¸åiéq", SexId = sexes.Single(s => s.Name == "èó").Id, Email = "usami.keiko@example.com", Children = new List<Child>
{
new Child { Name = "âFç≤î¸ó¡âÓ", SexId = sexes.Single(s => s.Name == "íj").Id, Birthday = DateTime.Parse("2010-02-01") },
new Child { Name = "âFç≤î¸êMâÓ", SexId = sexes.Single(s => s.Name == "íj").Id, Birthday = DateTime.Parse("2013-11-08") },
new Child { Name = "âFç≤î¸èÉ", SexId = sexes.Single(s => s.Name == "íj").Id, Birthday = DateTime.Parse("2015-04-05") }
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
