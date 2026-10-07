IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clients] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [ContactDetails] nvarchar(500) NOT NULL,
    [Region] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);

CREATE TABLE [Contracts] (
    [Id] int NOT NULL IDENTITY,
    [ClientId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [ServiceLevel] nvarchar(100) NOT NULL,
    [SignedAgreementPath] nvarchar(500) NULL,
    [SignedAgreementFileName] nvarchar(255) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Contracts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contracts_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ServiceRequests] (
    [Id] int NOT NULL IDENTITY,
    [ContractId] int NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [CostUsd] decimal(18,2) NOT NULL,
    [CostZar] decimal(18,2) NOT NULL,
    [ExchangeRateUsed] decimal(18,4) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ServiceRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ServiceRequests_Contracts_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [Contracts] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactDetails', N'CreatedAt', N'Name', N'Region') AND [object_id] = OBJECT_ID(N'[Clients]'))
    SET IDENTITY_INSERT [Clients] ON;
INSERT INTO [Clients] ([Id], [ContactDetails], [CreatedAt], [Name], [Region])
VALUES (1, N'info@oceanicfreight.com | +27 21 555 0100', '2025-01-01T00:00:00.0000000', N'Oceanic Freight Ltd', N'Africa'),
(2, N'ops@eurocargo.de | +49 30 555 0200', '2025-01-01T00:00:00.0000000', N'EuroCargo GmbH', N'Europe'),
(3, N'contact@pacificrim.com | +65 6555 0300', '2025-01-01T00:00:00.0000000', N'Pacific Rim Shipping', N'Asia-Pacific');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactDetails', N'CreatedAt', N'Name', N'Region') AND [object_id] = OBJECT_ID(N'[Clients]'))
    SET IDENTITY_INSERT [Clients] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientId', N'CreatedAt', N'EndDate', N'ServiceLevel', N'SignedAgreementFileName', N'SignedAgreementPath', N'StartDate', N'Status') AND [object_id] = OBJECT_ID(N'[Contracts]'))
    SET IDENTITY_INSERT [Contracts] ON;
INSERT INTO [Contracts] ([Id], [ClientId], [CreatedAt], [EndDate], [ServiceLevel], [SignedAgreementFileName], [SignedAgreementPath], [StartDate], [Status])
VALUES (1, 1, '2025-01-01T00:00:00.0000000', '2026-01-01T00:00:00.0000000', N'Premium', NULL, NULL, '2025-01-01T00:00:00.0000000', N'Active'),
(2, 2, '2024-06-01T00:00:00.0000000', '2025-06-01T00:00:00.0000000', N'Standard', NULL, NULL, '2024-06-01T00:00:00.0000000', N'Expired'),
(3, 3, '2025-03-01T00:00:00.0000000', '2027-03-01T00:00:00.0000000', N'Enterprise', NULL, NULL, '2025-03-01T00:00:00.0000000', N'Draft');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientId', N'CreatedAt', N'EndDate', N'ServiceLevel', N'SignedAgreementFileName', N'SignedAgreementPath', N'StartDate', N'Status') AND [object_id] = OBJECT_ID(N'[Contracts]'))
    SET IDENTITY_INSERT [Contracts] OFF;

CREATE INDEX [IX_Contracts_ClientId] ON [Contracts] ([ClientId]);

CREATE INDEX [IX_ServiceRequests_ContractId] ON [ServiceRequests] ([ContractId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260607023757_SeedData', N'10.0.8');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260607194715_TestingFeatures1', N'10.0.8');

COMMIT;
GO

SELECT * FROM Clients;
SELECT * FROM Contracts;