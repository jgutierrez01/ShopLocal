declare @ModuloEmbarqueIDSegSpool INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'Embarque')

insert into CampoSeguimientoSpool
            (
              ModuloSeguimientoSpoolID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI
            )
            VALUES
            (
             @ModuloEmbarqueIDSegSpool,
             'Nota 1',
             'Note 1',
             6,
             'litNota1',
             'Nota1',
             '',
             '',
             150
            )

insert into CampoSeguimientoSpool
            (
              ModuloSeguimientoSpoolID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI
            )
            VALUES
            (
             @ModuloEmbarqueIDSegSpool,
             'Nota 2',
             'Note 2',
             7,
             'litNota2',
             'Nota2',
             '',
             '',
             150
            )
            
insert into CampoSeguimientoSpool
            (
              ModuloSeguimientoSpoolID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI
            )
            VALUES
            (
             @ModuloEmbarqueIDSegSpool,
             'Nota 3',
             'Note 3',
             8,
             'litNota3',
             'Nota3',
             '',
             '',
             150
            )
            
insert into CampoSeguimientoSpool
            (
              ModuloSeguimientoSpoolID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI
            )
            VALUES
            (
             @ModuloEmbarqueIDSegSpool,
             'Nota 4',
             'Note 4',
             9,
             'litNota4',
             'Nota4',
             '',
             '',
             150
            )
            
insert into CampoSeguimientoSpool
            (
              ModuloSeguimientoSpoolID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI
            )
            VALUES
            (
             @ModuloEmbarqueIDSegSpool,
             'Nota 5',
             'Note 5',
             10,
             'litNota5',
             'Nota5',
             '',
             '',
             150
            )