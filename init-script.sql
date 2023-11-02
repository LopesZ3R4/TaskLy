EXEC sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
EXEC sp_configure 'xp_cmdshell', 1;
GO
RECONFIGURE;
GO

PRINT 'Creating database...'
CREATE DATABASE bd;
GO

PRINT 'Setting database as current...'

USE bd;
GO

PRINT 'Creating admin user...'

CREATE LOGIN adm WITH PASSWORD = '@Admin1234';
GO

PRINT 'Granting sysadmin role to admin user...'

ALTER SERVER ROLE sysadmin ADD MEMBER adm;
GO

PRINT 'Creating Users table...'

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(128) NOT NULL,
    UserType NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Token NVARCHAR(255)
);
CREATE INDEX idx_username ON Users (Username);
CREATE INDEX idx_email ON Users (Email);

INSERT INTO Users (Username,Password,UserType,Email) values ('admin','34b9b7e38c513dd5b4001aa7b2f05f15444c7c520d5b851b28ef22e462811cc9','Admin','admin@sou.br');
go


PRINT 'Initialization script completed successfully.'