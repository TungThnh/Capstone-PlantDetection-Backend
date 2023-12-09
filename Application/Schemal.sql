/****** Object:  Database [PlantDetection]    Script Date: 10/11/2023 10:39:40 PM ******/
CREATE DATABASE [PlantDetection]
GO
Use [PlantDetection]
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [uniqueidentifier] NOT NULL,
	[ManagerId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Note] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateAt] [datetime] NOT NULL,
	[NumberOfMember] [int] NOT NULL,
	[Status] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK__Class__3214EC07680D71EB] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [uniqueidentifier] NOT NULL,
	[PlantId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manager]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manager](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](256) NULL,
	[Address] [nvarchar](max) NULL,
	[DayOfBirth] [nvarchar](256) NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[Status] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plant]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plant](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Code] [nvarchar](256) NOT NULL,
	[Status] [nvarchar](256) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlantCategory]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlantCategory](
	[CategoryId] [uniqueidentifier] NOT NULL,
	[PlantId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[PlantId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[Id] [uniqueidentifier] NOT NULL,
	[StudentId] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[Label] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](256) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Result]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Result](
	[Id] [uniqueidentifier] NOT NULL,
	[ReportId] [uniqueidentifier] NOT NULL,
	[PlantId] [uniqueidentifier] NULL,
	[Message] [nvarchar](max) NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[College] [nvarchar](256) NULL,
	[Phone] [nvarchar](256) NULL,
	[Address] [nvarchar](max) NULL,
	[DayOfBirth] [nvarchar](256) NULL,
	[Status] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClass]    Script Date: 10/11/2023 10:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClass](
	[ClassId] [uniqueidentifier] NOT NULL,
	[StudentId] [uniqueidentifier] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[JoinAt] [datetime] NULL,
	[Status] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClassId] ASC,
	[StudentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Class] ([Id], [ManagerId], [Code], [Name], [Description], [CreateAt], [NumberOfMember], [Status]) VALUES (N'6b8105bb-e985-4be7-b16c-7d797ed43467', N'f5da4b6f-3a98-4199-9d9b-64a55c8d9fae', N'HD001', N'L?p c?a ??ng', N'L?p có s?c hút', CAST(N'2023-10-01T07:59:03.020' AS DateTime), 3, N'Pending Approval')
