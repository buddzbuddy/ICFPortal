namespace CISSAPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Companies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        AspNetUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId)
                .Index(t => t.AspNetUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "AspNetUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Companies", new[] { "AspNetUserId" });
            DropTable("dbo.Companies");
        }
    }
}
