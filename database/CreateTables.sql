USE [OnlineCourses]
GO

DROP TABLE IF EXISTS dbo.[CourseInstructor]
GO

DROP TABLE IF EXISTS dbo.[Instructor]
GO

DROP TABLE IF EXISTS dbo.[Comment]
GO

DROP TABLE IF EXISTS dbo.[Price]
GO

DROP TABLE IF EXISTS dbo.[Course]
GO

CREATE TABLE dbo.[Course]
(
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL CONSTRAINT [DF_Course_Id] DEFAULT NEWSEQUENTIALID() NOT NULL,
	[Title] NVARCHAR(500) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	[PublishingDate] DATE NULL,
	[ProfilePicture] VARBINARY(MAX) NULL,
	[CreatedAt] DATETIME2(3) CONSTRAINT [DF_Course_CreatedAt] DEFAULT GETDATE() NOT NULL,
	[UpdatedAt] DATETIME2(3) CONSTRAINT [DF_Course_UpdatedAt] NULL,
	[CreatedBy] UNIQUEIDENTIFIER NULL,
	[UpdatedBy] UNIQUEIDENTIFIER NULL,
	[RowVersion] TIMESTAMP NOT NULL,

	CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([Id] ASC),
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Course_Title] ON dbo.[Course] ([Title] ASC)
GO
INSERT INTO dbo.[Course]([Title], [Description], [PublishingDate]) VALUES('React Hooks Firebase y Material Design', 'Curso de programación de React', '2020-02-05')
INSERT INTO dbo.[Course]([Title], [Description], [PublishingDate]) VALUES('ASP.NET Core y React Hooks', 'Curso de .NET y Javascript', '2020-11-05')
--INSERT INTO dbo.[Course]([Title], [Description], [PublishingDate]) VALUES('ASP.NET Core y React Hooks', 'Curso de .NET y Javascript', '2020-11-05')
GO

CREATE TABLE Price
(
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL CONSTRAINT [DF_Price_Id] DEFAULT NEWSEQUENTIALID() NOT NULL,
	[CourseId] UNIQUEIDENTIFIER NOT NULL,
	[CurrentPrice] DECIMAL(10,4) NOT NULL,
	[Promotion] DECIMAL(10,4) DEFAULT 0.00 NOT NULL,
	[CreatedAt] DATETIME2(3) CONSTRAINT [DF_Price_CreatedAt] DEFAULT GETDATE() NOT NULL,
	[UpdatedAt] DATETIME2(3) CONSTRAINT [DF_Price_UpdatedAt] NULL,
	[CreatedBy] UNIQUEIDENTIFIER NULL,
	[UpdatedBy] UNIQUEIDENTIFIER NULL,
	[RowVersion] TIMESTAMP NOT NULL,

	CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Price_Course_CourseId] FOREIGN KEY([CourseId]) REFERENCES dbo.[Course]([Id]),
	CONSTRAINT [CK_Price_CurrentPrice] CHECK([CurrentPrice] >= 0),
	CONSTRAINT [CK_Price_Promotion] CHECK([Promotion] >= 0)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Price_CourseId] ON dbo.[Price] ([CourseId] ASC)
GO
DECLARE @courseId1 UNIQUEIDENTIFIER;
DECLARE @courseId2 UNIQUEIDENTIFIER;
SELECT TOP(1) @courseId1 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
SELECT @courseId2 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
INSERT INTO dbo.[Price]([CourseId], [CurrentPrice], [Promotion]) VALUES(@courseId1, 900, 9.99)
INSERT INTO dbo.[Price]([CourseId], [CurrentPrice], [Promotion]) VALUES(@courseId2, 650, 15)
GO

CREATE TABLE Comment
(
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL CONSTRAINT [DF_Comment_Id] DEFAULT NEWSEQUENTIALID() NOT NULL,
	[CourseId] UNIQUEIDENTIFIER NOT NULL,
	[StudentName] NVARCHAR(60) NOT NULL,
	[Message] NVARCHAR(1500) NULL,
	[Score] TINYINT NOT NULL,
	[CreatedAt] DATETIME2(3) CONSTRAINT [DF_Comment_CreatedAt] DEFAULT GETDATE() NOT NULL,
	[UpdatedAt] DATETIME2(3) CONSTRAINT [DF_Comment_UpdatedAt] NULL,
	[CreatedBy] UNIQUEIDENTIFIER NULL,
	[UpdatedBy] UNIQUEIDENTIFIER NULL,
	[RowVersion] TIMESTAMP NOT NULL,

	CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Comment_Course_CourseId] FOREIGN KEY([CourseId]) REFERENCES dbo.[Course]([Id]),
	CONSTRAINT [CK_Comment_Score] CHECK([Score] > 0 AND [Score] <= 5)
)
GO
CREATE NONCLUSTERED INDEX [IX_Comment_CourseId] ON dbo.[Comment] ([CourseId] ASC)
GO
DECLARE @courseId1 UNIQUEIDENTIFIER;
DECLARE @courseId2 UNIQUEIDENTIFIER;
SELECT TOP(1) @courseId1 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
SELECT @courseId2 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
INSERT INTO dbo.[Comment]([CourseId], [StudentName], [Message], [Score]) VALUES(@courseId1, 'Alberto Rosales', 'Es el mejor curso de React', 5)
INSERT INTO dbo.[Comment]([CourseId], [StudentName], [Message], [Score]) VALUES(@courseId1, 'Román Albeiro', 'Curso excelente de Programación', 5)
INSERT INTO dbo.[Comment]([CourseId], [StudentName], [Message], [Score]) VALUES(@courseId1, 'Ángela Arias', 'Laboratorios muy prácticos', 4)
INSERT INTO dbo.[Comment]([CourseId], [StudentName], [Message], [Score]) VALUES(@courseId2, 'Fabián Drez', 'Buen curso de ASP.NET Core', 5)
INSERT INTO dbo.[Comment]([CourseId], [StudentName], [Message], [Score]) VALUES(@courseId2, 'Felipe Benegas', 'Sql Server al máximo', 5)
GO

