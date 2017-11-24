alter table ItemCode
add Diametro1 decimal(7,4) null

alter table ItemCode
add Diametro2 decimal(7,4) null


alter table ItemCode
add FamiliaAceroID int null

alter table ItemCode
add constraint FK_ItemCode_FamiliaAcero foreign key (FamiliaAceroID) references FamiliaAcero (FamiliaAceroID)