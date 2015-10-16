## Auth.Api

> Auth.Api is an OAuth2 and OpenId Connection service that uses IdentityServer3 with EntityFramework and AspNetMembership extensions.

### How to Build

```shell
$ npm run build                 # or, `npm run build -- release`
```

By default, it builds in a *debug* mode. If you need to build in a release
mode, just add `-- release` flag. This will optimize the output bundle for
production deployment.

### How to Run

```shell
$ npm start                     # or, `npm start -- release`
```

This will start a lightweight development server with "live reload" and
synchronized browsing across multiple devices and browsers.

### How to Deploy



### How to Test

> Look at [IdentityServer Client Samples](https://github.com/IdentityServer/IdentityServer3.Samples/tree/master/source/Clients)

### Migrations

#### Enabling

```shell
PM> Enable-Migrations -MigrationsDirectory Migrations\ClientConfiguration -ContextTypeName ClientConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
PM> Enable-Migrations -MigrationsDirectory Migrations\ScopeConfiguration -ContextTypeName ScopeConfigurationDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
PM> Enable-Migrations -MigrationsDirectory Migrations\OperationalConfiguration -ContextTypeName OperationalDbContext -ContextAssemblyName IdentityServer3.EntityFramework -ConnectionStringName IdSvr3Config
```

#### Adding

```shell
PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
PM> Add-Migration -Name InitialCreate -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config
```

#### Updating

```
PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.ClientConfiguration.Configuration -ConnectionStringName IdSvr3Config
PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.ScopeConfiguration.Configuration -ConnectionStringName IdSvr3Config
PM> Update-Database -ConfigurationTypeName Auth.Api.Migrations.OperationalConfiguration.Configuration -ConnectionStringName IdSvr3Config
```

### Specifications / Documentation

  * [OAuth 2](http://oauth.net/2/)
  * [OAuth 2 RFC](http://tools.ietf.org/html/rfc6749)
  * [OpenID Spec](http://openid.net/specs/openid-connect-core-1_0.html)

### Demonstrations / Videos
  
  * [Introduction to OpenID Connect, OAuth2 and IdentityServer](https://vimeo.com/113604459)

### Related Projects

  * [IdentityServer3](https://github.com/IdentityServer/IdentityServer3)
  * [IdentityServer3 Documentation](https://identityserver.github.io/Documentation/)
  * [IdentityServer3 Samples](https://github.com/IdentityServer/IdentityServer3.Samples/)
  * [IdentityManager](https://github.com/IdentityManager/IdentityManager)
  * [IdentityManager Wiki](https://github.com/IdentityManager/IdentityManager/wiki)
  
### Support

  * [IdentityServer Gitter](https://gitter.im/IdentityServer/IdentityServer3) on Gitter — Feedback, feature requests, Q&A
  * Email matthew.oliver@fnf.com