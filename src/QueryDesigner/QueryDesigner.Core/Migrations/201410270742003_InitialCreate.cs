namespace QueryDesigner.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ConnectionString = c.String(),
                        User_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "User_ID", "dbo.Users");
            DropIndex("dbo.Connections", new[] { "User_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Connections");
        }
    }
}
