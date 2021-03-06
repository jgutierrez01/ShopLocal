USE [master]
GO

/****** HABILITAMOS LA OPCION EN EL SERVIDOR PARA PERMITIR xp_cmdshell  ******/

---- To allow advanced options to be changed.
EXEC SP_CONFIGURE 'show advanced options', 1
GO

-- To update the currently configured value for advanced options.
RECONFIGURE
GO

-- To enable the feature.
EXEC SP_CONFIGURE 'xp_cmdshell', 1
GO

-- To update the currently configured value for this feature.
RECONFIGURE
GO 

/****** DAMOS ACCESO A LA CUENTA SAM A LA TABLA MASTER  ******/

EXEC SP_GRANTDBACCESS sam
GO

/****** OTORGAMOS PRIVILEGIOS DE EJECUCION A XP_CMDSHELLACCESO A LA CUENTA SAM ******/

GRANT EXECUTE ON XP_CMDSHELL TO sam
GO

--sieena\sam.app_pool sería una cuenta de windows normal, hay que crearla antes.
EXEC SP_XP_CMDSHELL_PROXY_ACCOUNT 'SQLSTEELGO\samcmdshell','Temp0ral!'
GO

