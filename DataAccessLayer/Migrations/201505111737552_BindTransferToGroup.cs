namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BindTransferToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Transfers", "GroupId");
            AddForeignKey("dbo.Transfers", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "GroupId", "dbo.Groups");
            DropIndex("dbo.Transfers", new[] { "GroupId" });
            DropColumn("dbo.Transfers", "GroupId");
        }
    }
}
