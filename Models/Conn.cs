using Npgsql;

namespace Estacionas.Models
{
    public class Conn
    {
        public Conn() { }

        private NpgsqlConnection conn;

        public NpgsqlConnection Connect()
        {
            var connString = "Host=hansken.db.elephantsql.com;Username=icausbcx;Password=4hXxAElaa3hQy-6emkopDG8HxR8hawPS;Database=icausbcx";

            this.conn = new NpgsqlConnection(connString);
            this.conn.OpenAsync();

            return conn;
        }

        public void Insert()
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO Clientes (nome, documento) VALUES (@n, @d)", conn))
            {
                cmd.Parameters.AddWithValue("n", "Hello world");
                cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
