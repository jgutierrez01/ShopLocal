using System;
using System.Collections.Generic;
using SAM.Entities;

namespace SAM.BusinessLogic.Ingenieria
{
    public class CorteSpoolIng : CorteSpool, IRegistroValido
    {
        public static Dictionary<Columna, int> Col
            = new Dictionary<Columna, int>
                  {
                      {Columna.Linea, (int) Columna.Linea},
                      {Columna.DibujoReferencia, (int) Columna.DibujoReferencia},
                      {Columna.Revision, (int) Columna.Revision},
                      {Columna.NombreDeSpool, (int) Columna.NombreDeSpool},
                      {Columna.ItemCode, (int) Columna.ItemCode},
                      {Columna.Especificacion, (int) Columna.Especificacion},
                      {Columna.Diametro, (int) Columna.Diametro},
                      {Columna.DescripcionDeMaterial, (int) Columna.DescripcionDeMaterial},
                      {Columna.EtiquetaDeSeccionDeTubo, (int) Columna.EtiquetaDeSeccionDeTubo},
                      {Columna.Cantidad, (int) Columna.Cantidad},
                      {Columna.TipoDeCorteTramoInicio, (int) Columna.TipoDeCorteTramoInicio},
                      {Columna.TipoCorteTramoFinal, (int) Columna.TipoCorteTramoFinal},
                      {Columna.EtiquetaDeLocalizacion, (int) Columna.EtiquetaDeLocalizacion},
                      {Columna.Proyecto, (int) Columna.Proyecto},
                      {Columna.Observaciones, (int) Columna.Observaciones},
                      {Columna.RevisionSteelgo, (int) Columna.RevisionSteelgo}
                  };
        public enum Columna
        {
            Linea = 0,
            DibujoReferencia = 1,
            Revision = 2,
            NombreDeSpool = 3,
            ItemCode = 4,
            Especificacion = 5,
            Diametro = 6,
            DescripcionDeMaterial = 7,
            EtiquetaDeSeccionDeTubo = 8,
            Cantidad = 9,
            TipoDeCorteTramoInicio = 10,
            TipoCorteTramoFinal = 11,
            EtiquetaDeLocalizacion = 12,
            Proyecto = 13,
            Observaciones = 14,
            RevisionSteelgo = 15
        }

       

        public Dictionary<Columna, string> Token;

        public event DelegateValidacionIngenieria CorteSpoolValidado;
        public event DelegateConstruccionRegistroIngenieria Construye;
        public event DelegateFuncionValidacionRegistro ValidaRegistro;
        public event DelegateFuncionValidacionIntegridadRegistro ValidaIntegridadRegistro;

        private readonly List<string> _erroresRegistro;
        public List<string> ErroresRegistro
        {
            get
            {
                return _erroresRegistro;
            }
        }

        //Para homologacion
        public string TRclass { get; set; }
        public int NumLinea { get; set; }
        public string Archivo { get; set; }
        public CorteSpoolIng(string etiquetaMaterial)
        {
            Token = new Dictionary<Columna, string>(Enum.GetValues(typeof(Columna)).Length);
            for (int i = 0; i < Enum.GetValues(typeof(Columna)).Length; i++)
            {
                Token.Add((Columna)i,string.Empty);
            }
            EtiquetaMaterial = etiquetaMaterial;
            Token[Columna.EtiquetaDeLocalizacion] = etiquetaMaterial;
            TRclass = string.Empty;
        }
        public CorteSpoolIng(int numLinea, string archivo, string[] palabras, Guid? usuarioModifica)
        {
            UsuarioModifica = usuarioModifica;
            NumLinea = numLinea;
            Archivo = archivo;
            Token = new Dictionary<Columna, string>(Enum.GetValues(typeof(Columna)).Length);
            for (int i = 0; i < palabras.Length; i++)
            {
                Token.Add((Columna)i, palabras[i].Trim());
            }
            _erroresRegistro = new List<string>();
            TRclass = string.Empty;
        }

        private bool _registroValido;
        public bool RegistroValido
        {
            get { return _registroValido; }
            set
            {
                if (value != _registroValido && value)
                {
                    if (CorteSpoolValidado != null)
                    {
                        CorteSpoolValidado(this);
                    }
                }
                _registroValido = value;
            }
        }
        public bool EsRegistroValido()
        {
            return RegistroValido;
        }

        public void ConstruyeRegistro()
        {
            if (Construye != null)
            {
                Construye(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Valida()
        {
            if (ValidaIntegridadRegistro != null && ValidaRegistro != null)
            {
                RegistroValido = ValidaIntegridadRegistro(this) &&  ValidaRegistro(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
