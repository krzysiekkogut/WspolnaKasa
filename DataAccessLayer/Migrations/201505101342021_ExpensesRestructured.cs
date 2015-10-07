namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpensesRestructured : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.TransactionApplicationUsers", "Transaction_TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.TransactionApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Transactions", new[] { "GroupId" });
            DropIndex("dbo.TransactionApplicationUsers", new[] { "Transaction_TransactionId" });
            DropIndex("dbo.TransactionApplicationUsers", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        UserPayingId = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ExpenseApplicationUsers",
                c => new
                    {
                        Expense_ExpenseId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Expense_ExpenseId, t.ApplicationUser_Id })
                .ForeignKey("dbo.Expenses", t => t.Expense_ExpenseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Expense_ExpenseId)
                .Index(t => t.ApplicationUser_Id);
            
            DropTable("dbo.Transactions");
            DropTable("dbo.TransactionApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransactionApplicationUsers",
                c => new
                    {
                        Transaction_TransactionId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Transaction_TransactionId, t.ApplicationUser_Id });
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        GroupId = c.Int(nullable: false),
                        UserPayingId = c.String(),
                    })
                .PrimaryKey(t => t.TransactionId);
            
            DropForeignKey("dbo.ExpenseApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExpenseApplicationUsers", "Expense_ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.Expenses", "GroupId", "dbo.Groups");
            DropIndex("dbo.ExpenseApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ExpenseApplicationUsers", new[] { "Expense_ExpenseId" });
            DropIndex("dbo.Expenses", new[] { "GroupId" });
            DropTable("dbo.ExpenseApplicationUsers");
            DropTable("dbo.Expenses");
            CreateIndex("dbo.TransactionApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.TransactionApplicationUsers", "Transaction_TransactionId");
            CreateIndex("dbo.Transactions", "GroupId");
            AddForeignKey("dbo.TransactionApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TransactionApplicationUsers", "Transaction_TransactionId", "dbo.Transactions", "TransactionId", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
        }
    }
}
