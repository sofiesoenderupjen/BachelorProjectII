CREATE TABLE [dbo].[IndlaegseddelInfo] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (255) NULL,
    [PharmaceuticalForm]            NVARCHAR (255) NULL,
    [Company] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
