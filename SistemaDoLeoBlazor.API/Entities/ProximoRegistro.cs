namespace SistemaDoLeoBlazor.API.Entities
{
    public class ProximoRegistro
    {
        public int id { get; set; }
        public required int categoria { get; set; }
        public required int cliente { get; set; }
        public required int formaPgto { get; set; }
        public required int operador { get; set; }
        public required int pedido { get; set; }
        public required int produto { get; set; }
    }
}
