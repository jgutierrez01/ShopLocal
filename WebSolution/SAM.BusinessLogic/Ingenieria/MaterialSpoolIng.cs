using System;
using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Extensions;
using SAM.Entities;

namespace SAM.BusinessLogic.Ingenieria
{
    public class MaterialSpoolIng : MaterialSpool, IRegistroValido
    {
        public static Dictionary<Columna, int> Col
            = new Dictionary<Columna, int>
                  {
                      {Columna.Linea, (int) Columna.Linea},
                      {Columna.DibujoRef, (int) Columna.DibujoRef},
                      {Columna.RevCte1, (int) Columna.RevCte1},
                      {Columna.Spool, (int) Columna.Spool},
                      {Columna.Ic, (int) Columna.Ic},
                      {Columna.Diametro, (int) Columna.Diametro},
                      {Columna.EtLoc, (int) Columna.EtLoc},
                      {Columna.Cantidad, (int) Columna.Cantidad},
                      {Columna.PesoKgs, (int) Columna.PesoKgs},
                      {Columna.ClasifMat, (int) Columna.ClasifMat},
                      {Columna.Descripcion, (int) Columna.Descripcion},
                      {Columna.Especificacion, (int) Columna.Especificacion},
                      {Columna.RevCte2, (int) Columna.RevCte2},
                      {Columna.RevSt, (int) Columna.RevSt}
                  };
        public enum Columna
        {
            Linea = 0,
            DibujoRef = 1,
            RevCte1 = 2,
            Spool = 3,
            Ic = 4,
            Diametro = 5,
            EtLoc = 6,
            Cantidad = 7,
            PesoKgs = 8,
            ClasifMat = 9,
            Descripcion = 10,
            Especificacion = 11,
            RevCte2 = 12,
            RevSt = 13
        }

      
        public Dictionary<Columna, string> Token;

        public event DelegateValidacionIngenieria MaterialSpoolValidado;
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
        public MaterialSpoolIng(int numLinea, string archivo, string[] palabras, Guid? usuarioModifica)
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

        public MaterialSpoolIng(string etiqueta)
        {
            Token = new Dictionary<Columna, string>(Enum.GetValues(typeof(Columna)).Length);
            for (int i = 0; i < Enum.GetValues(typeof(Columna)).Length; i++)
            {
                Token.Add((Columna)i, string.Empty);
            }
            Etiqueta = etiqueta;
            Token[Columna.EtLoc] = etiqueta;
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
                    if (MaterialSpoolValidado != null)
                    {
                        MaterialSpoolValidado(this);
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
                RegistroValido = ValidaIntegridadRegistro (this) && ValidaRegistro(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        

        public static bool TieneMaterialDespacho(MaterialSpool msBD)
        {
            return msBD.OrdenTrabajoMaterial.Count > 0 &&
                   msBD.OrdenTrabajoMaterial.Any(x => x.TieneCorte.GetValueOrDefault(false) || x.TieneDespacho);
        }
    }
}
