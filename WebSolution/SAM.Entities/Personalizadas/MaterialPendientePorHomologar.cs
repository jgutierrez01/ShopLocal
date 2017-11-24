namespace SAM.Entities.Personalizadas
{
    public class MaterialPendientePorHomologar
    {
        public int MaterialSpoolID { get; set; }
        public int MaterialSpoolPendienteID { get; set; }
        public AccionesHomologacion Accion { get; set; }
        public bool PasoValidacion { get; set; }
        public string MensajeValidacion { get; set; }
    }
}