CREATE TABLE Instructor
(
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL CONSTRAINT [DF_Instructor_Id] DEFAULT NEWSEQUENTIALID() NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NULL,
	[Degree] NVARCHAR(100) NULL,
	[ProfilePicture] VARBINARY(MAX) NULL,
	[CreatedAt] DATETIME2(3) CONSTRAINT [DF_Instructor_CreatedAt] DEFAULT GETDATE() NOT NULL,
	[UpdatedAt] DATETIME2(3) CONSTRAINT [DF_Instructor_UpdatedAt] NULL,
	[CreatedBy] UNIQUEIDENTIFIER NULL,
	[UpdatedBy] UNIQUEIDENTIFIER NULL,
	[RowVersion] TIMESTAMP NOT NULL,

	CONSTRAINT [PK_Instructor] PRIMARY KEY CLUSTERED ([Id] ASC),
)
GO
INSERT INTO dbo.[Instructor]([FirstName], [LastName], [Degree]) VALUES('Vaxi', 'Drez', 'Máster')
INSERT INTO dbo.[Instructor]([FirstName], [LastName], [Degree]) VALUES('Nestor', 'Arcila', 'Ingeniero')
INSERT INTO dbo.[Instructor]([FirstName], [LastName], [Degree]) VALUES('John', 'Ortiz', 'Técnico')
INSERT INTO dbo.[Instructor]([FirstName], [LastName], [Degree]) VALUES('Ángela', 'Arias', 'Ingeniero')
GO

CREATE TABLE CourseInstructor
(
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL CONSTRAINT [DF_CourseInstructor_Id] DEFAULT NEWSEQUENTIALID() NOT NULL,
	[CourseId] UNIQUEIDENTIFIER NOT NULL,
	[InstructorId] UNIQUEIDENTIFIER NOT NULL,
	[CreatedAt] DATETIME2(3) CONSTRAINT [DF_CourseInstructor_CreatedAt] DEFAULT GETDATE() NOT NULL,
	[UpdatedAt] DATETIME2(3) CONSTRAINT [DF_CourseInstructor_UpdatedAt] NULL,
	[CreatedBy] UNIQUEIDENTIFIER NULL,
	[UpdatedBy] UNIQUEIDENTIFIER NULL,
	[RowVersion] TIMESTAMP NOT NULL,

	CONSTRAINT [PK_CourseInstructor] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_CourseInstructor_Course_CourseId] FOREIGN KEY([CourseId]) REFERENCES dbo.[Course]([Id]),
	CONSTRAINT [FK_CourseInstructor_Instructor_InstructorId] FOREIGN KEY([InstructorId]) REFERENCES dbo.[Instructor]([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_CourseInstructor_CourseId] ON dbo.[CourseInstructor] ([CourseId] ASC)
CREATE NONCLUSTERED INDEX [IX_CourseInstructor_InstructorId] ON dbo.[CourseInstructor] ([InstructorId] ASC)
GO
DECLARE @courseId1 UNIQUEIDENTIFIER;
DECLARE @courseId2 UNIQUEIDENTIFIER;
SELECT TOP(1) @courseId1 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
SELECT @courseId2 = C.[Id] FROM dbo.[Course] C ORDER BY C.[Id]
INSERT INTO dbo.[CourseInstructor]([CourseId], [InstructorId])
SELECT TOP(3) @courseId1, I.[Id] FROM dbo.[Instructor] I
INSERT INTO dbo.[CourseInstructor]([CourseId], [InstructorId])
SELECT TOP(1) @courseId2, I.[Id] FROM dbo.[Instructor] I ORDER BY I.[Id] DESC
INSERT INTO dbo.[CourseInstructor]([CourseId], [InstructorId])
SELECT TOP(1) @courseId2, I.[Id] FROM dbo.[Instructor] I
GO

--SELECT * FROM dbo.[Course]
--SELECT * FROM dbo.[Price]
--SELECT * FROM dbo.[Comment]
--GO

--SELECT C.[Id], C.[Title], C.[Description], C.[PublishingDate], P.[CurrentPrice], P.[Promotion], Co.[StudentName], Co.[Message], Co.[Score]  FROM dbo.[Course] C
--LEFT JOIN dbo.[Price] P ON P.[CourseId] = C.[Id]
--LEFT JOIN dbo.[Comment] Co ON Co.[CourseId] = C.[Id]
