CREATE TABLE [dbo].[UserWalletTransaction] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [UserWalletAccountNumber] BIGINT          NOT NULL,
    [FromToAccountNumber]     BIGINT          NULL,
    [TransactionTypeId]       INT             NOT NULL,
    [Amount]                  DECIMAL (18, 8) NOT NULL,
    [TransactionDate]         DATETIME        NOT NULL,
    [EndBalance]              DECIMAL (18, 8) NOT NULL,
    CONSTRAINT [PK_UserWalletTransaction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserWalletTransaction_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionType] ([Id]),
    CONSTRAINT [FK_UserWalletTransaction_UserWalletTransaction] FOREIGN KEY ([UserWalletAccountNumber]) REFERENCES [dbo].[UserWalletAccount] ([AccountNumber])
);

