
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/20/2021 20:37:01
-- Generated from EDMX file: C:\Users\telis\source\repos\WindowsFormsChessApp\WindowsFormsChessApp\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CS2021];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GameHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_GameHistory];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Histories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Histories];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Player_black_nickname] nvarchar(max)  NOT NULL,
    [Player_white_nickname] nvarchar(max)  NOT NULL,
    [Winner] nvarchar(max)  NOT NULL,
    [Timestamp] nvarchar(max)  NOT NULL,
    [Duration] nvarchar(max)  NOT NULL,
    [GivenTime] nvarchar(max)  NOT NULL,
    [Remaining_white_player] nvarchar(max)  NOT NULL,
    [Remaining_black_player] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Histories'
CREATE TABLE [dbo].[Histories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Move] nvarchar(max)  NOT NULL,
    [Game_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [PK_Histories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Game_Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_GameHistory]
    FOREIGN KEY ([Game_Id])
    REFERENCES [dbo].[Games]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameHistory'
CREATE INDEX [IX_FK_GameHistory]
ON [dbo].[Histories]
    ([Game_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------