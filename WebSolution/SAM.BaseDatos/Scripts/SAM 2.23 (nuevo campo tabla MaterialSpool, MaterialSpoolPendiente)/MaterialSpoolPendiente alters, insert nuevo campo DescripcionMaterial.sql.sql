-- SE AGREGA NUEVA COLUMNA
alter table MaterialSpoolPendiente
add DescripcionMaterial NVARCHAR(150) NULL
GO

-- SE ACTUALIZAN LOS VALORES DEL CAMPO
update MaterialSpoolPendiente
set DescripcionMaterial = ic.DescripcionEspanol
from MaterialSpoolPendiente ms
inner join ItemCode ic on ms.ItemCodeID = ic.ItemCodeID