namespace Lab1.Models
{
    public class Aspirante : IComparable<Aspirante>
    {
        public string nombre { get; set; }
        public string dpi { get; set; }
        public string nacimiento { get; set; }
        public string direccion { get; set; }
        public int CompareTo(Aspirante other)
        {
            int result = this.nombre.CompareTo(other.nombre);
            return result;
        }
    }
}