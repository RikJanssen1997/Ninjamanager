CREATE TABLE [dbo].[Equipment]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [GoldValue] INT NOT NULL, 
    [Strenght] INT NOT NULL, 
    [Intelligence] INT NOT NULL, 
    [Agility] INT NOT NULL, 
    [Category] NVARCHAR(50) NOT NULL, 
    [Picture] IMAGE NOT NULL, 
    CONSTRAINT [FK_Category] FOREIGN KEY (Category) REFERENCES Category (Category)
)
