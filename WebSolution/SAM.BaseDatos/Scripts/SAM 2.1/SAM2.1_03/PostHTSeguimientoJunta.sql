


update ModuloSeguimientoJunta
set NombreIngles = 'RT Test(Post HT)' where ModuloSeguimientoJuntaID=11

update ModuloSeguimientoJunta
set NombreIngles = 'PT Test(Post HT)' where ModuloSeguimientoJuntaID=12

select * from ModuloSeguimientoJunta

update TipoPrueba 
set NombreIngles = 'RT (POST-HT)' where TipoPruebaID=5


update TipoPrueba 
set NombreIngles = 'PT (POST-HT)' where TipoPruebaID=6

select * from TipoPrueba