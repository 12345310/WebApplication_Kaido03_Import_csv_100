namespace WebApplication_Kaido03.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SexId = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        ParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parents", t => t.ParentId, cascadeDelete: true)
                .ForeignKey("dbo.Sexes", t => t.SexId)
                .Index(t => t.SexId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SexId = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sexes", t => t.SexId)
                .Index(t => t.SexId);
            
            CreateTable(
                "dbo.Sexes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Children", "SexId", "dbo.Sexes");
            DropForeignKey("dbo.Parents", "SexId", "dbo.Sexes");
            DropForeignKey("dbo.Children", "ParentId", "dbo.Parents");
            DropIndex("dbo.Parents", new[] { "SexId" });
            DropIndex("dbo.Children", new[] { "ParentId" });
            DropIndex("dbo.Children", new[] { "SexId" });
            DropTable("dbo.Sexes");
            DropTable("dbo.Parents");
            DropTable("dbo.Children");
        }
    }
}
