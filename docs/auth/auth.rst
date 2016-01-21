Auth.Api
========

> Auth.Api is an OAuth2 and OpenId Connection service that uses IdentityServer3 with EntityFramework and AspNetMembership extensions.

Setup
-----

To create the database
^^^^^^^^^^^^^^^^^^^^^^

From a powershell console under $(solution)\build\scripts\AuthDb folder:

	.. code-block::ps
		
		$ .\CreateDatabase.ps1

NOTE: This runs against the default instance of SQLExpress 2014 (i.e. `.\SQLEXPRESS`).
If you are using a different version of SQLExpresss, you will have to modify `AuthInstall.LOCAL.sql`.
If you are using something other than SQLExpresss, you will have to modify `AuthInstall.LOCAL.sql and CreateDatabase.ps1`.

Creating the Self-Signed Certs
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

From a powershell console under $(solution)\build\scripts\AuthWeb folder:

	.. code-block::ps
		
		$ .\Setup.ps1

This script will create self-signed certs to use for signing tokens and for
the site to run in SSL in the self-hosted project.


Running the Application
-----------------------

After creating the database, the cert, and configuring your netsh as above -- right click "Auth.SelfHost" and run.

The application is working correctly if:

    1. There are no stacktraces in the console window on startup
	2. You can login to https://localhost:44333/auth/idm with the credentials (u: `admin@example.com` / p: `P@ssw0rd` )

Deploying the Application
-------------------------

TBD

Testing the Application
-----------------------

Using the sample MVC App:
^^^^^^^^^^^^^^^^^^^^^^^^^

Open:

> $\...\Auth\Main\Identity.Samples.sln

Debug MvcCodeFlowClientManual

Using Postman
^^^^^^^^^^^^^

Other
^^^^^

> Look at [IdentityServer Client Samples](https://github.com/IdentityServer/IdentityServer3.Samples/tree/master/source/Clients)

Migrations
----------

Unless modifying the schema -- just use CreateDatabase.ps1 under the scripts folder.

Enabling
^^^^^^^^

This only needs to be executed at the start of the project.

	.. code-block::ps
		
		PM> Enable-Migrations -MigrationsDirectory Migrations\ClientConfiguration -ContextTypeName ClientConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
		PM> Enable-Migrations -MigrationsDirectory Migrations\ScopeConfiguration -ContextTypeName ScopeConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
		PM> Enable-Migrations -MigrationsDirectory Migrations\OperationalConfiguration -ContextTypeName OperationalDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config

Adding
^^^^^^

This should be executed if IdentityServer3 has a major update that changes the schema.

	.. code-block::ps

		PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
		PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
		PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config


Updating
^^^^^^^^

This should be executed after executing Add-Migration above.

	.. code-block::ps

		PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
		PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
		PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config

Specifications / Documentation
------------------------------

  * [OAuth 2](http://oauth.net/2/)
  * [OAuth 2 RFC](http://tools.ietf.org/html/rfc6749)
  * [OpenID Spec](http://openid.net/specs/openid-connect-core-1_0.html)

Demonstrations / Videos
-----------------------
  
  * [Introduction to OpenID Connect, OAuth2 and IdentityServer](https://vimeo.com/113604459)

Related Projects
----------------

  * [IdentityServer3](https://github.com/IdentityServer/IdentityServer3)
  * [IdentityServer3 Documentation](https://identityserver.github.io/Documentation/)
  * [IdentityServer3 Samples](https://github.com/IdentityServer/IdentityServer3.Samples/)
  * [IdentityManager](https://github.com/IdentityManager/IdentityManager)
  * [IdentityManager Wiki](https://github.com/IdentityManager/IdentityManager/wiki)
  
Support
-------

  * [IdentityServer Gitter](https://gitter.im/IdentityServer/IdentityServer3) on Gitter — Feedback, feature requests, Q&A
  * Email matthewaustinoliver@gmail.com