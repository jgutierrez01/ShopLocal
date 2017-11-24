alter table BastonSpool
drop constraint FK_BastonSpool_Estacion

alter table BastonSpool
alter column EstacionID INT NULL

alter table BastonSpool
add constraint FK_BastonSpool_Estacion FOREIGN KEY (EstacionID) REFERENCES Estacion (EstacionID)