namespace LifeBook.Data.Entities
{
    public class Attack
    {
        public int Id { get; set; }              
        public string Name { get; set; }         
        public string Description { get; set; }  
        public string VideoUrl { get; set; }     

        // Relación con el sistema operativo
        public int SoId { get; set; }            // Id del sistema operativo al que pertenece
        public So? So { get; set; } // Referencia al sistema operativo

        // Relación con herramientas
        public List<Tool>? Tools { get; set; }
    }
}
