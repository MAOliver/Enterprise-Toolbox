WindowsAuthentication.Api
=========================

> WindowsAuthentication.Api is a service that proxies windows credentials into an ADFS endpoint to be leveraged 
> as an ADFS token source by the Auth.API

How to Run
----------

WindowsAuthentication.Api can be run independently of any other project.

However, it is best to run the setup steps in Auth.API in order to configure the STS token in your token store.

Additionally, if you are running under IIS, you'll need to give "read" permissions to the IIS AppPool\(winad pool name):

	1. Create an app pool (named e.g. "auth")
	2. Publish WindowsAuthentication.API to "auth"
	3. Open mmc.exe
	4. File -> Add/Remove Snap-In -> Certificates -> Local Computer
	5. Expand Personal -> Certificates
	6. Right click on "sts", All Tasks -> Manage Private Keys
	7. Add "IIS AppPool\auth", read permission
	8. Ok / Ok / Ok...

How to Deploy
-------------

TBD

How to Test
-----------

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

	.. code-block::ps
		
		PM> Enable-Migrations -MigrationsDirectory Migrations\MembershipConfiguration -ContextTypeName MembershipContext -ContextAssemblyName Identity.Core -ConnectionStringName IdSvr3Config

Adding
^^^^^^

	.. code-block::ps

		PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Membership.Api.Migrations.MembershipConfiguration.Configuration -ConnectionStringName IdSvr3Config


Updating
^^^^^^^^

	.. code-block::ps

		PM> Update-Database -ConfigurationTypeName Membership.Api.Migrations.MembershipConfiguration.Configuration -ConnectionStringName IdSvr3Config

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

  * [IdentityManager](https://github.com/IdentityManager/IdentityManager)
  * [IdentityManager Wiki](https://github.com/IdentityManager/IdentityManager/wiki)
  
Support
-------

  * Email matthewaustinoliver@gmail.com