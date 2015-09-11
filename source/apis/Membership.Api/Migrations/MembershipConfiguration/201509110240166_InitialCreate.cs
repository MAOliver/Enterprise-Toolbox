namespace Membership.Api.Migrations.MembershipConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "membership.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "membership.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("membership.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("membership.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "membership.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                "membership.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("membership.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "membership.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("membership.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("membership.AspNetUserRoles", "UserId", "membership.AspNetUsers");
            DropForeignKey("membership.AspNetUserLogins", "UserId", "membership.AspNetUsers");
            DropForeignKey("membership.AspNetUserClaims", "UserId", "membership.AspNetUsers");
            DropForeignKey("membership.AspNetUserRoles", "RoleId", "membership.AspNetRoles");
            DropIndex("membership.AspNetUserLogins", new[] { "UserId" });
            DropIndex("membership.AspNetUserClaims", new[] { "UserId" });
            DropIndex("membership.AspNetUsers", "UserNameIndex");
            DropIndex("membership.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("membership.AspNetUserRoles", new[] { "UserId" });
            DropIndex("membership.AspNetRoles", "RoleNameIndex");
            DropTable("membership.AspNetUserLogins");
            DropTable("membership.AspNetUserClaims");
            DropTable("membership.AspNetUsers");
            DropTable("membership.AspNetUserRoles");
            DropTable("membership.AspNetRoles");
        }
    }
}
