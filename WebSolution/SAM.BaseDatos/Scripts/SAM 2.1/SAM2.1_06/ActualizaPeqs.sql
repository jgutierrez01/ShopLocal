
ALTER TABLE Peq
ADD ProyectoID int

UPDATE Peq
SET ProyectoID = 46

ALTER TABLE Peq
ALTER COLUMN ProyectoID INT NOT NULL

ALTER TABLE Peq 
ADD CONSTRAINT FK_Peq_ProyectoID FOREIGN KEY (ProyectoID) REFERENCES Proyecto(ProyectoID) 
