CREATE TABLE [dbo].[FLP_Branch]
(
	[BranchId]				INT			IDENTITY(1, 1) NOT NULL,
	[BranchName]			NVARCHAR(40)	NOT NULL,
	[BranchCode]			NVARCHAR(10)	NOT NULL,
	[BranchAccountCode]		NVARCHAR(10)	NOT NULL,
	CONSTRAINT [PK_FLP_Branch] PRIMARY KEY CLUSTERED ([BranchId] ASC)
)