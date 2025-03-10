using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examen1.Models;

namespace Examen1.Controllers;

public class HomeController : Controller
{
    private readonly AccesoDatos _accesoDatos;
    private readonly ILogger<HomeController> _logger;

    public HomeController(AccesoDatos accesoDatos, ILogger<HomeController> logger)
    {
        _accesoDatos = accesoDatos;
        _logger = logger;
    }

    public ActionResult Index()
    {
        var citas = _accesoDatos.ObtenerCitasDelDia();
        return View(citas);
    }

    [HttpPost]
    public ActionResult BuscarPaciente(string cedula)
    {
        var paciente = _accesoDatos.ObtenerPacientePorCedula(cedula);
        if (paciente == null)
        {
            return RedirectToAction("RegistrarPaciente");
        }
        return View("Pacientes", paciente);
    }

    [HttpPost]
    public ActionResult RegistrarPaciente(Pacientes paciente)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            _accesoDatos.InsertarPaciente(paciente);
            TempData["SuccessMessage"] = "Paciente registrado con éxito.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al registrar paciente: " + ex.Message);
            TempData["ErrorMessage"] = "Hubo un problema al registrar el paciente.";
            return RedirectToAction("Index");
        }
    }

    public ActionResult EliminarPaciente(string cedula)
    {
        var paciente = _accesoDatos.ObtenerPacientePorCedula(cedula);
        if (paciente == null)
        {
            return RedirectToAction("Pacientes");
        }

        _accesoDatos.EliminarPacientePorCedula(cedula);

        return RedirectToAction("Pacientes");
    }


    [HttpPost]
    public ActionResult AgendarCita(Citas cita)
    {
        if (!ModelState.IsValid)
        {
            return View("FormularioCita", cita);
        }

        try
        {
            _accesoDatos.InsertarCita(cita);
            TempData["SuccessMessage"] = "Cita agendada con éxito.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al agendar cita: " + ex.Message);
            TempData["ErrorMessage"] = "No se pudo agendar la cita.";
            return RedirectToAction("Index");
        }
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
}
