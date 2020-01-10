CREATE TABLE [dbo].[Inventory]
(
	[NinjaId] INT NOT NULL, 
    [EquipmentId] INT NOT NULL, 
    CONSTRAINT [FK_NinjaId] FOREIGN KEY (NinjaId) REFERENCES Ninja (Id) ON DELETE CASCADE, 
    CONSTRAINT [FK_EquipmentId] FOREIGN KEY (EquipmentId) REFERENCES Equipment (Id) ON DELETE CASCADE, 
    CONSTRAINT [PK_Inventory] PRIMARY KEY (NinjaId, EquipmentId)
)
