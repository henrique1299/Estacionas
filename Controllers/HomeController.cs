using Estacionas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Estacionas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Distance()
        {
            View view = new View();
            List<Estacionamentos> estacionamentos = new List<Estacionamentos>();

            Conn connection = new Conn();
            NpgsqlConnection conn = connection.Connect();
            var transaction = conn.BeginTransaction();
            using (var cmd = new NpgsqlCommand("SELECT * FROM Estacionamentos", conn, transaction))
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Estacionamentos e = new Estacionamentos();
                    e.Id =  reader.GetInt32(0);
                    e.Name = reader.GetString(1);
                    e.Rua = reader.GetString(2);
                    e.Numero = reader.GetString(3);
                    e.Cidade = reader.GetString(4);
                    e.Bairro = reader.GetString(5);
                    e.Cep = reader.GetString(6);
                    e.Telefone = reader.GetString(7);
                    e.Latitude = reader.GetDecimal(8);
                    e.Longitude = reader.GetDecimal(9);

                    estacionamentos.Add(e);
                }
            }
            conn.Close();

            string result = await view.CalculateDistance(estacionamentos, "09210-380");
            int ini = result.IndexOf("\"results\"") + 11;
            int fim = result.IndexOf("]", ini);
            result = result.Substring(ini, fim - ini);

            int aux;
            ini = 0;
            while ((ini = result.IndexOf("travelDistance", ini+1)) > 0)
            {
                aux = result.IndexOf("travelDistance", ini) + "travelDistance".Length + 2;
                estacionamentos[0].Distancia = Convert.ToDecimal(result.Substring(aux, 6)) / 1000; ;
            }

            estacionamentos = estacionamentos.OrderBy(o => o.Distancia).ToList();

            conn = connection.Connect();
            transaction = conn.BeginTransaction();
            using (var cmd = new NpgsqlCommand("select count(*) from (SELECT Vagas.id, Reservas.id FROM Vagas LEFT JOIN Reservas on Vagas.id = Reservas.idVaga where idEstacionamento = "+estacionamentos[0].Id+" group by Vagas.id, Reservas.id having (count(Reservas.Id) = 0)) as aux ", conn, transaction))
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    estacionamentos[0].Vagas = reader.GetInt32(0);
                }
            }
            conn.Close();

            return View("Mapa", estacionamentos[0]);
        }

        public IActionResult Reservar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Conn connection = new Conn();
            NpgsqlConnection conn = connection.Connect();
            var transaction = conn.BeginTransaction();
            using (var cmd = new NpgsqlCommand("INSERT INTO Reservas (idCliente, idVaga) VALUES (2,3)", conn, transaction))
            {
                cmd.ExecuteNonQuery();
            }
            transaction.Commit();
            conn.Close();

            return View("Index");
        }
    }
}