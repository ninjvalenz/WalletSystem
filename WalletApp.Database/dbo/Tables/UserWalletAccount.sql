CREATE TABLE [dbo].[UserWalletAccount] (
    [AccountNumber]  BIGINT           NOT NULL,
    [UserSecurityId] UNIQUEIDENTIFIER NOT NULL,
    [RegisteredDate] DATETIME         NOT NULL,
    [Balance]        DECIMAL (18, 8)  NOT NULL,
    CONSTRAINT [PK_UserWalletAccount] PRIMARY KEY CLUSTERED ([AccountNumber] ASC),
    CONSTRAINT [FK_UserWalletAccount_UserSecurity] FOREIGN KEY ([UserSecurityId]) REFERENCES [dbo].[UserSecurity] ([Id])
);

