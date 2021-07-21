CREATE TABLE [dbo].[UserSecurity] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Login]       VARCHAR (100)    NOT NULL,
    [Password]    VARCHAR (50)     NOT NULL,
    [CreatedDate] DATETIME         NULL,
    CONSTRAINT [PK_UserSecurity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

