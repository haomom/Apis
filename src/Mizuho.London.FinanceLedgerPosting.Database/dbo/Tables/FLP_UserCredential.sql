CREATE TABLE [dbo].[FLP_UserCredential]
(
	[UserCredentialId]	INT			IDENTITY(1, 1) NOT NULL,
	[UserName]			NVARCHAR(30)	NOT NULL,
	[Branch]			NVARCHAR(10)	NOT NULL,
	[GBaseUserId]		NVARCHAR(20)	NOT NULL,
	[GBaseEmployeeId]	NVARCHAR(20)	NOT NULL,
	[GBasePassword]		NVARCHAR(1000)	NOT NULL,
	[ExpiryDate]		DATETIME		NOT NULL,
	[ModifiedBy]		NVARCHAR(30)	NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedOn]	DATETIME		NOT NULL,
	CONSTRAINT [PK_FLP_UserCredential] PRIMARY KEY CLUSTERED ([UserCredentialId] ASC)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FLP_UserCredential_UserName_Branch]
    ON [dbo].[FLP_UserCredential]([UserName] ASC, [Branch] ASC);
