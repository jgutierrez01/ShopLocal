declare @ModuloEmbarqueIDSegJunta INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Embarque')

insert into CampoSeguimientoJunta
            (
              ModuloSeguimientoJuntaID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI,
              TipoDeDato
            )
            VALUES
            (
             @ModuloEmbarqueIDSegJunta,
             'Nota 1',
             'Note 1',
             6,
             'litNota1',
             'Nota1',
             '',
             '',
             150,
             'System.String'
            )
            
insert into CampoSeguimientoJunta
            (
              ModuloSeguimientoJuntaID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI,
              TipoDeDato
            )
            VALUES
            (
             @ModuloEmbarqueIDSegJunta,
             'Nota 2',
             'Note 2',
             7,
             'litNota2',
             'Nota2',
             '',
             '',
             150,
             'System.String'
            )
            
insert into CampoSeguimientoJunta
            (
              ModuloSeguimientoJuntaID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI,
              TipoDeDato
            )
            VALUES
            (
             @ModuloEmbarqueIDSegJunta,
             'Nota 3',
             'Note 3',
             8,
             'litNota3',
             'Nota3',
             '',
             '',
             150,
             'System.String'
            )

insert into CampoSeguimientoJunta
            (
              ModuloSeguimientoJuntaID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI,
              TipoDeDato
            )
            VALUES
            (
             @ModuloEmbarqueIDSegJunta,
             'Nota 4',
             'Note 4',
             9,
             'litNota4',
             'Nota4',
             '',
             '',
             150,
             'System.String'
            )

insert into CampoSeguimientoJunta
            (
              ModuloSeguimientoJuntaID,
              Nombre,
              NombreIngles,
              OrdenUI,
              NombreControlUI,
              NombreColumnaSp,
              DataFormat,
              CssColumnaUI,
              AnchoUI,
              TipoDeDato
            )
            VALUES
            (
             @ModuloEmbarqueIDSegJunta,
             'Nota 5',
             'Note 5',
             10,
             'litNota5',
             'Nota5',
             '',
             '',
             150,
             'System.String'
            )