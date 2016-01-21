Membership.Api
==============

> Membership.Api is a User and User Claims management Web Interface. It is dependent on Auth.Api for security.

How to Run
----------

Membership.Api is dependent on Auth.Api. The best way to run the application is to execute the setup steps
indicated in authsetup_ and then run the Auth.SelfHost project.

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