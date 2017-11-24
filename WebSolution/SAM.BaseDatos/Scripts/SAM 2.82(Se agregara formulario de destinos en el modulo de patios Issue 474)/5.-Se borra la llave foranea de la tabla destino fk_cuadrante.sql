
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Destino_CuadranteID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Destino]'))
ALTER TABLE [dbo].[Destino] DROP CONSTRAINT [FK_Destino_CuadranteID]
GO


