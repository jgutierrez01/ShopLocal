declare  @PatioId int
select @PatioId= PatioID from patio where nombre like 'Altamira'

insert into cuadrante values (@PatioId,'Cargado',null,GETDATE(), null)

