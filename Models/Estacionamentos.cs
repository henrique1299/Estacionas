using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Estacionas.Models
{
    public class Estacionamentos
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set;}

    }
}
