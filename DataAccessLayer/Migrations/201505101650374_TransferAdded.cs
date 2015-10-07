namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransferAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        TransferId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransferId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "ApplicationUserId", "dbo.Users");
            DropIndex("dbo.Transfers", new[] { "ApplicationUserId" });
            DropTable("dbo.Transfers");
        }
    }
}
