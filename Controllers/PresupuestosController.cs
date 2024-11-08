using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp6.Models;

namespace tp6.Controllers;

public class PresupuestosController : Controller
{
    private readonly PresupuestosRepositorio repositorio;
    private readonly ProductosRepositorio repositorio_p;

    public PresupuestosController()
    {
        repositorio = new PresupuestosRepositorio();
        repositorio_p = new ProductosRepositorio();
    }

    public IActionResult Index()
    {
        var presupuestos = repositorio.GetAll();
        return View("Index", presupuestos);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        ViewBag.Productos = repositorio_p.GetAll();
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Presupuesto presupuesto)
    {
        presupuesto.Detalle = presupuesto.Detalle
                                .Where(x => x.Cantidad > 0)
                                .ToList();
        repositorio.Add(presupuesto);
        return Index();
    }

    [HttpGet]
    public IActionResult Modificar(int id)
    {
        var presupuesto = repositorio.GetById(id);
        ViewBag.Productos = repositorio_p.GetAll();
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Modificar(Presupuesto presupuesto)
    {
        presupuesto.Detalle = presupuesto.Detalle
                                .Where(x => x.Cantidad > 0)
                                .ToList();
        repositorio.Modificar(presupuesto);
        return Index();
    }

    // eliminar

    public IActionResult VerDetalle(int id)
    {
        var presupuesto = repositorio.GetById(id);
        return View(presupuesto);
    }

    public IActionResult Eliminar(int id)
    {
        repositorio.Eliminar(id);
        return Index();
    }
}