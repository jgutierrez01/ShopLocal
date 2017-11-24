namespace SAM.Entities.Personalizadas
{
    public class JuntaPendientePorHomologar
    {
        public int JuntaSpoolID { get; set; }
        public int JuntaSpoolPendienteID { get; set; }
        public AccionesHomologacion Accion { get; set; }
        public bool PasoValidacion { get; set; }
        public string MensajeValidacion { get; set; }
    }
}
