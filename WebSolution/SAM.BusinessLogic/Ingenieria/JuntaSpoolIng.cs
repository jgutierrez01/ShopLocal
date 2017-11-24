using System;
using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Extensions;
using SAM.Entities;

namespace SAM.BusinessLogic.Ingenieria
{
    public class JuntaSpoolIng : JuntaSpool, IRegistroValido
    {


        public static Dictionary<Columna, int> Col
            = new Dictionary<Columna, int>
                  {
                      {Columna.NombreDeSpool, (int) Columna.NombreDeSpool},
                      {Columna.NumeroDeJunta, (int) Columna.NumeroDeJunta},
                      {Columna.Diametro, (int) Columna.Diametro},
                      {Columna.TipoDeJunta, (int) Columna.TipoDeJunta},
                      {Columna.Cedula, (int) Columna.Cedula},
                      {Columna.EtiquetaDeLocalizacion, (int) Columna.EtiquetaDeLocalizacion},
                      {Columna.TipoDeMaterial, (int) Columna.TipoDeMaterial},
                      {Columna.Linea, (int) Columna.Linea},
                      {Columna.DibujoRefrencia, (int) Columna.DibujoRefrencia},
                      {Columna.NoSeOcupa, (int) Columna.NoSeOcupa},
                      {Columna.NoSeOcupa1, (int) Columna.NoSeOcupa1},
                      {Columna.RevisionCliente, (int) Columna.RevisionCliente},
                      {Columna.RevisionSteelgo, (int) Columna.RevisionSteelgo},
                      {Columna.Fabarea, (int) Columna.Fabarea}
                  };
        public enum Columna
        {
            NombreDeSpool = 0,
            NumeroDeJunta = 1,
            Diametro = 2,
            TipoDeJunta = 3,
            Cedula = 4,
            EtiquetaDeLocalizacion = 5,
            TipoDeMaterial = 6,
            Linea = 7,
            DibujoRefrencia = 8,
            NoSeOcupa = 9,
            NoSeOcupa1 = 10,
            RevisionCliente = 11,
            RevisionSteelgo = 12,
            Fabarea = 13
        }

      
        public Dictionary<Columna, string> Token;

        public event DelegateValidacionIngenieria JuntaValidada;
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
        
        public int NumLinea { get; set; }
        public string Archivo { get; set; }
        public string TRclass { get; set; }
        public JuntaSpoolIng(int numLinea, string archivo, string[] palabras,Guid? usuarioModifica)
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

        public JuntaSpoolIng(string etiqueta)
        {
            Token = new Dictionary<Columna, string>(Enum.GetValues(typeof(Columna)).Length);
            for (int i = 0; i < Enum.GetValues(typeof(Columna)).Length; i++)
            {
                Token.Add((Columna)i, string.Empty);
            }
            Etiqueta = etiqueta;
            Token[Columna.NumeroDeJunta] = etiqueta;
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
                    if (JuntaValidada != null)
                    {
                        JuntaValidada(this);
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
                RegistroValido = ValidaIntegridadRegistro(this) && ValidaRegistro(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        
        public static bool TieneArmadoOSoldado(JuntaSpool juntaSpoolEnBD)
        {
            return juntaSpoolEnBD.JuntaWorkstatus.Count > 0 &&
                   juntaSpoolEnBD.JuntaWorkstatus.Any(x => x.JuntaFinal && (x.SoldaduraAprobada || x.ArmadoAprobado));
        }
    }
}
