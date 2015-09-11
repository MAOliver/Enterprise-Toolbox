namespace IdentityApi.Migrations.ClientConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "clients.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 200),
                        ClientName = c.String(nullable: false, maxLength: 200),
                        ClientUri = c.String(maxLength: 2000),
                        LogoUri = c.String(),
                        RequireConsent = c.Boolean(nullable: false),
                        AllowRememberConsent = c.Boolean(nullable: false),
                        Flow = c.Int(nullable: false),
                        AllowClientCredentialsOnly = c.Boolean(nullable: false),
                        AllowAccessToAllScopes = c.Boolean(nullable: false),
                        IdentityTokenLifetime = c.Int(nullable: false),
                        AccessTokenLifetime = c.Int(nullable: false),
                        AuthorizationCodeLifetime = c.Int(nullable: false),
                        AbsoluteRefreshTokenLifetime = c.Int(nullable: false),
                        SlidingRefreshTokenLifetime = c.Int(nullable: false),
                        RefreshTokenUsage = c.Int(nullable: false),
                        UpdateAccessTokenOnRefresh = c.Boolean(nullable: false),
                        RefreshTokenExpiration = c.Int(nullable: false),
                        AccessTokenType = c.Int(nullable: false),
                        EnableLocalLogin = c.Boolean(nullable: false),
                        IncludeJwtId = c.Boolean(nullable: false),
                        AlwaysSendClientClaims = c.Boolean(nullable: false),
                        PrefixClientClaims = c.Boolean(nullable: false),
                        AllowAccessToAllGrantTypes = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ClientId, unique: true);
            
            CreateTable(
                "clients.ClientCorsOrigins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Origin = c.String(nullable: false, maxLength: 150),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientCustomGrantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrantType = c.String(nullable: false, maxLength: 250),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Scope = c.String(nullable: false, maxLength: 200),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 250),
                        Type = c.String(maxLength: 250),
                        Description = c.String(maxLength: 2000),
                        Expiration = c.DateTimeOffset(precision: 7),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientIdPRestrictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Provider = c.String(nullable: false, maxLength: 200),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientPostLogoutRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false, maxLength: 2000),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "clients.ClientRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false, maxLength: 2000),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("clients.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("clients.ClientRedirectUris", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientPostLogoutRedirectUris", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientIdPRestrictions", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientSecrets", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientClaims", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientScopes", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientCustomGrantTypes", "Client_Id", "clients.Clients");
            DropForeignKey("clients.ClientCorsOrigins", "Client_Id", "clients.Clients");
            DropIndex("clients.ClientRedirectUris", new[] { "Client_Id" });
            DropIndex("clients.ClientPostLogoutRedirectUris", new[] { "Client_Id" });
            DropIndex("clients.ClientIdPRestrictions", new[] { "Client_Id" });
            DropIndex("clients.ClientSecrets", new[] { "Client_Id" });
            DropIndex("clients.ClientClaims", new[] { "Client_Id" });
            DropIndex("clients.ClientScopes", new[] { "Client_Id" });
            DropIndex("clients.ClientCustomGrantTypes", new[] { "Client_Id" });
            DropIndex("clients.ClientCorsOrigins", new[] { "Client_Id" });
            DropIndex("clients.Clients", new[] { "ClientId" });
            DropTable("clients.ClientRedirectUris");
            DropTable("clients.ClientPostLogoutRedirectUris");
            DropTable("clients.ClientIdPRestrictions");
            DropTable("clients.ClientSecrets");
            DropTable("clients.ClientClaims");
            DropTable("clients.ClientScopes");
            DropTable("clients.ClientCustomGrantTypes");
            DropTable("clients.ClientCorsOrigins");
            DropTable("clients.Clients");
        }
    }
}
