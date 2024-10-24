namespace LifeBook.Data.Entities
{
    public class Protocol
    {
        public int Id { get; set; }               // Identificador único para el protocolo
        public string Name { get; set; }          // Nombre del protocolo (ej. FTP)
        public string Description { get; set; }   // Descripción del protocolo
        public string Port { get; set; }             // Puerto asignado al protocolo
    }
}
