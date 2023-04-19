namespace Estacionas.Models
{
    public class VagasOculpadas
    {
        public int Id { get; set; }
        public int idCliente { get; set; }
        public int idVaga { get; set; }
        public string Carro { get; set; }
        public DateTime Data { get; set; }
    }
}
