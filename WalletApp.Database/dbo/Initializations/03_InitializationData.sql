USE [WalletApp]
GO
SET IDENTITY_INSERT [dbo].[TransactionType] ON 

INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (1, N'Deposit')
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (2, N'Withdraw')
INSERT [dbo].[TransactionType] ([Id], [Type]) VALUES (3, N'Transfer')
SET IDENTITY_INSERT [dbo].[TransactionType] OFF
GO
