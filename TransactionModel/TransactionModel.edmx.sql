
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/27/2011 12:12:15
-- Generated from EDMX file: C:\Documents and Settings\chaiwat.k\Desktop\MyEverything\CarPass\VSS\Devs\Transaction.root\Transaction\TransactionModel\TransactionModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TRANSACTION_DEV];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Configurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configurations];
GO
IF OBJECT_ID(N'[dbo].[ErrorLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrorLogs];
GO
IF OBJECT_ID(N'[dbo].[PaymentCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentCodes];
GO
IF OBJECT_ID(N'[dbo].[ServiceCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCodes];
GO
IF OBJECT_ID(N'[dbo].[ReconciliationFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReconciliationFiles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [Group] nvarchar(50)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Value1] nvarchar(100)  NOT NULL,
    [Value2] nvarchar(100)  NULL,
    [Value3] nvarchar(100)  NULL,
    [Description] nvarchar(255)  NULL,
    [Version] timestamp  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateBy] nvarchar(50)  NOT NULL,
    [UpdateDate] datetime  NOT NULL,
    [UpdateBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'ErrorLogs'
CREATE TABLE [dbo].[ErrorLogs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [IssuedBy] nvarchar(50)  NOT NULL,
    [IssuedDate] datetime  NOT NULL,
    [IssuedMessage] nvarchar(max)  NOT NULL,
    [Severity] tinyint  NOT NULL,
    [Version] timestamp  NOT NULL
);
GO

-- Creating table 'PaymentCodes'
CREATE TABLE [dbo].[PaymentCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] char(8)  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [Version] timestamp  NOT NULL
);
GO

-- Creating table 'ServiceCodes'
CREATE TABLE [dbo].[ServiceCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] int  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [IsRevenue] bit  NOT NULL,
    [Description] nvarchar(255)  NULL
);
GO

-- Creating table 'ReconciliationFiles'
CREATE TABLE [dbo].[ReconciliationFiles] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(100)  NOT NULL,
    [Contents] varbinary(max)  NOT NULL,
    [BackupPath] nvarchar(255)  NULL,
    [IsValid] bit  NOT NULL,
    [Source] nvarchar(255)  NULL,
    [Version] timestamp  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateBy] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Group], [Name] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [PK_Configurations]
    PRIMARY KEY CLUSTERED ([Group], [Name] ASC);
GO

-- Creating primary key on [Id] in table 'ErrorLogs'
ALTER TABLE [dbo].[ErrorLogs]
ADD CONSTRAINT [PK_ErrorLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentCodes'
ALTER TABLE [dbo].[PaymentCodes]
ADD CONSTRAINT [PK_PaymentCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceCodes'
ALTER TABLE [dbo].[ServiceCodes]
ADD CONSTRAINT [PK_ServiceCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReconciliationFiles'
ALTER TABLE [dbo].[ReconciliationFiles]
ADD CONSTRAINT [PK_ReconciliationFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------