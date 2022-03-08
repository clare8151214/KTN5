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
    [cartId] int IDENTITY(1,1) NOT NULL,
	[oName] nvarchar(50) NOT NULL,
    [uId] int NOT NULL,
    [oId] int NOT NULL,
	[oIntro] nvarchar(max)  NULL,
	[oQty] int NULL,
	PRIMARY KEY CLUSTERED ([cartId] ASC)
);
GO

CREATE TABLE [dbo].[Order] (
    [orderId] int IDENTITY(1,1) NOT NULL,
	[hName] nvarchar(50) NOT NULL,
    [uId] int NOT NULL,
    [hPhone] nvarchar(50)  NULL,
	[hEamil] nvarchar(50)  NULL,
	[hCarrier] nvarchar(max) NULL,
	[created_at] datetime  DEFAULT GETDATE(),
	PRIMARY KEY CLUSTERED ([orderId] ASC)
);
GO

CREATE TABLE [dbo].[OrderDetail] (
    [odId] int IDENTITY(1,1) NOT NULL,
	[oName] nvarchar(50) NOT NULL,
    [orderId] int NOT NULL,
    [oQty] int NULL,
	
	PRIMARY KEY CLUSTERED ([odId] ASC)
);
GO

CREATE TABLE [dbo].[Restaurant] (
    [rId] int IDENTITY(1,1) NOT NULL,
	[rName] nvarchar(255) NULL,
    [rAddress] nvarchar(255) NULL,
    [rPhone] nvarchar(255) NULL,
	[startTime] time(0) NULL,
	[endTime] time(0) NULL,
	PRIMARY KEY CLUSTERED ([rId] ASC)
);
GO

CREATE TABLE [dbo].[Fund] (
    [fId] int IDENTITY(1,1) NOT NULL,
	[cId] int NULL,
	[fText] nvarchar(max) NULL,
    [targetMoney] money NULL,
    [accMoney] money NULL,
	[startTime] datetime NULL,
	[endTime] datetime NULL,
	[trueName] nvarchar(50) NULL,
	[fPhone] nvarchar(50) NULL,
	[fEmail] nvarchar(50) NULL,

	PRIMARY KEY CLUSTERED ([fId] ASC)
);
GO



CREATE TABLE [dbo].[Solution] (
    [sId] int IDENTITY(1,1) NOT NULL,
	[fId] int NOT NULL,
    [sMoney] money NULL,
    [intro] nvarchar(max) NOT NULL,    
	PRIMARY KEY CLUSTERED ([sId] ASC)
);
GO

CREATE TABLE [dbo].[Sponsor] (
    [uId] int  NOT NULL,
	[sId] int  NOT NULL,
);
GO

CREATE TABLE [dbo].[Invoice] (
    [iId] int IDENTITY(1,1) NOT NULL,
	[cId] int NOT NULL,
	[targetNumber] bigint NOT NULL,
	[accNumber] bigint NOT NULL, 
	PRIMARY KEY CLUSTERED ([iId] ASC)
);
GO

CREATE TABLE [dbo].[Donate] (
    [dId] int IDENTITY(1,1) NOT NULL,
	[cId] int NOT NULL,
	[uId] int NULL,
	[type] nvarchar(50) NOT NULL,
	[money] money NOT NULL, 
	PRIMARY KEY CLUSTERED ([dId] ASC)
);
GO

CREATE TABLE [dbo].[BloodMap] (
    [bId] int IDENTITY(1,1) NOT NULL,
	[bName] nvarchar(100) NOT NULL,
	[bAddress] nvarchar(100) NULL,
	[bPhone] nvarchar(100) NULL,
	[lat] decimal(12,9) NOT NULL,
	[lng] decimal(12,9) NOT NULL,
	[startTime] time NULL,
	[endTime] time NULL,
	PRIMARY KEY CLUSTERED ([bId] ASC)
);
GO



--TRUNCATE TABLE [dbo].[User];
--TRUNCATE TABLE [dbo].[Charity_Member];
--TRUNCATE TABLE [dbo].[Object];

--TRUNCATE TABLE [dbo].[Fund];
--TRUNCATE TABLE [dbo].[Solution];
--TRUNCATE TABLE [dbo].[Order];
--TRUNCATE TABLE [dbo].[OrderDetail];
TRUNCATE TABLE [dbo].[Sponsor];