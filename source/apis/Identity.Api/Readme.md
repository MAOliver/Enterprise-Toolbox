# Overview
IdentityServer3 w/ AspNetMembership and EntityFramework

## Enabling
Enable-Migrations -MigrationsDirectory Migrations\ClientConfiguration -ContextTypeName ClientConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
Enable-Migrations -MigrationsDirectory Migrations\ScopeConfiguration -ContextTypeName ScopeConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
Enable-Migrations -MigrationsDirectory Migrations\OperationalConfiguration -ContextTypeName OperationalDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config

## Adding
Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config

## Updating
Update-Database -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
Update-Database -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
Update-Database -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config
