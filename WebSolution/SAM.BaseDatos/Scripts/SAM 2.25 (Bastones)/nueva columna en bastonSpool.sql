alter table BastonSpool
add TallerID int null
GO

alter table BastonSpool
add foreign key (TallerID) references Taller (TallerID)
GO