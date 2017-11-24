namespace SAM.BusinessLogic.Ingenieria
{
    /// <summary>
    /// Interfaz que se usara para asegurarnos que se tienen estos metodos para los objetos del proceso de importacion de ingenieria
    /// </summary>
    public interface IRegistroValido
    {
        /// <summary>
        /// Determina si este objeto es valido acorde a las validaciones de BL
        /// </summary>
        /// <returns></returns>
        bool EsRegistroValido();

        /// <summary>
        /// Metodo que dispara el evento de construccion del registro
        /// </summary>
        void ConstruyeRegistro();

        /// <summary>
        /// Metodo que dispara los eventos de validacion
        /// </summary>
        void Valida();

    }
}
