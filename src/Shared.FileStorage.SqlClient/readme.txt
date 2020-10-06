Shared.FileStorage.SqlClient
Shared helper library for implementing file storage using SQL Server. Requires common/base library.


/**** BEGIN: CREATE Application Files table in a SQL database ****/
CREATE TABLE [dbo].[_ApplicationFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareName] [nvarchar](255) NOT NULL,
	[FolderName] [nvarchar](255) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[ByteData] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_ApplicationFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ApplicationFiles_ShareName_FolderName_FileName] ON [dbo].[_ApplicationFiles]
(
	[ShareName] ASC,
	[FolderName] ASC,
	[FileName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/**** END: CREATE Application Files table in a SQL database ****/



GitHub Repository
https://github.com/bradhuggins/Shared.FileStorage

About
This project was created and is maintained by Brad Huggins.

For more information, visit http://www.bradhuggins.com