GO
INSERT [dbo].[Manager] ([Id], [FirstName], [LastName], [Email], [Phone], [Address], [DayOfBirth], [AvatarUrl], [Status]) VALUES (N'f5da4b6f-3a98-4199-9d9b-64a55c8d9fae', N'hai', N'dang lo ba', N'ghostvshell@gmail.com', NULL, NULL, NULL, N'https://lh3.googleusercontent.com/a/ACg8ocKrg7FV1A-oQcduVB-JlbnkrBv9GYbVV7fyBlCHKJD3=s96-c', N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'87bdc750-2127-4015-8bf5-01a3c020be02', N'dang hai', N'lo ', N'ghostvshell@gmail.com', N'https://lh3.googleusercontent.com/a/ACg8ocKrg7FV1A-oQcduVB-JlbnkrBv9GYbVV7fyBlCHKJD3=s96-c', N'fpt univer', N'0328749312', N'512 nguyen xien', N'24/11/1999', N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'c4d39bb3-08fe-4097-9108-06955f331926', N'Pekeo', N'Pekeo', N'yuriboyka1108@gmail.com', N'https://lh3.googleusercontent.com/a/ACg8ocJVIkb8AkrJIWe3LAY_hMLDPKUbS0Pg7VwCfUAuHp-q0gU=s96-c', N'', N'123', N'', N'', N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'f71f99a7-db11-4a5c-a746-67357e844635', N'hai', N'dang lo ba', N'124357689hell@gmail.com', N'https://lh3.googleusercontent.com/a/ACg8ocIX_LYOMqDCu4U8-I9wqYIt8C0A1FZaBuRZZIpwsl4Z=s96-c', NULL, NULL, NULL, NULL, N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'b56cd659-d6fe-4f21-99a7-8dd86b9ae184', N'Tài', N'Võ T?n', N'taivtse130716@fpt.edu.vn', N'https://lh3.googleusercontent.com/a/ACg8ocKAXk9jC7yIZmmNdYT7xnHD2pawhENdVrfgclcFgKBtE_4=s96-c', NULL, NULL, NULL, NULL, N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'2a373bb9-7152-45a3-81a8-90cfad603cec', N'HCM)', N'Phan Hoang Chien (K14', N'chienphse140586@fpt.edu.vn', N'https://lh3.googleusercontent.com/a/ACg8ocJqbcDxs9B8V7emNNlqBWfzSiTVZNZLZDrN7JbTNdWxdQ=s96-c', N'FPT', N'09649049170000000', N'', N'', N'Active')
GO
INSERT [dbo].[Student] ([Id], [FirstName], [LastName], [Email], [AvatarUrl], [College], [Phone], [Address], [DayOfBirth], [Status]) VALUES (N'50454757-cf7f-47fa-97d8-f71a81583d37', N'Võ', N'T?n Tài', N'votantai4899@gmail.com', N'https://lh3.googleusercontent.com/a/ACg8ocKvqDoAiWkm88ibWKk-w09k0_MKs5oa5-Md7vCWnScooxGH=s96-c', N'FPT University', N'', N'', N'', N'Active')
GO
INSERT [dbo].[StudentClass] ([ClassId], [StudentId], [CreateAt], [JoinAt], [Status], [Description]) VALUES (N'6b8105bb-e985-4be7-b16c-7d797ed43467', N'87bdc750-2127-4015-8bf5-01a3c020be02', CAST(N'2023-10-05T04:34:52.770' AS DateTime), NULL, N'Enrolled', NULL)
GO
INSERT [dbo].[StudentClass] ([ClassId], [StudentId], [CreateAt], [JoinAt], [Status], [Description]) VALUES (N'6b8105bb-e985-4be7-b16c-7d797ed43467', N'f71f99a7-db11-4a5c-a746-67357e844635', CAST(N'2023-10-07T11:55:38.480' AS DateTime), NULL, N'Pending Approval', NULL)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Manager__A9D1053454510217]    Script Date: 10/11/2023 10:39:43 PM ******/
ALTER TABLE [dbo].[Manager] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Result__D5BD480459B62BBA]    Script Date: 10/11/2023 10:39:43 PM ******/
ALTER TABLE [dbo].[Result] ADD UNIQUE NONCLUSTERED 
(
	[ReportId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Student__A9D1053458AC8DF1]    Script Date: 10/11/2023 10:39:43 PM ******/
ALTER TABLE [dbo].[Student] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__StudentC__32C52B989481B62B]    Script Date: 10/11/2023 10:39:43 PM ******/
ALTER TABLE [dbo].[StudentClass] ADD UNIQUE NONCLUSTERED 
(
	[StudentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Class] ADD  CONSTRAINT [DF__Class__CreateAt__6FE99F9F]  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Image] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Plant] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Report] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Result] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[StudentClass] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK__Class__ManagerId__6EF57B66] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Manager] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK__Class__ManagerId__6EF57B66]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD FOREIGN KEY([PlantId])
REFERENCES [dbo].[Plant] ([Id])
GO
ALTER TABLE [dbo].[PlantCategory]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[PlantCategory]  WITH CHECK ADD FOREIGN KEY([PlantId])
REFERENCES [dbo].[Plant] ([Id])
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([PlantId])
REFERENCES [dbo].[Plant] ([Id])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([ReportId])
REFERENCES [dbo].[Report] ([Id])
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD  CONSTRAINT [FK__StudentCl__Class__73BA3083] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[StudentClass] CHECK CONSTRAINT [FK__StudentCl__Class__73BA3083]
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER DATABASE [PlantDetection] SET  READ_WRITE 
GO
