ALTER TABLE ProyectoDossier
ADD Trazabilidad bit

ALTER TABLE ProyectoDossier
ADD WPS bit

ALTER TABLE ProyectoDossier
ADD Embarque bit

update ProyectoDossier
set trazabilidad = 0, wps=0, embarque = 0

ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_Trazabilidad]  DEFAULT ((0)) FOR [Trazabilidad]
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_TWPS]  DEFAULT ((0)) FOR [WPS]
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_Embarque]  DEFAULT ((0)) FOR [Embarque]

ALTER TABLE ProyectoDossier
ALTER COLUMN Trazabilidad BIT NOT NULL
ALTER TABLE ProyectoDossier
ALTER COLUMN WPS BIT NOT NULL
ALTER TABLE ProyectoDossier
ALTER COLUMN Embarque BIT NOT NULL

