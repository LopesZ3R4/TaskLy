EXEC sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
EXEC sp_configure 'xp_cmdshell', 1;
GO
RECONFIGURE;
GO
EXEC sp_configure 'default language', 29;
go
RECONFIGURE;
go

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

CREATE TABLE Tasks
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500),
    StartDate DATETIME NOT NULL,
    Duration BigInt NOT NULL,
    AutoFinish bit NOT NULL,
    Finished bit NOT NULL,
    Owner NVARCHAR(50) not null,
    FinishDate AS DATEADD(minute, Duration, StartDate),
    FOREIGN KEY (Owner) REFERENCES Users (Username)
);

CREATE INDEX Idx_Tasks_Owner ON Tasks (Owner);
CREATE INDEX Idx_Tasks_Id ON Tasks (Id);

SET QUOTED_IDENTIFIER ON;
GO
CREATE INDEX Idx_FinishedDate ON Tasks (FinishDate);
GO

INSERT INTO Tasks (Title, Description, StartDate, Duration, AutoFinish, Finished, Owner)
VALUES ('Entrega do Projeto', 'Projeto de Aplicação fullstack sobre Tasks', '2023-11-07T07:30:00', 120, 1, 0, 'admin');

INSERT INTO Tasks (Title, Description, StartDate, Duration, AutoFinish, Finished, Owner)
VALUES ('Protótipo 1', 'Entrega do Primeiro Prototipo', '2023-11-02T10:30:00', 120, 1, 0, 'admin');

PRINT 'Initialization script completed successfully.'