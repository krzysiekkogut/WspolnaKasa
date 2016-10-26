namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        UserPayingId = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserPayingId)
                .Index(t => t.UserPayingId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Secret = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        TransferId = c.Int(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransferId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserExpenses",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Expense_ExpenseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Expense_ExpenseId })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Expenses", t => t.Expense_ExpenseId, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Expense_ExpenseId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_GroupId })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Transfers", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transfers", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transfers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroups", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserExpenses", "Expense_ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.UserExpenses", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenses", "UserPayingId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenses", "GroupId", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "Group_GroupId" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.UserExpenses", new[] { "Expense_ExpenseId" });
            DropIndex("dbo.UserExpenses", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Transfers", new[] { "GroupId" });
            DropIndex("dbo.Transfers", new[] { "ReceiverId" });
            DropIndex("dbo.Transfers", new[] { "SenderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Expenses", new[] { "GroupId" });
            DropIndex("dbo.Expenses", new[] { "UserPayingId" });
            DropTable("dbo.UserGroups");
            DropTable("dbo.UserExpenses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Transfers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Expenses");
        }
    }
}
