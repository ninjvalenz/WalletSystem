CREATE TABLE [dbo].[UserSecurityQueue] (
    [QueueId]                BIGINT           NOT NULL,
    [Login]                  VARCHAR (100)    NOT NULL,
    [Password]               VARCHAR (50)     NOT NULL,
    [CreatedDate]            DATETIME         NOT NULL,
    [QueueStatusId]          INT              NOT NULL,
    [Message]                VARCHAR (MAX)    NULL,
    [RegisteredUserId]       UNIQUEIDENTIFIER NULL,
    [RegisteredWalletAcctNo] BIGINT           NULL,
    CONSTRAINT [PK_TransactionDetailUserSecurity] PRIMARY KEY CLUSTERED ([QueueId] ASC),
    CONSTRAINT [FK_UserSecurityQueue_QueueStatusType] FOREIGN KEY ([QueueStatusId]) REFERENCES [dbo].[QueueStatusType] ([Id])
);



