USE [WalletApp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserWalletTransaction]') AND type in (N'U'))
ALTER TABLE [dbo].[UserWalletTransaction] DROP CONSTRAINT IF EXISTS [FK_UserWalletTransaction_UserWalletTransaction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserWalletTransaction]') AND type in (N'U'))
ALTER TABLE [dbo].[UserWalletTransaction] DROP CONSTRAINT IF EXISTS [FK_UserWalletTransaction_TransactionType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserWalletAccount]') AND type in (N'U'))
ALTER TABLE [dbo].[UserWalletAccount] DROP CONSTRAINT IF EXISTS [FK_UserWalletAccount_UserSecurity]
GO
/****** Object:  Table [dbo].[UserWalletTransaction]    Script Date: 7/23/2021 1:06:25 AM ******/
DROP TABLE IF EXISTS [dbo].[UserWalletTransaction]
GO
/****** Object:  Table [dbo].[UserWalletAccount]    Script Date: 7/23/2021 1:06:25 AM ******/
DROP TABLE IF EXISTS [dbo].[UserWalletAccount]
GO
/****** Object:  Table [dbo].[UserSecurity]    Script Date: 7/23/2021 1:06:25 AM ******/
DROP TABLE IF EXISTS [dbo].[UserSecurity]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 7/23/2021 1:06:25 AM ******/
DROP TABLE IF EXISTS [dbo].[TransactionType]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 7/23/2021 1:06:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](100) NOT NULL,
 CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSecurity]    Script Date: 7/23/2021 1:06:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSecurity](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_UserSecurity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserWalletAccount]    Script Date: 7/23/2021 1:06:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserWalletAccount](
	[AccountNumber] [bigint] NOT NULL,
	[UserSecurityId] [uniqueidentifier] NOT NULL,
	[RegisteredDate] [datetime] NOT NULL,
	[Balance] [decimal](18, 8) NOT NULL,
 CONSTRAINT [PK_UserWalletAccount] PRIMARY KEY CLUSTERED 
(
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserWalletTransaction]    Script Date: 7/23/2021 1:06:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserWalletTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserWalletAccountNumber] [bigint] NOT NULL,
	[FromToAccountNumber] [bigint] NULL,
	[TransactionTypeId] [int] NOT NULL,
	[Amount] [decimal](18, 8) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EndBalance] [decimal](18, 8) NOT NULL,
 CONSTRAINT [PK_UserWalletTransaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserWalletAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserWalletAccount_UserSecurity] FOREIGN KEY([UserSecurityId])
REFERENCES [dbo].[UserSecurity] ([Id])
GO
ALTER TABLE [dbo].[UserWalletAccount] CHECK CONSTRAINT [FK_UserWalletAccount_UserSecurity]
GO
ALTER TABLE [dbo].[UserWalletTransaction]  WITH CHECK ADD  CONSTRAINT [FK_UserWalletTransaction_TransactionType] FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransactionType] ([Id])
GO
ALTER TABLE [dbo].[UserWalletTransaction] CHECK CONSTRAINT [FK_UserWalletTransaction_TransactionType]
GO
ALTER TABLE [dbo].[UserWalletTransaction]  WITH CHECK ADD  CONSTRAINT [FK_UserWalletTransaction_UserWalletTransaction] FOREIGN KEY([UserWalletAccountNumber])
REFERENCES [dbo].[UserWalletAccount] ([AccountNumber])
GO
ALTER TABLE [dbo].[UserWalletTransaction] CHECK CONSTRAINT [FK_UserWalletTransaction_UserWalletTransaction]
GO
