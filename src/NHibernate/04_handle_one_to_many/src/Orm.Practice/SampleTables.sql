-->> NOTE: THIS SCRIPT MUST BE RUN IN SQLCMD MODE INSIDE SQL SERVER MANAGEMENT STUDIO. <<--
SET NOCOUNT OFF;
GO

PRINT CONVERT(varchar(1000), @@VERSION);
GO

PRINT '';
PRINT 'Started - ' + CONVERT(varchar, GETDATE(), 121);
GO

USE [master];
GO
-- ****************************************
-- Drop Database
-- ****************************************
PRINT '';
PRINT '*** Dropping Database';
GO

IF EXISTS (SELECT [name] FROM [master].[sys].[databases] WHERE [name] = N'AwesomeDb')
    DROP DATABASE [AwesomeDb];

-- If the database has any other open connections close the network connection.
IF @@ERROR = 3702 
    RAISERROR('[AwesomeDb] database cannot be dropped because there are still other open connections', 127, 127) WITH NOWAIT, LOG;
GO


-- ****************************************
-- Create Database
-- ****************************************
PRINT '';
PRINT '*** Creating Database';
GO

CREATE DATABASE [AwesomeDb]
GO

PRINT '';
PRINT '*** Checking for AwesomeDb Database';

/* CHECK FOR DATABASE IF IT DOESN'T EXISTS, DO NOT RUN THE REST OF THE SCRIPT */
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE name = N'AwesomeDb')
BEGIN
PRINT 'AwesomeDb Database does not exist.  Make sure that the script is being run in SQLCMD mode and that the variables have been correctly set.';
SET NOEXEC ON;
END
GO

ALTER DATABASE [AwesomeDb] 
SET RECOVERY SIMPLE, 
    ANSI_NULLS ON, 
    ANSI_PADDING ON, 
    ANSI_WARNINGS ON, 
    ARITHABORT ON, 
    CONCAT_NULL_YIELDS_NULL ON, 
    QUOTED_IDENTIFIER ON, 
    NUMERIC_ROUNDABORT OFF, 
    PAGE_VERIFY CHECKSUM, 
    ALLOW_SNAPSHOT_ISOLATION OFF;
GO

USE [AwesomeDb];
GO

-- ******************************************************
-- Create tables
-- ******************************************************

PRINT '';
PRINT '*** Creating Tables';
GO

CREATE TABLE [dbo].[parent](
    [ParentID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
) ON [PRIMARY];
GO

INSERT INTO [dbo].[parent] VALUES 
    ('e395b6fc-14ff-47da-819e-526d6c9896d3', 'parent-query-1', 1),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b8f', 'parent-query-2', 1);
GO

CREATE TABLE [dbo].[child](
    [ChildID] [UNIQUEIDENTIFIER] NOT NULL,
    [ParentID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
)
GO

INSERT INTO [dbo].[child] VALUES
    (NEWID(), 'e395b6fc-14ff-47da-819e-526d6c9896d3', 'child-1-for-parent-1', 1),
    (NEWID(), 'e395b6fc-14ff-47da-819e-526d6c9896d3', 'child-2-for-parent-1', 1),
    (NEWID(), 'e395b6fc-14ff-47da-819e-526d6c9896d3', 'child-3-for-parent-1', 1),
    (NEWID(), 'e395b6fc-14ff-47da-819e-526d6c9896d3', 'child-4-for-parent-1', 1),
    (NEWID(), 'dfbfd41d-e7c6-4709-9bf0-bd490d227b8f', 'child-1-for-parent-2', 1),
    (NEWID(), 'dfbfd41d-e7c6-4709-9bf0-bd490d227b8f', 'child-2-for-parent-2', 1),
    (NEWID(), 'dfbfd41d-e7c6-4709-9bf0-bd490d227b8f', 'child-3-for-parent-2', 1);
GO

ALTER TABLE [dbo].[parent] WITH CHECK ADD 
    CONSTRAINT [PK_parent_parentid] PRIMARY KEY CLUSTERED 
    (
        [ParentID]
    );
GO

ALTER TABLE [dbo].[child] WITH CHECK ADD 
    CONSTRAINT [PK_child_childid] PRIMARY KEY CLUSTERED 
    (
        [ChildID]
    );
GO

CREATE TABLE [dbo].[student](
	[StudentID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[teacher](
	[TeacherID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[student_teacher](
	[StudentID] [UNIQUEIDENTIFIER] NOT NULL,
	[TeacherID] [UNIQUEIDENTIFIER] NOT NULL,
)
GO

ALTER TABLE [dbo].[student] WITH CHECK ADD 
    CONSTRAINT [PK_student_studentid] PRIMARY KEY CLUSTERED 
    (
        [StudentID]
    );
GO

ALTER TABLE [dbo].[teacher] WITH CHECK ADD 
    CONSTRAINT [PK_teacher_teacherid] PRIMARY KEY CLUSTERED 
    (
        [TeacherID]
    );
GO

INSERT INTO [dbo].[teacher] VALUES 
    ('B5EF40D6-EC15-4578-8C46-DC7690D25DB3', 'teacher 1', 1),
    ('4638F01A-B370-4596-905B-83C576F7C94F', 'teacher 2', 1);
GO

INSERT INTO [dbo].[student] VALUES 
    ('FA7067F3-2B27-46BA-88E6-FEC9B26CA46E', 'student 1', 1),
    ('C528A854-EB58-4DDC-B569-00A3DE1C83C9', 'student 2', 1);
GO

INSERT INTO [dbo].[student_teacher] VALUES 
    ('FA7067F3-2B27-46BA-88E6-FEC9B26CA46E', 'B5EF40D6-EC15-4578-8C46-DC7690D25DB3'),
    ('C528A854-EB58-4DDC-B569-00A3DE1C83C9', 'B5EF40D6-EC15-4578-8C46-DC7690D25DB3'),
    ('C528A854-EB58-4DDC-B569-00A3DE1C83C9', '4638F01A-B370-4596-905B-83C576F7C94F'),
    ('FA7067F3-2B27-46BA-88E6-FEC9B26CA46E', '4638F01A-B370-4596-905B-83C576F7C94F');
GO

-- ****************************************
-- Shrink Database
-- ****************************************
PRINT '';
PRINT '*** Shrinking Database';
GO

DBCC SHRINKDATABASE ([AwesomeDb]);
GO


USE [master];
GO

PRINT 'Finished - ' + CONVERT(varchar, GETDATE(), 121);
GO


SET NOEXEC OFF