ALTER TABLE JuntaSpool
ADD RequierePwht BIT  NULL

begin tran
	UPDATE  js
	SET js.RequierePwht = s.RequierePwht 
	FROM JuntaSpool js
	INNER JOIN Spool s 
	ON js.SpoolID = s.SpoolID 
	where js.RequierePwht IS NULL
rollback


ALTER TABLE JuntaSpool
ALTER COLUMN RequierePwht BIT NOT NULL


ALTER TABLE JuntaSpoolPendiente
ADD RequierePwht BIT  NULL


begin tran
	UPDATE  jsp
	SET jsp.RequierePwht = sp.RequierePwht 
	FROM JuntaSpoolPendiente jsp
	INNER JOIN SpoolPendiente sp 
	ON jsp.SpoolPendienteID = sp.SpoolPendienteID 
	where jsp.RequierePwht IS NULL
rollback

ALTER TABLE JuntaSpoolPendiente
ALTER COLUMN RequierePwht   BIT NOT NULL  



