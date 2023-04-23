using Estacionas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using static Estacionas.Models.Conn;

namespace Estacionas.Controllers
{
    public class ClientesController : Controller
    {
        public ActionResult Clientes()
        {
            return View();
        }
        // GET: ClientesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClientesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Conn connection = new Conn();
                NpgsqlConnection conn = connection.Connect();
                var transaction = conn.BeginTransaction();
                using (var cmd = new NpgsqlCommand("INSERT INTO Clientes (nome, documento) VALUES ((@nome), (@doc))", conn, transaction))
                {
                    string nome = collection["Name"];
                    string doc = collection["Documento"];
                    nome.Replace("{", "");
                    nome.Replace("}", "");
                    doc.Replace("{", "");
                    doc.Replace("}", "");

                    cmd.Parameters.AddWithValue("nome", nome);
                    cmd.Parameters.AddWithValue("doc", doc);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                conn.Close();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Clientes");
            }
        }

        // GET: ClientesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
