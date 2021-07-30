CREATE TABLE [dbo].[UserWalletAccountQueue] (
    [QueueId]        BIGINT           NOT NULL,
    [UserSecurityId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [QueueStatusId]  INT              NOT NULL,
    [Message]        VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_UserWalletAccountQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC),
    CONSTRAINT [FK_UserWalletAccountQueue_QueueStatusType] FOREIGN KEY ([QueueStatusId]) REFERENCES [dbo].[QueueStatusType] ([Id]),
    CONSTRAINT [FK_UserWalletAccountQueue_UserSecurity] FOREIGN KEY ([UserSecurityId]) REFERENCES [dbo].[UserSecurity] ([Id])
);

