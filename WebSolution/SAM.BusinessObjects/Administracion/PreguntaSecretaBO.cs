using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Administracion
{
    public class PreguntaSecretaBO
    {
        private static readonly object _mutex = new object();
        private static PreguntaSecretaBO _instance;
        private List<PreguntaSecreta> _lst;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PreguntaSecretaBO()
        {
            _lst = new List<PreguntaSecreta>();
            _lst.Add(new PreguntaSecreta { Pregunta = "¿Cuál era tu trabajo soñado de niño(a)?", PreguntaIngles = "What was your dream job as a child?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿Cuál es el nombre de la primer persona que besaste?", PreguntaIngles = "What was the name of the first person you kissed?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿A qué escuela ibas en sexto año?", PreguntaIngles = "What was the name of the school you attended in sixth year?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿Cuál fue tu primer carro?", PreguntaIngles = "What was the make of your first car?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿A los cuántos años fue tu primer beso?", PreguntaIngles = "How old were you on your fist kiss?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿Cuál es el nombre del maestro que más recuerdas?", PreguntaIngles = "What's the name of your most memorable teacher?" });
            _lst.Add(new PreguntaSecreta { Pregunta = "¿Cuál es la dirección de tu primera casa?", PreguntaIngles = "What was the address of your first home?" });
        }

        /// <summary>
        /// Obtiene la instancia de la clase PreguntaSecretaBO
        /// </summary>
        /// <returns></returns>
        public static PreguntaSecretaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PreguntaSecretaBO();
                    }
                }
                return _instance;
            }
        }

        public List<PreguntaSecreta> ObtenerTodas()
        {
            return _lst;
        }
    }
}
