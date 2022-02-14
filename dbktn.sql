CREATE TABLE [dbo].[User] (
    [uId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50)  NULL,
    [account] nvarchar(254) NULL,
    [Password] nvarchar(max)  NULL,
    [IsEmailVerified] bit  NULL,
    [ActivationCode] uniqueidentifier  NULL,
    [ResetPasswordCode] nvarchar(100)  NULL,
    [created_at] datetime  DEFAULT GETDATE(),
	PRIMARY KEY CLUSTERED ([uId] ASC)
);
GO

CREATE TABLE [dbo].[Charity_Member] (
    [cId] int IDENTITY(1,1) NOT NULL,
    [c_name] nvarchar(50)  NULL,
    [c_address] nvarchar(max) NULL,
    [c_phone] nvarchar(50)  NULL,
    [c_pname] nvarchar(50)  NULL,
    [photo] nvarchar(100)  NULL,
	[heart_code] int null,
    [created_at] datetime  DEFAULT GETDATE(),

	PRIMARY KEY CLUSTERED ([cId] ASC)
);
GO