CREATE TABLE [dbo].[UserWalletTransactionQueue] (
    [QueueId]                 BIGINT          NOT NULL,
    [UserWalletAccountNumber] BIGINT          NOT NULL,
    [FromToAccountNumber]     BIGINT          NULL,
    [TransactionTypeId]       INT             NOT NULL,
    [Amount]                  DECIMAL (18, 8) NOT NULL,
    [CreatedDate]             DATETIME        NOT NULL,
    [QueueStatusId]           INT             NOT NULL,
    [Message]                 VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_UserWalletTransactionQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC),
    CONSTRAINT [FK_UserWalletTransactionQueue_QueueStatusType] FOREIGN KEY ([QueueStatusId]) REFERENCES [dbo].[QueueStatusType] ([Id]),
    CONSTRAINT [FK_UserWalletTransactionQueue_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionType] ([Id]),
    CONSTRAINT [FK_UserWalletTransactionQueue_UserWalletAccount] FOREIGN KEY ([UserWalletAccountNumber]) REFERENCES [dbo].[UserWalletAccount] ([AccountNumber])
);



