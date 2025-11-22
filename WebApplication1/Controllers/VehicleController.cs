using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entities;
using WebApplication1.Infrastructure.Contexts;

namespace WebApplication1.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly DatabaseContext _context;
    
    public VehicleController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        return Ok(_context.Vehicles.ToList());
    }
    
    [HttpGet("{id}")]
    public IActionResult Read(int id)
    {
        var vehicle = _context.Vehicles.Find(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost()]
    public IActionResult Post([FromBody] Vehicle vehicle)
    {
        try
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Uma exceção ocorreu ao tentar inserir um veículo: {e.Message}");
            return BadRequest();
        }
        
        return Ok(vehicle);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Vehicle newVehicle)
    {
        var vehicle = _context.Vehicles.Find(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(newVehicle.LicensePlate))
        {
            vehicle.LicensePlate = newVehicle.LicensePlate;
        }

        if (!string.IsNullOrEmpty(newVehicle.Model))
        {
            vehicle.Model = newVehicle.Model;
        }

        if (!string.IsNullOrEmpty(newVehicle.Brand))
        {
            vehicle.Brand = newVehicle.Brand;
        }

        if (!string.IsNullOrEmpty(newVehicle.Color))
        {
            vehicle.Color = newVehicle.Color;
        }

        try
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Uma exceção ocorreu ao tentar atualizar os dados de um veículo: {e.Message}");
            return BadRequest();
        }
        
        return Ok(vehicle);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var vehicle = _context.Vehicles.Find(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        try
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Uma exceção ocorreu ao tentar remover um veículo: {e.Message}");
            return BadRequest();
        }
        return NoContent();
    }
}