declare @moduloID INT
SET @moduloID= (select ModuloID from modulo where Nombre ='Work Status')

insert into Permiso(ModuloID
					,Nombre
					,NombreIngles
					,Descripcion
					,UsuarioModifica
					,FechaModificacion					
					)
		VALUES(
			@moduloID
			,'Transferencia Spool'
			,'Spool Transfer'
			,NULL
			,NULL
			,NULL
		)

DECLARE @ID INT
SELECT @ID=@@IDENTITY



INSERT INTO Pagina (PermisoID,
					Url,
					UsuarioModifica,
					FechaModificacion
					)
		VALUES(@ID,
			'/WorkStatus/LstTransferenciaSpool.aspx',
			NULL,
			NULL
		)
	