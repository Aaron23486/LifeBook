namespace LifeBook.Data.Entities
{
    public class IACategory
    {
        public int Id { get; set; }               // Identificador único de la categoría
        public string Name { get; set; }          // Nombre de la categoría (e.g., "Videos", "Images", "Text")
        public string Description { get; set; }   // Descripción de la categoría

        // Contador que representa la cantidad de IA en esta categoría
        public int IACount => IAs.Count;

        // Relación con la entidad IA
        public ICollection<IA> IAs { get; set; } = new List<IA>();
    }
}