USE [master]
GO

IF NOT EXISTS (SELECT * FROM [syslogins] WHERE [loginname] = N'AuthAdmin_LOCAL')
BEGIN
	CREATE LOGIN [AuthAdmin_LOCAL]
		WITH PASSWORD=N'P@ssword123',
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF;
END

IF NOT EXISTS (SELECT * FROM [syslogins] WHERE [loginname] = N'AuthService_LOCAL')
BEGIN
	CREATE LOGIN [AuthService_LOCAL]
		WITH PASSWORD=N'P@ssword123',
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF;
END

USE [master]
go

DECLARE @dbname nvarchar(128)
SET @dbname = N'Auth_LOCAL'

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
BEGIN
	ALTER DATABASE [Auth_LOCAL] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [Auth_LOCAL];
END

CREATE DATABASE [Auth_LOCAL] ON  PRIMARY 
( NAME = N'Auth', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Auth_LOCAL.mdf', MAXSIZE = UNLIMITED )
 LOG ON 
( NAME = N'Auth_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Auth_LOCAL_log.ldf', MAXSIZE = 2048GB )
GO

USE [Auth_LOCAL]
GO

CREATE USER [AuthService_LOCAL] FOR LOGIN [AuthService_LOCAL] WITH DEFAULT_SCHEMA=[dbo]
GO

GRANT CONNECT TO [AuthService_LOCAL] AS [dbo]
GO

ALTER DATABASE [Auth_LOCAL] SET  READ_WRITE 
GO

EXEC sp_changedbowner N'AuthAdmin_LOCAL';
GO

EXEC sp_addrolemember N'db_datawriter', N'AuthService_LOCAL'
GO
EXEC sp_addrolemember N'db_datareader', N'AuthService_LOCAL'
GO