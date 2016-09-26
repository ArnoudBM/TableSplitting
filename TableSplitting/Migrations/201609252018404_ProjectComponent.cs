namespace TableSplitting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectComponent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(),
                        ManufacturerCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessProject", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectComponents", "ProjectId", "dbo.BusinessProject");
            DropIndex("dbo.ProjectComponents", new[] { "ProjectId" });
            DropTable("dbo.ProjectComponents");
        }
    }
}
