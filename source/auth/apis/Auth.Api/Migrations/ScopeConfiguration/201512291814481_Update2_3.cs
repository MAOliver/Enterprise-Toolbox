namespace Auth.Api.Migrations.ScopeConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2_3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScopeSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        Expiration = c.DateTimeOffset(precision: 7),
                        Type = c.String(maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        Scope_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .Index(t => t.Scope_Id);
            
            AddColumn("dbo.Scopes", "AllowUnrestrictedIntrospection", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScopeSecrets", "Scope_Id", "dbo.Scopes");
            DropIndex("dbo.ScopeSecrets", new[] { "Scope_Id" });
            DropColumn("dbo.Scopes", "AllowUnrestrictedIntrospection");
            DropTable("dbo.ScopeSecrets");
        }
    }
}
