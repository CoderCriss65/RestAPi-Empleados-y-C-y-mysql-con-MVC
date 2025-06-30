using MVCwebApi.Models;
namespace MVCwebApi.Repositories.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Empleado empleado);
        Task<bool> ActualizarAsync(Empleado empleado);
        Task<bool> EliminarAsync(int id);
    }
}
