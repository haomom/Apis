CREATE TABLE [dbo].[SuspenseAccount] (
    [SuspenseAccountId] INT          IDENTITY (1, 1) NOT NULL,
    [Branch]            NVARCHAR (4) NOT NULL,
    [Currency]          NVARCHAR (4) NOT NULL,
    [AccountCode]       NVARCHAR (5) NOT NULL,
    [AccountNoPart1]    NVARCHAR (3) NOT NULL,
    [AccountNoPart2]    NVARCHAR (6) NOT NULL,
    CONSTRAINT [PK_dbo.SuspenseAccount] PRIMARY KEY CLUSTERED ([SuspenseAccountId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SuspenseAccount_Branch_Currency]
    ON [dbo].[SuspenseAccount]([Branch] ASC, [Currency] ASC);

