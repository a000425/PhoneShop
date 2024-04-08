USE [master]
GO
/****** Object:  Database [Phone]    Script Date: 2024/4/8 下午 05:34:30 ******/
CREATE DATABASE [Phone]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Phone', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Phone.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Phone_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Phone_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Phone] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Phone].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Phone] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Phone] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Phone] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Phone] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Phone] SET ARITHABORT OFF 
GO
ALTER DATABASE [Phone] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Phone] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Phone] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Phone] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Phone] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Phone] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Phone] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Phone] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Phone] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Phone] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Phone] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Phone] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Phone] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Phone] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Phone] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Phone] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Phone] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Phone] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Phone] SET  MULTI_USER 
GO
ALTER DATABASE [Phone] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Phone] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Phone] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Phone] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Phone] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Phone] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Phone] SET QUERY_STORE = OFF
GO
USE [Phone]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Account] [varchar](20) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[Name] [varchar](10) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Cellphone] [varchar](10) NOT NULL,
	[AuthCode] [varchar](10) NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK__Account__B0C3AC47E3AEAEB6] PRIMARY KEY CLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Account] [varchar](20) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemNum] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[FormatId] [int] NOT NULL,
 CONSTRAINT [PK__Cart__3214EC07031FF0EE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Format]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Format](
	[FormatId] [int] IDENTITY(0,1) NOT NULL,
	[Brand] [varchar](50) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[ItemPrice] [int] NOT NULL,
	[Store] [int] NOT NULL,
	[Space] [varchar](5) NOT NULL,
	[ItemId] [int] NOT NULL,
 CONSTRAINT [PK__Format__5D3DCB5948F76996] PRIMARY KEY CLUSTERED 
(
	[FormatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Img]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Img](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[FormatId] [int] NOT NULL,
	[ItemImg] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [int] IDENTITY(0,1) NOT NULL,
	[ItemName] [varchar](100) NOT NULL,
	[Instruction] [varchar](200) NOT NULL,
	[IsAvailable] [bit] NOT NULL,
 CONSTRAINT [PK__Item__727E838BED3998F9] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(0,1) NOT NULL,
	[Account] [varchar](20) NOT NULL,
	[TotalPrice] [int] NOT NULL,
	[OrderTime] [datetime] NOT NULL,
	[OrderStatus] [varchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Order__C3905BCF73228E1C] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemNum] [int] NOT NULL,
 CONSTRAINT [PK__OrderIte__3214EC071D559B71] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QA]    Script Date: 2024/4/8 下午 05:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QA](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[Account] [varchar](20) NOT NULL,
	[Content] [varchar](200) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Reply] [varchar](200) NULL,
	[ReplyTime] [datetime] NULL,
 CONSTRAINT [PK__QA__3214EC07EFE38943] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1234', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'1234@gmail.com', N'0909333333', N'mV7xPF7HZy', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1470', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', N'Uaa536Rl7z', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1471', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', N'yg8iQXF1ck', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1472', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', N'DGBGFfNHTn', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1473', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', N'Hw6eUAECr6', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1474', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', NULL, 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1479', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', N'F5Du5hXceE', 0)
INSERT [dbo].[Account] ([Account], [Password], [Name], [Email], [Cellphone], [AuthCode], [IsAdmin]) VALUES (N'1487', N'tJJwVjuX4cy6WvwL1gIpEXrTX3AW+J3LaNGVqtrqCBU=', N'User1234', N'lu39791064@gmail.com', N'0909333333', NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([Id], [Account], [ItemId], [ItemNum], [AddTime], [FormatId]) VALUES (3, N'1474', 6, 1, CAST(N'2024-04-08T17:30:40.397' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[Format] ON 

INSERT [dbo].[Format] ([FormatId], [Brand], [Color], [ItemPrice], [Store], [Space], [ItemId]) VALUES (0, N'oppo', N'blue', 400, 20, N'256G', 6)
INSERT [dbo].[Format] ([FormatId], [Brand], [Color], [ItemPrice], [Store], [Space], [ItemId]) VALUES (1, N'apple', N'yellow', 300, 30, N'128G', 8)
INSERT [dbo].[Format] ([FormatId], [Brand], [Color], [ItemPrice], [Store], [Space], [ItemId]) VALUES (2, N'oppo', N'black', 500, 20, N'256G', 6)
INSERT [dbo].[Format] ([FormatId], [Brand], [Color], [ItemPrice], [Store], [Space], [ItemId]) VALUES (3, N'apple', N'red', 500, 30, N'128G', 8)
SET IDENTITY_INSERT [dbo].[Format] OFF
GO
SET IDENTITY_INSERT [dbo].[Img] ON 

INSERT [dbo].[Img] ([Id], [FormatId], [ItemImg]) VALUES (0, 0, N'0.jpg')
INSERT [dbo].[Img] ([Id], [FormatId], [ItemImg]) VALUES (1, 1, N'1.jpg')
INSERT [dbo].[Img] ([Id], [FormatId], [ItemImg]) VALUES (2, 2, N'2.jpg')
INSERT [dbo].[Img] ([Id], [FormatId], [ItemImg]) VALUES (3, 3, N'3.jpg')
SET IDENTITY_INSERT [dbo].[Img] OFF
GO
SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([ItemId], [ItemName], [Instruction], [IsAvailable]) VALUES (6, N'123', N'123', 0)
INSERT [dbo].[Item] ([ItemId], [ItemName], [Instruction], [IsAvailable]) VALUES (8, N'423', N'456', 0)
SET IDENTITY_INSERT [dbo].[Item] OFF
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__IsAdmin__36B12243]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[Format] ADD  CONSTRAINT [DF__Format__store__398D8EEE]  DEFAULT ((0)) FOR [Store]
GO
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [DF__Item__IsAvailabl__3C69FB99]  DEFAULT ((0)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK__Cart__Account__403A8C7D] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Account])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK__Cart__Account__403A8C7D]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK__Cart__FormatId__0697FACD] FOREIGN KEY([FormatId])
REFERENCES [dbo].[Format] ([FormatId])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK__Cart__FormatId__0697FACD]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK__Cart__ItemId__412EB0B6] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK__Cart__ItemId__412EB0B6]
GO
ALTER TABLE [dbo].[Format]  WITH CHECK ADD  CONSTRAINT [FK__Format__ItemId__607251E5] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[Format] CHECK CONSTRAINT [FK__Format__ItemId__607251E5]
GO
ALTER TABLE [dbo].[Img]  WITH CHECK ADD  CONSTRAINT [FK__Img__FormatId__6FE99F9F] FOREIGN KEY([FormatId])
REFERENCES [dbo].[Format] ([FormatId])
GO
ALTER TABLE [dbo].[Img] CHECK CONSTRAINT [FK__Img__FormatId__6FE99F9F]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__Account__440B1D61] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Account])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__Account__440B1D61]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Order__46E78A0C] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Order__46E78A0C]
GO
ALTER TABLE [dbo].[QA]  WITH CHECK ADD  CONSTRAINT [FK__QA__Account__49C3F6B7] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Account])
GO
ALTER TABLE [dbo].[QA] CHECK CONSTRAINT [FK__QA__Account__49C3F6B7]
GO
ALTER TABLE [dbo].[QA]  WITH CHECK ADD  CONSTRAINT [FK__QA__ItemId__4AB81AF0] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[QA] CHECK CONSTRAINT [FK__QA__ItemId__4AB81AF0]
GO
USE [master]
GO
ALTER DATABASE [Phone] SET  READ_WRITE 
GO
