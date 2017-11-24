
ALTER TABLE [dbo].[Corte] DROP CONSTRAINT [FK__Corte__CortadorI__196C6ECD]

ALTER TABLE [dbo].[Corte] ALTER COLUMN CortadorID INT NULL

ALTER TABLE [dbo].[Despacho] DROP CONSTRAINT [FK__Despacho__Despac__18784A94]

ALTER TABLE [dbo].[Despacho] ALTER COLUMN DespachadorID INT NULL