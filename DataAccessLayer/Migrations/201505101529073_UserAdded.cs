namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserExpenses", newName: "ExpenseUsers");
            RenameTable(name: "dbo.ApplicationUserGroups", newName: "GroupUsers");
            RenameColumn(table: "dbo.ExpenseUsers", name: "ApplicationUser_Id", newName: "User_UserId");
            RenameColumn(table: "dbo.GroupUsers", name: "ApplicationUser_Id", newName: "User_UserId");
            RenameIndex(table: "dbo.GroupUsers", name: "IX_ApplicationUser_Id", newName: "IX_User_UserId");
            RenameIndex(table: "dbo.ExpenseUsers", name: "IX_ApplicationUser_Id", newName: "IX_User_UserId");
            DropPrimaryKey("dbo.ExpenseUsers");
            DropPrimaryKey("dbo.GroupUsers");
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddPrimaryKey("dbo.ExpenseUsers", new[] { "Expense_ExpenseId", "User_UserId" });
            AddPrimaryKey("dbo.GroupUsers", new[] { "Group_GroupId", "User_UserId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.GroupUsers");
            DropPrimaryKey("dbo.ExpenseUsers");
            DropTable("dbo.Users");
            AddPrimaryKey("dbo.GroupUsers", new[] { "ApplicationUser_Id", "Group_GroupId" });
            AddPrimaryKey("dbo.ExpenseUsers", new[] { "ApplicationUser_Id", "Expense_ExpenseId" });
            RenameIndex(table: "dbo.ExpenseUsers", name: "IX_User_UserId", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.GroupUsers", name: "IX_User_UserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.GroupUsers", name: "User_UserId", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.ExpenseUsers", name: "User_UserId", newName: "ApplicationUser_Id");
            RenameTable(name: "dbo.GroupUsers", newName: "ApplicationUserGroups");
            RenameTable(name: "dbo.ExpenseUsers", newName: "ApplicationUserExpenses");
        }
    }
}
