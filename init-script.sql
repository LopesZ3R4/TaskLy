EXEC sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
EXEC sp_configure 'xp_cmdshell', 1;
GO
RECONFIGURE;
GO
/*
EXEC sp_configure 'default language', 29;
go
RECONFIGURE;
go
*/

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

PRINT 'Creating County table...'
CREATE TABLE County
(
    CountyCode NVARCHAR(5) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
CREATE INDEX idx_County_countycode ON County (CountyCode);
INSERT INTO County (CountyCode, Name)
VALUES
('BR-AC', 'Acre'),
('BR-AL', 'Alagoas'),
('BR-AP', 'Amapá'),
('BR-AM', 'Amazonas'),
('BR-BA', 'Bahia'),
('BR-CE', 'Ceará'),
('BR-DF', 'Distrito Federal'),
('BR-ES', 'Espírito Santo'),
('BR-GO', 'Goiás'),
('BR-MA', 'Maranhão'),
('BR-MT', 'Mato Grosso'),
('BR-MS', 'Mato Grosso do Sul'),
('BR-MG', 'Minas Gerais'),
('BR-PA', 'Pará'),
('BR-PB', 'Paraíba'),
('BR-PR', 'Paraná'),
('BR-PE', 'Pernambuco'),
('BR-PI', 'Piauí'),
('BR-RJ', 'Rio de Janeiro'),
('BR-RN', 'Rio Grande do Norte'),
('BR-RS', 'Rio Grande do Sul'),
('BR-RO', 'Rondônia'),
('BR-RR', 'Roraima'),
('BR-SC', 'Santa Catarina'),
('BR-SP', 'São Paulo'),
('BR-SE', 'Sergipe'),
('BR-TO', 'Tocantins');

PRINT 'Creating Users table...'

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(128) NOT NULL,
    UserType NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Token NVARCHAR(255),
    CountyCode NVARCHAR(5) NOT NULL,
    FOREIGN KEY (CountyCode) REFERENCES County (CountyCode)
);
CREATE INDEX idx_username ON Users (Username);
CREATE INDEX idx_email ON Users (Email);

INSERT INTO Users (Username,Password,UserType,Email,CountyCode) values ('admin','34b9b7e38c513dd5b4001aa7b2f05f15444c7c520d5b851b28ef22e462811cc9','Admin','admin@sou.br','BR-MT');
INSERT INTO Users (Username,Password,UserType,Email,CountyCode) values ('joao.moreno','34b9b7e38c513dd5b4001aa7b2f05f15444c7c520d5b851b28ef22e462811cc9','Admin','joao@sou.br','BR-SP');

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
VALUES ('Protótipo 1', 'Entrega do Primeiro Prototipo', '2023-11-02T10:30:00', 120, 1, 0, 'joao.moreno');

CREATE TABLE Holidays
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE NOT NULL,
    LocalName NVARCHAR(255) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    CountryCode CHAR(2) NOT NULL,
    CountyCode NVARCHAR(5),
    Fixed BIT NOT NULL,
    Global BIT NOT NULL,
    LaunchYear INT
    FOREIGN KEY (CountyCode) REFERENCES County (CountyCode)
);

CREATE INDEX idx_holidays_date ON Holidays (Date);

CREATE INDEX idx_holiday
ON Holidays (Date, CountryCode, CountyCode);

CREATE INDEX idx_holiday_full
ON Holidays (Date, Name, CountryCode, CountyCode);

CREATE TABLE Types
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL
);

CREATE TABLE HolidayTypes
(
    HolidayId INT NOT NULL,
    TypeId INT NOT NULL,
    PRIMARY KEY (HolidayId, TypeId),
    FOREIGN KEY (HolidayId) REFERENCES Holidays(Id),
    FOREIGN KEY (TypeId) REFERENCES Types(Id)
);

CREATE INDEX idx_types_typename ON Types (TypeName);

CREATE INDEX idx_holidaytypes_holidayid ON HolidayTypes (HolidayId);
CREATE INDEX idx_holidaytypes_typeid ON HolidayTypes (TypeId);

INSERT INTO Types (TypeName)
VALUES
('Public'),
('Bank'),
('School'),
('Authorities'),
('Optional'),
('Observance');

CREATE TABLE Tags
(
    Id INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Color NVARCHAR(7) NOT NULL,
    Owner NVARCHAR(50) NOT NULL,
    FOREIGN KEY (Owner) REFERENCES Users (Username),
    PRIMARY KEY (Id, Owner)
);

CREATE TABLE TaskTags
(
    TaskId INT NOT NULL,
    TagId INT NOT NULL,
    Owner NVARCHAR(50) NOT NULL,
    PRIMARY KEY (TaskId, TagId, Owner),
    FOREIGN KEY (TaskId) REFERENCES Tasks (Id),
    FOREIGN KEY (TagId, Owner) REFERENCES Tags (Id, Owner)
);

CREATE INDEX idx_tags_Owner ON Tags (Owner);

CREATE INDEX idx_tasktags_taskid ON TaskTags (TaskId);
CREATE INDEX idx_tasktags_tagid ON TaskTags (TagId, Owner);

INSERT INTO Tags (Id, Name, Color, Owner)
VALUES 
(1,'Tag1', '#FF0000', 'Admin'), -- Red
(2,'Tag2', '#00FF00', 'Admin'), -- Green
(3,'Tag3', '#0000FF', 'Admin'), -- Blue
(4,'Tag4', '#FFFF00', 'Admin'), -- Yellow
(5,'Tag5', '#FF00FF', 'Admin'); -- Magenta

DECLARE @TagId INT;
SELECT @TagId = Id FROM Tags WHERE Name = 'Tag1' AND Owner = 'Admin';

-- Associate the first tag with task 1
INSERT INTO TaskTags (TaskId, TagId, Owner)
VALUES (1, @TagId, 'Admin');
go

CREATE TRIGGER trg_InsertTagsForNewUser
ON Users
AFTER INSERT
AS
BEGIN
    -- Insert tags for the new user
    INSERT INTO Tags (Id, Name, Color, Owner)
    SELECT 1, 'Tag1', '#FF0000', i.Username FROM inserted i
    UNION ALL
    SELECT 2, 'Tag2', '#00FF00', i.Username FROM inserted i
    UNION ALL
    SELECT 3, 'Tag3', '#0000FF', i.Username FROM inserted i
    UNION ALL
    SELECT 4, 'Tag4', '#FFFF00', i.Username FROM inserted i
    UNION ALL
    SELECT 5, 'Tag5', '#FF00FF', i.Username FROM inserted i;
END;
GO

PRINT 'Initialization script completed successfully.'