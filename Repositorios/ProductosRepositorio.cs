using Microsoft.Data.Sqlite;
using tp6.Models;

public class ProductosRepositorio
{
    private readonly string connectionString = "Data Source=db/Tienda.db;Cache=Shared";

    public List<Producto> GetAll()
    {
        var productos = new List<Producto>();
        string queryString = @"
                                SELECT idProducto, Descripcion, Precio
                                FROM Productos";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                productos.Add(new Producto(
                        Convert.ToInt32(reader["idProducto"]),
                        reader["Descripcion"].ToString(),
                        Convert.ToInt32(reader["precio"])
                ));
            }
            connection.Close();
        }
        return productos;
    }

    public Producto GetById(int idProducto)
    {
        var producto = new Producto();
        string queryString = @"
                                SELECT idProducto, Descripcion, Precio
                                FROM Productos
                                WHERE idProducto = @idProducto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("idProducto", idProducto);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                producto.Descripcion = reader["Descripcion"].ToString();
                producto.Precio = Convert.ToInt32(reader["Precio"]);
            }
            connection.Close();
        }
        return producto;
    }

    public void Add(Producto producto)
    {
        string queryString = @"
                                INSERT INTO Productos (Descripcion, Precio)
                                VALUES (@descripcion, @precio)";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Modificar(Producto producto)
    {
        string queryString = @"
                                UPDATE Productos
                                SET Descripcion = @descripcion, Precio = @precio
                                WHERE idProducto = @idProducto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@idProducto", producto.IdProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Eliminar(int idProducto)
    {
        string queryString = @"
                                DELETE FROM Productos
                                WHERE idProducto = @idProducto";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}