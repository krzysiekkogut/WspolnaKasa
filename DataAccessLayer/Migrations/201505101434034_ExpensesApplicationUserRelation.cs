namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpensesApplicationUserRelation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExpenseApplicationUsers", newName: "ApplicationUserExpenses");
            DropPrimaryKey("dbo.ApplicationUserExpenses");
            AddPrimaryKey("dbo.ApplicationUserExpenses", new[] { "ApplicationUser_Id", "Expense_ExpenseId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApplicationUserExpenses");
            AddPrimaryKey("dbo.ApplicationUserExpenses", new[] { "Expense_ExpenseId", "ApplicationUser_Id" });
            RenameTable(name: "dbo.ApplicationUserExpenses", newName: "ExpenseApplicationUsers");
        }
    }
}
