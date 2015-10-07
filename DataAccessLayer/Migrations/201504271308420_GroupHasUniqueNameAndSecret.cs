namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupHasUniqueNameAndSecret : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Secret", c => c.String(nullable: false));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Groups", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "Name" });
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Groups", "Secret");
        }
    }
}
