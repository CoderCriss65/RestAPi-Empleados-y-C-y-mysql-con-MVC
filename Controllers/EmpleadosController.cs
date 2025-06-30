using Microsoft.AspNetCore.Mvc;
using MVCwebApi.Models;
using MVCwebApi.Repositories.Interfaces;
using MVCwebApi.Models;

namespace EmpleadosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoRepository _repository;

    public EmpleadosController(IEmpleadoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empleado>>> ObtenerTodos()
    {
        var empleados = await _repository.ObtenerTodosAsync();
        return Ok(empleados);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Empleado>> ObtenerPorId(int id)
    {
        var empleado = await _repository.ObtenerPorIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }
        return Ok(empleado);
    }

    [HttpPost]
    public async Task<ActionResult<Empleado>> Crear(Empleado empleado)
    {
        var nuevoId = await _repository.CrearAsync(empleado);
        empleado.Id = nuevoId;
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoId }, empleado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, Empleado empleado)
    {
        if (id != empleado.Id)
        {
            return BadRequest();
        }

        var actualizado = await _repository.ActualizarAsync(empleado);
        if (!actualizado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _repository.EliminarAsync(id);
        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}