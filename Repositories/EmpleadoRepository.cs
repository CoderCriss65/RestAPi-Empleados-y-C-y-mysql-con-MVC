using MySqlConnector;
using MVCwebApi.Models;
using MVCwebApi.Repositories.Interfaces;
using System.Data;

namespace MVCwebApi.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly string _connectionString;

        public EmpleadoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Empleado>> ObtenerTodosAsync()
        {
            var empleados = new List<Empleado>();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM empleados", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                empleados.Add(MapReaderToEmpleado(reader));
            }

            return empleados;
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM empleados WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToEmpleado(reader);
            }

            return null;
        }

        public async Task<int> CrearAsync(Empleado empleado)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"INSERT INTO empleados 
                    (documento, nombres, apellidos, fechanacimiento, telefono, correo, direccion, ciudad, contrato, jornada) 
                    VALUES 
                    (@doc, @nombres, @apellidos, @fechaNac, @tel, @correo, @dir, @ciudad, @contrato, @jornada);
                    SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            AgregarParametros(command, empleado);

            var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
            return newId;
        }

        public async Task<bool> ActualizarAsync(Empleado empleado)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"UPDATE empleados SET 
                    documento = @doc,
                    nombres = @nombres,
                    apellidos = @apellidos,
                    fechanacimiento = @fechaNac,
                    telefono = @tel,
                    correo = @correo,
                    direccion = @dir,
                    ciudad = @ciudad,
                    contrato = @contrato,
                    jornada = @jornada
                    WHERE id = @id";

            using var command = new MySqlCommand(query, connection);
            AgregarParametros(command, empleado);
            command.Parameters.AddWithValue("@id", empleado.Id);

            var affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("DELETE FROM empleados WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            var affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }

        private Empleado MapReaderToEmpleado(MySqlDataReader reader)
        {
            return new Empleado
            {
                Id = reader.GetInt32("id"),
                Documento = reader.GetString("documento"),
                Nombres = reader.GetString("nombres"),
                Apellidos = reader.GetString("apellidos"),
                FechaNacimiento = reader.IsDBNull("fechanacimiento") ? null : reader.GetDateTime("fechanacimiento"),
                Telefono = reader.GetString("telefono"),
                Correo = reader.GetString("correo"),
                Direccion = reader.GetString("direccion"),
                Ciudad = reader.GetString("ciudad"),
                Contrato = reader.GetString("contrato"),
                Jornada = reader.GetString("jornada")
            };
        }

        private void AgregarParametros(MySqlCommand command, Empleado empleado)
        {
            command.Parameters.AddWithValue("@doc", empleado.Documento);
            command.Parameters.AddWithValue("@nombres", empleado.Nombres);
            command.Parameters.AddWithValue("@apellidos", empleado.Apellidos);
            command.Parameters.AddWithValue("@fechaNac", empleado.FechaNacimiento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@tel", empleado.Telefono);
            command.Parameters.AddWithValue("@correo", empleado.Correo);
            command.Parameters.AddWithValue("@dir", empleado.Direccion);
            command.Parameters.AddWithValue("@ciudad", empleado.Ciudad);
            command.Parameters.AddWithValue("@contrato", empleado.Contrato);
            command.Parameters.AddWithValue("@jornada", empleado.Jornada);
        }
    }
}
