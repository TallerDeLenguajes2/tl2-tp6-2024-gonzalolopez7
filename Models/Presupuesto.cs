namespace tp6.Models;

public class Presupuesto
{
    int idPresupuesto;
    string nombreDestinatario;
    DateTime fechaCreacion;
    List<PresupuestoDetalle> detalle;

    public Presupuesto() {  }

    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
    }

    public Presupuesto(string nombreDestinatario, DateTime fechaCreacion)
    {
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

    public int MontoPresupuesto()
    {
        int monto = 0;
        foreach (var item in Detalle)
            monto += item.Producto.Precio * item.Cantidad;

        return monto;
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }

    public double CantidadProductos()
    {
        int cantidad = 0;
        foreach (var item in Detalle)
            cantidad += item.Cantidad;

        return cantidad;
    }
}