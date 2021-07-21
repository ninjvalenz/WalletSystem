CREATE TABLE [dbo].[TransactionType] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Type] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

