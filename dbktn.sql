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

CREATE TABLE [dbo].[Object] (
    [oId] int IDENTITY(1,1) NOT NULL,
	[oName] nvarchar(50) NOT NULL,
    [cId] int  NULL,
    [oPhoto] nvarchar(MAX)  NULL,
	[oNumber] nvarchar(max)  NULL,
	[oIntro] nvarchar(max) NULL,
    [type] nvarchar(50)  NULL,
       
	PRIMARY KEY CLUSTERED ([oId] ASC)
);
GO


CREATE TABLE [dbo].[ShoppingCart] (
    [hId] int IDENTITY(1,1) NOT NULL,
	[hName] nvarchar(50) NOT NULL,
    [uId] int NOT NULL,
    [hPhone] nvarchar(50)  NULL,
	[hEamil] nvarchar(max)  NULL,
	[hCarrier] nvarchar(max) NULL,       
	PRIMARY KEY CLUSTERED ([hId] ASC)
);
GO
