using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp6.Models;

namespace tp6.Controllers;

public class ProductosController : Controller
{
    ProductosRepositorio repositorio;

    public ProductosController()
    {
        repositorio = new ProductosRepositorio();
    }

    public IActionResult Index()
    {
        var productos = repositorio.GetAll();
        return View("Index", productos);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Producto producto)
    {
        repositorio.Add(producto);
        return Index();
    }

    [HttpGet]
    public IActionResult Modificar(int id)
    {
        var producto = repositorio.GetById(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult Modificar(Producto producto)
    {
        repositorio.Modificar(producto);
        return Index();
    }

    public IActionResult Eliminar(int id)
    {
        repositorio.Eliminar(id);
        return Index();
    }
}