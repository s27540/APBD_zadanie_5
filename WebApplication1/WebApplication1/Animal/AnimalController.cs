using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Animal;

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly SqlConnection _connection;

    public AnimalController(SqlConnection connection)
    {
        _connection = connection;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Animal>> GetAnimals(string orderBy = "name")
    {
        try
        {
            _connection.Open();
            SqlCommand command = new SqlCommand($"SELECT * FROM Animals ORDER BY {orderBy}", _connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Animal> animals = new List<Animal>();
            while (reader.Read())
            {
                animals.Add(new Animal
                {
                    IdAnimal = reader.GetInt32(reader.GetOrdinal("IdAnimal")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Area = reader.GetString(reader.GetOrdinal("Area"))
                });
            }

            return Ok(animals);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost]
    public ActionResult AddAnimal([FromBody] Animal newAnimal)
    {
        try
        {
            if (newAnimal == null)
            {
                return BadRequest("Bad request!");
            }

            _connection.Open();
            SqlCommand command = new SqlCommand(
                "INSERT INTO Animals (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)",
                _connection);
            command.Parameters.AddWithValue("@Name", newAnimal.Name);
            command.Parameters.AddWithValue("@Description", newAnimal.Description);
            command.Parameters.AddWithValue("@Category", newAnimal.Category);
            command.Parameters.AddWithValue("@Area", newAnimal.Area);
            command.ExecuteNonQuery();

            return StatusCode(201);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        finally
        {
            _connection.Close();
        }
    }
    
    [HttpPut("{idAnimal}")]
    public ActionResult UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
    {
        try
        {
            if (updatedAnimal == null || idAnimal != updatedAnimal.IdAnimal)
            {
                return BadRequest("Bad request!");
            }

            _connection.Open();
            SqlCommand command = new SqlCommand(
                "UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal",
                _connection);
            command.Parameters.AddWithValue("@Name", updatedAnimal.Name);
            command.Parameters.AddWithValue("@Description", updatedAnimal.Description);
            command.Parameters.AddWithValue("@Category", updatedAnimal.Category);
            command.Parameters.AddWithValue("@Area", updatedAnimal.Area);
            command.Parameters.AddWithValue("@IdAnimal", idAnimal);
            command.ExecuteNonQuery();

            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpDelete("{idAnimal}")]
    public ActionResult DeleteAnimal(int idAnimal)
    {
        try
        {
            _connection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM Animals WHERE IdAnimal = @IdAnimal", _connection);
            command.Parameters.AddWithValue("@IdAnimal", idAnimal);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound("Not found!");
            }

            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        finally
        {
            _connection.Close();
        }
    }
}
    
    
