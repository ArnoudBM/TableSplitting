namespace TableSplitting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneToZeroOrOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonnelNumber = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Laptops",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Laptops", "Id", "dbo.Employees");
            DropIndex("dbo.Laptops", new[] { "Id" });
            DropTable("dbo.Laptops");
            DropTable("dbo.Employees");
        }
    }
}
