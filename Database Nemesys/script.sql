USE [master]
GO
/****** Object:  Database [cis2055-nemesys]    Script Date: 16/04/2021 14:58:04 ******/
CREATE DATABASE [cis2055-nemesys]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'cis2055-nemesys', FILENAME = N'C:\Users\liamc\cis2055-nemesys.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'cis2055-nemesys_log', FILENAME = N'C:\Users\liamc\cis2055-nemesys_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [cis2055-nemesys] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cis2055-nemesys].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [cis2055-nemesys] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET ARITHABORT OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [cis2055-nemesys] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [cis2055-nemesys] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET  DISABLE_BROKER 
GO
ALTER DATABASE [cis2055-nemesys] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [cis2055-nemesys] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [cis2055-nemesys] SET  MULTI_USER 
GO
ALTER DATABASE [cis2055-nemesys] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [cis2055-nemesys] SET DB_CHAINING OFF 
GO
ALTER DATABASE [cis2055-nemesys] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [cis2055-nemesys] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [cis2055-nemesys] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [cis2055-nemesys] SET QUERY_STORE = OFF
GO
USE [cis2055-nemesys]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [cis2055-nemesys]
GO
/****** Object:  Table [dbo].[Hazards]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hazards](
	[Harzard_ID] [int] IDENTITY(1,1) NOT NULL,
	[HazardType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Hazards] PRIMARY KEY CLUSTERED 
(
	[Harzard_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Investigations]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Investigations](
	[Investigation_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Report_ID] [int] NOT NULL,
	[Status_ID] [int] NOT NULL,
 CONSTRAINT [PK_Investigations] PRIMARY KEY CLUSTERED 
(
	[Investigation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogInvestigations]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogInvestigations](
	[LogInvestigation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Investigation_ID] [int] NOT NULL,
	[Description] [ntext] NOT NULL,
	[DateOfAction] [date] NOT NULL,
 CONSTRAINT [PK_LogInvestigations] PRIMARY KEY CLUSTERED 
(
	[LogInvestigation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pinpoints]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pinpoints](
	[Pinpoint_ID] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_Pinpoints] PRIMARY KEY CLUSTERED 
(
	[Pinpoint_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportHazard]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportHazard](
	[Hazard_ID] [int] IDENTITY(1,1) NOT NULL,
	[Report_ID] [int] NOT NULL,
 CONSTRAINT [PK_ReportHazard] PRIMARY KEY CLUSTERED 
(
	[Hazard_ID] ASC,
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[Report_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Pinpoint_ID] [int] NOT NULL,
	[DateOfReport] [date] NOT NULL,
	[DateTimeHazard] [date] NOT NULL,
	[Description] [ntext] NOT NULL,
	[Upvotes] [int] NOT NULL,
	[Image] [image] NULL,
 CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
(
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Role_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusCategory]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusCategory](
	[Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[StatusType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Status Category] PRIMARY KEY CLUSTERED 
(
	[Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 16/04/2021 14:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Role_ID] [int] NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Telephone] [int] NULL,
	[Password] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Reports] ADD  CONSTRAINT [DF_Reports_Upvotes]  DEFAULT ((0)) FOR [Upvotes]
GO
ALTER TABLE [dbo].[Investigations]  WITH CHECK ADD  CONSTRAINT [FK_Investigations_Reports] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([Report_ID])
GO
ALTER TABLE [dbo].[Investigations] CHECK CONSTRAINT [FK_Investigations_Reports]
GO
ALTER TABLE [dbo].[Investigations]  WITH CHECK ADD  CONSTRAINT [FK_Investigations_StatusCategory] FOREIGN KEY([Status_ID])
REFERENCES [dbo].[StatusCategory] ([Status_ID])
GO
ALTER TABLE [dbo].[Investigations] CHECK CONSTRAINT [FK_Investigations_StatusCategory]
GO
ALTER TABLE [dbo].[Investigations]  WITH CHECK ADD  CONSTRAINT [FK_Investigations_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Investigations] CHECK CONSTRAINT [FK_Investigations_Users]
GO
ALTER TABLE [dbo].[LogInvestigations]  WITH CHECK ADD  CONSTRAINT [FK_LogInvestigations_Investigations] FOREIGN KEY([Investigation_ID])
REFERENCES [dbo].[Investigations] ([Investigation_ID])
GO
ALTER TABLE [dbo].[LogInvestigations] CHECK CONSTRAINT [FK_LogInvestigations_Investigations]
GO
ALTER TABLE [dbo].[ReportHazard]  WITH CHECK ADD  CONSTRAINT [FK_ReportHazard_Hazard] FOREIGN KEY([Hazard_ID])
REFERENCES [dbo].[Hazards] ([Harzard_ID])
GO
ALTER TABLE [dbo].[ReportHazard] CHECK CONSTRAINT [FK_ReportHazard_Hazard]
GO
ALTER TABLE [dbo].[ReportHazard]  WITH CHECK ADD  CONSTRAINT [FK_ReportHazard_Reports] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([Report_ID])
GO
ALTER TABLE [dbo].[ReportHazard] CHECK CONSTRAINT [FK_ReportHazard_Reports]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_Pinpoints] FOREIGN KEY([Pinpoint_ID])
REFERENCES [dbo].[Pinpoints] ([Pinpoint_ID])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_Pinpoints]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([Role_ID])
REFERENCES [dbo].[Roles] ([Role_ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [cis2055-nemesys] SET  READ_WRITE 
GO
