namespace LifeBook.Data.Entities
{
    public class IA
    {
        public int Id { get; set; }               // Identificador único de la IA
        public string Name { get; set; }          // Nombre de la IA
        public string Description { get; set; }   // Descripción breve de la IA
        public string Url { get; set; }           // URL asociada a la IA

        // Relación con la entidad IACategory
        public int? CategoryId { get; set; }
        public IACategory? IACategory { get; set; }
    }
}