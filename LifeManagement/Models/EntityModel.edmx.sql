
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/23/2017 17:59:10
-- Generated from EDMX file: C:\Users\fher\Source\Repos\Life-Management-Platform-1.0\LifeManagement\Models\EntityModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SeniorDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_RoleUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_RoleUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryGoal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goals] DROP CONSTRAINT [FK_CategoryGoal];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Activities] DROP CONSTRAINT [FK_CategoryActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SprintGoal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goals] DROP CONSTRAINT [FK_SprintGoal];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSprint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sprints] DROP CONSTRAINT [FK_UserSprint];
GO
IF OBJECT_ID(N'[dbo].[FK_SprintSprintActivities]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SprintActivities] DROP CONSTRAINT [FK_SprintSprintActivities];
GO
IF OBJECT_ID(N'[dbo].[FK_ActivitySprintActivities]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SprintActivities] DROP CONSTRAINT [FK_ActivitySprintActivities];
GO
IF OBJECT_ID(N'[dbo].[FK_SprintActivitiesProgress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Progresses] DROP CONSTRAINT [FK_SprintActivitiesProgress];
GO
IF OBJECT_ID(N'[dbo].[FK_CoachUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coaches] DROP CONSTRAINT [FK_CoachUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CoachCoachReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CoachReviews] DROP CONSTRAINT [FK_CoachCoachReview];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCoachReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CoachReviews] DROP CONSTRAINT [FK_UserCoachReview];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Goals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goals];
GO
IF OBJECT_ID(N'[dbo].[Activities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activities];
GO
IF OBJECT_ID(N'[dbo].[Sprints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sprints];
GO
IF OBJECT_ID(N'[dbo].[Progresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Progresses];
GO
IF OBJECT_ID(N'[dbo].[SprintActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SprintActivities];
GO
IF OBJECT_ID(N'[dbo].[Coaches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coaches];
GO
IF OBJECT_ID(N'[dbo].[CoachReviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CoachReviews];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [DOB] datetime  NOT NULL,
    [RoleId] int  NOT NULL,
    [username] nvarchar(50)  NOT NULL,
    [password] nvarchar(20)  NOT NULL,
    [DateCreated] datetime  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Goals'
CREATE TABLE [dbo].[Goals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CategoryId] int  NOT NULL,
    [SprintId] int  NOT NULL
);
GO

-- Creating table 'Activities'
CREATE TABLE [dbo].[Activities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CategoryId] int  NOT NULL,
    [Img] tinyint  NOT NULL,
    [ImgMime] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Sprints'
CREATE TABLE [dbo].[Sprints] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SprintGoal] nvarchar(max)  NOT NULL,
    [DateFrom] datetime  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Progresses'
CREATE TABLE [dbo].[Progresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SprintActivitiesId] int  NOT NULL,
    [DatePerformed] datetime  NOT NULL
);
GO

-- Creating table 'SprintActivities'
CREATE TABLE [dbo].[SprintActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Specifics] nvarchar(max)  NOT NULL,
    [SprintId] int  NOT NULL,
    [ActivityId] int  NOT NULL
);
GO

-- Creating table 'Coaches'
CREATE TABLE [dbo].[Coaches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ReviewScore] int  NOT NULL,
    [Biography] nvarchar(max)  NOT NULL,
    [Skills] nvarchar(max)  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'CoachReviews'
CREATE TABLE [dbo].[CoachReviews] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CoachId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Review] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [PK_Goals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sprints'
ALTER TABLE [dbo].[Sprints]
ADD CONSTRAINT [PK_Sprints]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Progresses'
ALTER TABLE [dbo].[Progresses]
ADD CONSTRAINT [PK_Progresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SprintActivities'
ALTER TABLE [dbo].[SprintActivities]
ADD CONSTRAINT [PK_SprintActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Coaches'
ALTER TABLE [dbo].[Coaches]
ADD CONSTRAINT [PK_Coaches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CoachReviews'
ALTER TABLE [dbo].[CoachReviews]
ADD CONSTRAINT [PK_CoachReviews]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_RoleUser]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUser'
CREATE INDEX [IX_FK_RoleUser]
ON [dbo].[Users]
    ([RoleId]);
GO

-- Creating foreign key on [CategoryId] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [FK_CategoryGoal]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryGoal'
CREATE INDEX [IX_FK_CategoryGoal]
ON [dbo].[Goals]
    ([CategoryId]);
GO

-- Creating foreign key on [CategoryId] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [FK_CategoryActivity]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryActivity'
CREATE INDEX [IX_FK_CategoryActivity]
ON [dbo].[Activities]
    ([CategoryId]);
GO

-- Creating foreign key on [SprintId] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [FK_SprintGoal]
    FOREIGN KEY ([SprintId])
    REFERENCES [dbo].[Sprints]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SprintGoal'
CREATE INDEX [IX_FK_SprintGoal]
ON [dbo].[Goals]
    ([SprintId]);
GO

-- Creating foreign key on [UserId] in table 'Sprints'
ALTER TABLE [dbo].[Sprints]
ADD CONSTRAINT [FK_UserSprint]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSprint'
CREATE INDEX [IX_FK_UserSprint]
ON [dbo].[Sprints]
    ([UserId]);
GO

-- Creating foreign key on [SprintId] in table 'SprintActivities'
ALTER TABLE [dbo].[SprintActivities]
ADD CONSTRAINT [FK_SprintSprintActivities]
    FOREIGN KEY ([SprintId])
    REFERENCES [dbo].[Sprints]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SprintSprintActivities'
CREATE INDEX [IX_FK_SprintSprintActivities]
ON [dbo].[SprintActivities]
    ([SprintId]);
GO

-- Creating foreign key on [ActivityId] in table 'SprintActivities'
ALTER TABLE [dbo].[SprintActivities]
ADD CONSTRAINT [FK_ActivitySprintActivities]
    FOREIGN KEY ([ActivityId])
    REFERENCES [dbo].[Activities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivitySprintActivities'
CREATE INDEX [IX_FK_ActivitySprintActivities]
ON [dbo].[SprintActivities]
    ([ActivityId]);
GO

-- Creating foreign key on [SprintActivitiesId] in table 'Progresses'
ALTER TABLE [dbo].[Progresses]
ADD CONSTRAINT [FK_SprintActivitiesProgress]
    FOREIGN KEY ([SprintActivitiesId])
    REFERENCES [dbo].[SprintActivities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SprintActivitiesProgress'
CREATE INDEX [IX_FK_SprintActivitiesProgress]
ON [dbo].[Progresses]
    ([SprintActivitiesId]);
GO

-- Creating foreign key on [User_Id] in table 'Coaches'
ALTER TABLE [dbo].[Coaches]
ADD CONSTRAINT [FK_CoachUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachUser'
CREATE INDEX [IX_FK_CoachUser]
ON [dbo].[Coaches]
    ([User_Id]);
GO

-- Creating foreign key on [CoachId] in table 'CoachReviews'
ALTER TABLE [dbo].[CoachReviews]
ADD CONSTRAINT [FK_CoachCoachReview]
    FOREIGN KEY ([CoachId])
    REFERENCES [dbo].[Coaches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachCoachReview'
CREATE INDEX [IX_FK_CoachCoachReview]
ON [dbo].[CoachReviews]
    ([CoachId]);
GO

-- Creating foreign key on [UserId] in table 'CoachReviews'
ALTER TABLE [dbo].[CoachReviews]
ADD CONSTRAINT [FK_UserCoachReview]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCoachReview'
CREATE INDEX [IX_FK_UserCoachReview]
ON [dbo].[CoachReviews]
    ([UserId]);
GO
INSERT INTO [dbo].[Roles]
           ([Name])
     VALUES
           ('Guest'),('User'),('Admin')

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------