namespace Auth.Api.Migrations.ClientConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "LogoutUri", c => c.String());
            AddColumn("dbo.Clients", "LogoutSessionRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "LogoutSessionRequired");
            DropColumn("dbo.Clients", "LogoutUri");
        }
    }
}
