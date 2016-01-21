# Overview
IdentityServer3 AspNetMembership Core components

## Enabling
Enable-Migrations -MigrationsDirectory Migrations\IdentityConfiguration -ContextTypeName IdentityContext -ContextAssemblyName Identity.Core -ConnectionStringName IdSvr3Config

## Adding
Add-Migration -Name InitialCreate -ConfigurationTypeName IdentityApi.Migrations.IdentityConfiguration.Configuration -ConnectionStringName IdSvr3Config

## Updating
Update-Database -ConfigurationTypeName IdentityApi.Migrations.IdentityConfiguration.Configuration -ConnectionStringName IdSvr3Config