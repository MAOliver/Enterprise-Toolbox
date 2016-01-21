param(	
	[string]$env = "LOCAL",
	[string]$msbuild = $(throw "-msbuild is required.")
)

$migrate = "..\..\packages\EntityFramework.6.1.3\tools\Migrate.exe"
$connectionProvider = "System.Data.SqlClient"
$apiProj =  "..\..\source\apis\Auth.Api\Auth.Api.csproj"
$membershipProj = "..\..\source\apis\Membership.Api\Membership.Api.csproj"

if($env -eq 'LOCAL')
{
    $server = ".\SQLEXPRESS"
}

$connectionString = "Data Source=$server;Initial Catalog=Auth_$env;MultipleActiveResultSets=true;User ID=AuthAdmin_$env;Password=P@ssword123"

<#
.SYNOPSIS 
    Updates schema to latest version
.DESCRIPTION 
    Applies all code first migrations up to the latest version.
#>
Function MigrateToLatest()
{
	& $migrate Membership.Api Membership.Api.Migrations.MembershipConfiguration.Configuration /connectionString=$connectionString /connectionProviderName=$connectionProvider /startUpDirectory:"..\..\source\auth\apis\Membership.Api\bin" -Verbose
	& $migrate Auth.Api Auth.Api.Migrations.ClientConfiguration.Configuration /connectionString=$connectionString /connectionProviderName=$connectionProvider /startUpDirectory:"..\..\source\auth\apis\Auth.Api\bin" -Verbose
	& $migrate Auth.Api Auth.Api.Migrations.ScopeConfiguration.Configuration /connectionString=$connectionString /connectionProviderName=$connectionProvider /startUpDirectory:"..\..\source\auth\apis\Auth.Api\bin" -Verbose
	& $migrate Auth.Api Auth.Api.Migrations.OperationalConfiguration.Configuration /connectionString=$connectionString /connectionProviderName=$connectionProvider /startUpDirectory:"..\..\source\auth\apis\Auth.Api\bin" -Verbose
}

# Load Invoke-SqlCmd
if(Get-Module -ListAvailable | Where-Object { $_.name -eq 'sqlps' })
{
    Import-Module “sqlps” -DisableNameChecking
}
else
{
    add-pssnapin sqlservercmdletsnapin100
}

& $msbuild $apiProj
& $msbuild $membershipProj

Write-Warning 'Do you wish to drop / re-create the database?'
$continue = Read-Host 'Continue? (Y or N)'

if ($continue -eq 'y')
{
	$(Invoke-Sqlcmd -InputFile "AuthInstall.$env.sql" -ServerInstance "$server")
	
	MigrateToLatest
}
else 
{
	Write-Warning 'Do you wish to update to the latest schema?'
	$continue = Read-Host 'Continue? (Y or N)'

	if ($continue -eq 'y')
	{
		MigrateToLatest
	}
}


