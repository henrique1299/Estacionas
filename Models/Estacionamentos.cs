using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Estacionas.Models
{
    public class Estacionamentos
    {
        public List<Estacionamentos> exibicao { get; set; }
        public int Id { get; set; }
        public string Name { get; set;}
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set;}
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distancia { get; set; }
        public int Vagas { get; set; }
    }
}
