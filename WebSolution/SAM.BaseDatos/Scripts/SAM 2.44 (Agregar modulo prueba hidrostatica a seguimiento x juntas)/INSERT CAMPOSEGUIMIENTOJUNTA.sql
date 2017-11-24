GO
DECLARE @IdMosulojunta INT 
DECLARE @IdModuloSpool INT
SET @IdModuloSpool = (Select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'Prueba Hidrost�tica')
SET @IdMosulojunta = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Prueba Hidrost�tica')
--SELECT @IdMosulojunta

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
		   ,[DataFormat]
           ,[AnchoUI]
		   ,[CssColumnaUI])
		SELECT @IdMosulojunta
				, s.Nombre
				, s.NombreIngles
				, s.OrdenUI
				, s.NombreControlUI
				, s.NombreColumnaSp
				, s.DataFormat
				, s.AnchoUI
				, ''
		FROM CampoSeguimientoSpool s
		where s.ModuloSeguimientoSpoolID = @IdModuloSpool

UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.DateTime' where Nombre = 'Fecha Requisici�n' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.String' where Nombre = 'N�mero Requisici�n' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.DateTime' where Nombre = 'Fecha Prueba' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.DateTime' where Nombre = 'Fecha Reporte' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.String' where Nombre = 'N�mero Reporte' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.Int32' where Nombre = 'Hoja' AND ModuloSeguimientoJuntaID = @IdMosulojunta
UPDATE CampoSeguimientoJunta set TipoDeDato= 'System.Boolean' where Nombre = 'Aprobado' AND ModuloSeguimientoJuntaID = @IdMosulojunta
GO