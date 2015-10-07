namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReturnToApplicationUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExpenseUsers", newName: "ApplicationUserExpenses");
            RenameTable(name: "dbo.GroupUsers", newName: "ApplicationUserGroups");
            DropForeignKey("dbo.Transfers", "UserId", "dbo.Users");
            DropIndex("dbo.Transfers", new[] { "UserId" });
            RenameColumn(table: "dbo.ApplicationUserExpenses", name: "User_UserId", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.ApplicationUserGroups", name: "User_UserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.ApplicationUserExpenses", name: "IX_User_UserId", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.ApplicationUserGroups", name: "IX_User_UserId", newName: "IX_ApplicationUser_Id");
            DropPrimaryKey("dbo.ApplicationUserExpenses");
            DropPrimaryKey("dbo.ApplicationUserGroups");
            AddColumn("dbo.Transfers", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ApplicationUserExpenses", new[] { "ApplicationUser_Id", "Expense_ExpenseId" });
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "ApplicationUser_Id", "Group_GroupId" });
            CreateIndex("dbo.Transfers", "ApplicationUserId");
            AddForeignKey("dbo.Transfers", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Transfers", "UserId");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Transfers", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Transfers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Transfers", new[] { "ApplicationUserId" });
            DropPrimaryKey("dbo.ApplicationUserGroups");
            DropPrimaryKey("dbo.ApplicationUserExpenses");
            DropColumn("dbo.Transfers", "ApplicationUserId");
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "Group_GroupId", "User_UserId" });
            AddPrimaryKey("dbo.ApplicationUserExpenses", new[] { "Expense_ExpenseId", "User_UserId" });
            RenameIndex(table: "dbo.ApplicationUserGroups", name: "IX_ApplicationUser_Id", newName: "IX_User_UserId");
            RenameIndex(table: "dbo.ApplicationUserExpenses", name: "IX_ApplicationUser_Id", newName: "IX_User_UserId");
            RenameColumn(table: "dbo.ApplicationUserGroups", name: "ApplicationUser_Id", newName: "User_UserId");
            RenameColumn(table: "dbo.ApplicationUserExpenses", name: "ApplicationUser_Id", newName: "User_UserId");
            CreateIndex("dbo.Transfers", "UserId");
            AddForeignKey("dbo.Transfers", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            RenameTable(name: "dbo.ApplicationUserGroups", newName: "GroupUsers");
            RenameTable(name: "dbo.ApplicationUserExpenses", newName: "ExpenseUsers");
        }
    }
}
