# Overview
IdentityManager AspNetMembership Core components

## Enabling
Enable-Migrations -MigrationsDirectory Migrations\MembershipConfiguration -ContextTypeName MembershipContext -ContextAssemblyName Identity.Core -ConnectionStringName IdSvr3Config

## Adding
Add-Migration -Name InitialCreate -ConfigurationTypeName Membership.Api.Migrations.MembershipConfiguration.Configuration -ConnectionStringName IdSvr3Config

## Updating
Update-Database -ConfigurationTypeName Membership.Api.Migrations.MembershipConfiguration.Configuration -ConnectionStringName IdSvr3Config