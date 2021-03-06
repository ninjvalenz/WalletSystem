USE [WalletApp]
GO
SET IDENTITY_INSERT [dbo].[QueueStatusType] ON 
GO
INSERT [dbo].[QueueStatusType] ([Id], [Type]) VALUES (1, N'New')
GO
INSERT [dbo].[QueueStatusType] ([Id], [Type]) VALUES (2, N'Processing')
GO
INSERT [dbo].[QueueStatusType] ([Id], [Type]) VALUES (3, N'Success')
GO
INSERT [dbo].[QueueStatusType] ([Id], [Type]) VALUES (4, N'Failed')
GO
SET IDENTITY_INSERT [dbo].[QueueStatusType] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionType] ON 
GO
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (1, N'Deposit')
GO
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (2, N'Withdraw')
GO
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (3, N'Transfer')
GO
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (4, N'Refund')
GO
SET IDENTITY_INSERT [dbo].[TransactionType] OFF
GO
