/*
   martes, 22 de noviembre de 201104:11:14 p.m.
   User: sam
   Server: Mtysvr22
   Database: SAM
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProyectoDossier ADD
	MTR bit NOT NULL CONSTRAINT DF_ProyectoDossier_MTR DEFAULT ((0))
GO
ALTER TABLE dbo.ProyectoDossier SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
