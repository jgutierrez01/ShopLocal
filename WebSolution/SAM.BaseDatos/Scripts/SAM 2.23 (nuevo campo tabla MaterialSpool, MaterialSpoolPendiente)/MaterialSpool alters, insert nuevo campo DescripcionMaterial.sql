-- SE AGREGA NUEVA COLUMNA
alter table MaterialSpool
add DescripcionMaterial NVARCHAR(150) NULL
GO

-- SE ACTUALIZAN LOS VALORES DEL CAMPO
update MaterialSpool
set DescripcionMaterial = ic.DescripcionEspanol
from MaterialSpool ms
inner join ItemCode ic on ms.ItemCodeID = ic.ItemCodeID
