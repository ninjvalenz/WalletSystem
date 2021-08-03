CREATE TABLE [dbo].[QueueStatusType] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Type] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_QueueStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

