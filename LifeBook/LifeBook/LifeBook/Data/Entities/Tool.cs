namespace LifeBook.Data.Entities
{
    public class Tool
    {
        public int Id { get; set; }              // Identificador único
        public string Name { get; set; }       // Nombre de la herramienta
        public string Description { get; set; }  // Descripción de la herramienta

        // Nueva propiedad URL
        public string? URL { get; set; }        // Link o dirección de la herramienta

        public string? DocumentPath { get; set; }  // Nueva propiedad para el documento

        // Relación con el sistema operativo
        public int SoId { get; set; }            // Id del sistema operativo al que pertenece
        public So? So { get; set; }              // Referencia al sistema operativo

        // Relación con el ataque
        public int AttackId { get; set; }        // Id del ataque al que pertenece
        public Attack? Attack { get; set; }      // Referencia al ataque
    }
}
