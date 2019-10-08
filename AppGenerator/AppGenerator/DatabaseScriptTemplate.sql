 CREATE DATABASE [Garage]
 GO

 USE [Garage]
 GO

 	CREATE TABLE [dbo].[Product](
		[ID] int IDENTITY(1,1) NOT NULL,
		[TypeID] int,
		[Name] varchar(100),
		[Price] decimal(18, 2),
		[Description] varchar(100),
		[Image] varchar(1000),
		CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)
) ON [PRIMARY]
	GO

 	CREATE TABLE [dbo].[ProductType](
		[ID] int IDENTITY(1,1) NOT NULL,
		[Name] varchar(100),
		CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)
) ON [PRIMARY]
	GO

 	CREATE TABLE [dbo].[Purchase](
		[ID] int IDENTITY(1,1) NOT NULL,
		[CustomerID] int,
		[ProductID] int,
		[Amount] int,
		[Date] varchar(100),
		[IsInCart] varchar(100),
		CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)
) ON [PRIMARY]
	GO

 
 	ALTER TABLE [dbo].[Product] WITH CHECK ADD CONSTRAINT [FK_ProductProductType] FOREIGN KEY([TypeID])
	REFERENCES [dbo].[ProductType] ([ID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

 	ALTER TABLE [dbo].[Purchase] WITH CHECK ADD CONSTRAINT [FK_PurchaseCustomer] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[Customer] ([ID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

 	ALTER TABLE [dbo].[Purchase] WITH CHECK ADD CONSTRAINT [FK_PurchaseProduct] FOREIGN KEY([ProductID])
	REFERENCES [dbo].[Product] ([ID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

 