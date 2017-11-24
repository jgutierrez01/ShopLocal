using System;
using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Extensions;
using SAM.Entities;

namespace SAM.BusinessLogic.Ingenieria
{
    public class SpoolIng : Spool, IRegistroValido
    {

        /// <summary>
        /// diccionatio de Columnas, se construye solamente una vez, nos sirve apra identificar el numero de 
        /// columna donde el iterprete encontro un error
        /// </summary>
        public static Dictionary<Columna, int> Col
            = new Dictionary<Columna, int>
                  {
                      {Columna.Linea, (int) Columna.Linea},
                      {Columna.Spool, (int) Columna.Spool},
                      {Columna.Pdi, (int) Columna.Pdi},
                      {Columna.Kg, (int) Columna.Kg},
                      {Columna.M2, (int) Columna.M2},
                      {Columna.Espesor, (int) Columna.Espesor},
                      {Columna.FecGen, (int) Columna.FecGen},
                      {Columna.Especificacion, (int) Columna.Especificacion},
                      {Columna.Proyecto, (int) Columna.Proyecto},
                      {Columna.PorcPnd, (int) Columna.PorcPnd},
                      {Columna.RequierePwth, (int) Columna.RequierePwth},
                      {Columna.DibujoRef, (int) Columna.DibujoRef},
                      {Columna.RevCliente, (int) Columna.RevCliente},
                      {Columna.DiamReal, (int) Columna.DiamReal},
                      {Columna.RevSteelgo, (int) Columna.RevSteelgo},
                      {Columna.SistemaPintura, (int) Columna.SistemaPintura}
                  };

        public enum Columna
        {
            Linea = 0,
            Spool = 1,
            Pdi = 2,
            Kg = 3,
            M2 = 4,
            Espesor = 5,
            FecGen = 6,
            Especificacion = 7,
            Proyecto = 8,
            Material = 9,
            PorcPnd = 10,
            RequierePwth = 11,
            DibujoRef = 12,
            RevCliente = 13,
            DiamReal = 14,
            RevSteelgo = 15,
            SistemaPintura = 16
        }


       

        /// <summary>
        /// Diccionario que contiene los campos del renglon correspondiente en el archivo con que se creo este objeto
        /// </summary>
        public Dictionary<Columna, string> Token;

        public event DelegateValidacionIngenieria SpoolValidado;
        public event DelegateConstruccionRegistroIngenieria Construye;
        public event DelegateFuncionValidacionRegistro ValidaRegistro;
        public event DelegateFuncionValidacionIntegridadRegistro ValidaIntegridadRegistro;

        private readonly List<string> _erroresRegistro;
        /// <summary>
        /// Los errores de que se han encontrado para este objeto
        /// </summary>
        public List<string> ErroresRegistro
        {
            get
            {
                return _erroresRegistro;
            }
        }
       
        /// <summary>
        /// La linea del archivo con que se creo este objeto
        /// </summary>
        public int NumLinea { get; set; }
        
        /// <summary>
        /// El nombre del archivo
        /// </summary>
        public string Archivo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numLinea">numero de lina del archivo</param>
        /// <param name="archivo">nombre del archivo</param>
        /// <param name="palabras">arreglo de string que contiene cada una de las columnas de la linea del archivo</param>
        /// <param name="usuarioModifica">el nombre del usuario que usaremos para modifical la BD</param>
        public SpoolIng(int numLinea, string archivo, string[] palabras, Guid? usuarioModifica)
        {
            UsuarioModifica = usuarioModifica;
            NumLinea = numLinea;
            Archivo = archivo;
            _erroresRegistro = new List<string>();
            Token = new Dictionary<Columna, string>(Enum.GetValues(typeof(Columna)).Length);
            for (int i = 0; i < palabras.Length; i++)
            {
                Token.Add((Columna)i, palabras[i].Trim());
            }
        }

        private bool _registroValido;
        public bool RegistroValido
        {
            get { return _registroValido; }
            set
            {
                if (value != _registroValido && value)
                {
                    if (SpoolValidado != null)
                    {
                        SpoolValidado(this);
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
            if (ValidaRegistro != null && ValidaIntegridadRegistro != null)
            {
                RegistroValido = ValidaIntegridadRegistro(this) && ValidaRegistro(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

}
