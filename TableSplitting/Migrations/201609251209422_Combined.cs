namespace TableSplitting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Combined : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessProject",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HasDiscount = c.Boolean(nullable: false),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateAndTimeCreated = c.DateTime(nullable: false),
                        DateAndTimeLastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsumerProjects",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessProject", "Id", "dbo.Projects");
            DropForeignKey("dbo.ConsumerProjects", "Id", "dbo.Projects");
            DropIndex("dbo.ConsumerProjects", new[] { "Id" });
            DropIndex("dbo.BusinessProject", new[] { "Id" });
            DropTable("dbo.ConsumerProjects");
            DropTable("dbo.Projects");
            DropTable("dbo.BusinessProject");
        }
    }
}
