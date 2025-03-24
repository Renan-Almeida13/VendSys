-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'NayaxVendSys')
BEGIN
    CREATE DATABASE NayaxVendSys;
END
GO

USE NayaxVendSys;
GO

-- Create DexMeter table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DexMeter]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DexMeter](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Machine] [varchar](1) NOT NULL,
        [DexDateTime] [datetime] NOT NULL,
        [MachineSerialNumber] [varchar](50) NOT NULL,
        [ValueOfPaidVends] [decimal](18, 2) NOT NULL,
        CONSTRAINT [PK_DexMeter] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create DexLaneMeter table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DexLaneMeter]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DexLaneMeter](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DexMeterId] [int] NOT NULL,
        [ProductIdentifier] [varchar](50) NOT NULL,
        [Price] [decimal](18, 2) NOT NULL,
        [NumberOfVends] [int] NOT NULL,
        [ValueOfPaidSales] [decimal](18, 2) NOT NULL,
        CONSTRAINT [PK_DexLaneMeter] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_DexLaneMeter_DexMeter] FOREIGN KEY([DexMeterId]) 
            REFERENCES [dbo].[DexMeter] ([Id]) ON DELETE CASCADE
    );
END
GO

-- Create SaveDexMeter stored procedure
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SaveDexMeter')
    DROP PROCEDURE SaveDexMeter
GO

CREATE PROCEDURE [dbo].[SaveDexMeter]
    @Machine varchar(1),
    @DexDateTime datetime,
    @MachineSerialNumber varchar(50),
    @ValueOfPaidVends decimal(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id int;

    INSERT INTO [dbo].[DexMeter]
        ([Machine], [DexDateTime], [MachineSerialNumber], [ValueOfPaidVends])
    VALUES
        (@Machine, @DexDateTime, @MachineSerialNumber, @ValueOfPaidVends);

    SET @Id = SCOPE_IDENTITY();
    
    SELECT @Id as Id;
    RETURN @Id;
END
GO

-- Create SaveDexLaneMeter stored procedure
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SaveDexLaneMeter')
    DROP PROCEDURE SaveDexLaneMeter
GO

CREATE PROCEDURE [dbo].[SaveDexLaneMeter]
    @DexMeterId int,
    @ProductIdentifier varchar(50),
    @Price decimal(18, 2),
    @NumberOfVends int,
    @ValueOfPaidSales decimal(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[DexLaneMeter]
        ([DexMeterId], [ProductIdentifier], [Price], [NumberOfVends], [ValueOfPaidSales])
    VALUES
        (@DexMeterId, @ProductIdentifier, @Price, @NumberOfVends, @ValueOfPaidSales);
END
GO 