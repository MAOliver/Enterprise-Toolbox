namespace IdentityApi.Migrations.ScopeConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "scopes.Scopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        DisplayName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        Required = c.Boolean(nullable: false),
                        Emphasize = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        IncludeAllClaimsForUser = c.Boolean(nullable: false),
                        ClaimsRule = c.String(maxLength: 200),
                        ShowInDiscoveryDocument = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "scopes.ScopeClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        AlwaysIncludeInIdToken = c.Boolean(nullable: false),
                        Scope_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("scopes.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .Index(t => t.Scope_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("scopes.ScopeClaims", "Scope_Id", "scopes.Scopes");
            DropIndex("scopes.ScopeClaims", new[] { "Scope_Id" });
            DropTable("scopes.ScopeClaims");
            DropTable("scopes.Scopes");
        }
    }
}
