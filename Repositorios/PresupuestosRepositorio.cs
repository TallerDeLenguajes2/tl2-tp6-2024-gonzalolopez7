using Microsoft.Data.Sqlite;
using tp6.Models;

public class PresupuestosRepositorio
{
    
    private readonly string connectionString = "Data Source=db/Tienda.db;Cache=Shared";

    public List<Presupuesto> GetAll()
    {
        var presupuestos = new List<Presupuesto>();
        string queryString = @"
                                SELECT idPresupuesto, NombreDestinatario, FechaCreacion
                                FROM Presupuestos";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                presupuestos.Add(new Presupuesto(
                    Convert.ToInt32(reader["idPresupuesto"]),
                    reader["NombreDestinatario"].ToString(),
                    Convert.ToDateTime(reader["FechaCreacion"])
                ));
            }
            connection.Close();
        }
        return presupuestos;
    }

    public Presupuesto GetById(int idPresupuesto)
    {
        var presupuesto = new Presupuesto();
        string queryString = @"
                                SELECT
                                P.idPresupuesto,
                                P.NombreDestinatario,
                                P.FechaCreacion,
                                PR.idProducto,
                                PR.Descripcion,
                                PR.Precio,
                                PD.Cantidad
                                FROM Presupuestos P
                                LEFT JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                                LEFT JOIN Productos PR ON PD.idProducto = PR.idProducto
                                WHERE P.idPresupuesto = @idPresupuesto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                presupuesto.Detalle = GetDetalle(reader);
            }
            connection.Close();
        }
        return presupuesto;
    }

    private List<PresupuestoDetalle> GetDetalle(SqliteDataReader reader)
    {
        var detalle = new List<PresupuestoDetalle>();
        if (reader["Descripcion"].ToString() != "")
        {
            do
            {
                detalle.Add(new PresupuestoDetalle(
                    new Producto(
                        Convert.ToInt32(reader["idProducto"]),
                        reader["Descripcion"].ToString(),
                        Convert.ToInt32(reader["Precio"])
                    ),
                    Convert.ToInt32(reader["Cantidad"])
                ));
            } while (reader.Read());
        }
        return detalle;
    }

    public void Add(Presupuesto presupuesto)
    {
        string queryString = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @fechaCreacion)";
        string queryString2 = "SELECT last_insert_rowid()";
        int id = 0;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@nombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fechaCreacion", DateTime.Today.ToShortDateString());
            command.ExecuteNonQuery();

            SqliteCommand command2 = new SqliteCommand(queryString2, connection);
            id = Convert.ToInt32(command2.ExecuteScalar());
            connection.Close();
        }
        AddDetalle(id, presupuesto.Detalle);
    }

    private void AddDetalle(int idPresupuesto, List<PresupuestoDetalle> detalles)
    {
        foreach (var detalle in detalles)
        {
            string queryString = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command.Parameters.AddWithValue("@idProducto", detalle.Producto.IdProducto);
                command.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public void Modificar(Presupuesto presupuesto)
    {
        string queryString = @"
                                UPDATE Presupuestos
                                SET NombreDestinatario = @nombreDestinatario
                                WHERE idPresupuesto = @idPresupuesto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@nombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@idPresupuesto", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
        ModificarDetalle(presupuesto.IdPresupuesto, presupuesto.Detalle);
    }

    private void ModificarDetalle(int idPresupuesto, List<PresupuestoDetalle> detalle)
    {
        EliminarDetalle(idPresupuesto);
        AddDetalle(idPresupuesto, detalle);
    }

    private void EliminarDetalle(int idPresupuesto)
    {
        string queryString = @"
                                DELETE FROM PresupuestosDetalle
                                WHERE idPresupuesto = @idPresupuesto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}