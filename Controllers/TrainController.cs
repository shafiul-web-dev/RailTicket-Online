using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailTicket_Online.Data;
using RailTicket_Online.DTO;
using RailTicket_Online.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/trains")]
[ApiController]
public class TrainController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public TrainController(ApplicationDbContext context)
	{
		_context = context;
	}

	// 🔹 Get All Trains (READ)
	[HttpGet]
	public async Task<ActionResult<IEnumerable<TrainDto>>> GetTrains()
	{
		var trains = await _context.Trains.Select(t => new TrainDto
		{
			Id = t.Id,
			TrainName = t.TrainName,
			Source = t.Source,
			Destination = t.Destination,
			DepartureTime = t.DepartureTime,
			ArrivalTime = t.ArrivalTime
		}).ToListAsync();

		return Ok(trains);
	}

	// 🔹 Get Train by ID (READ)
	[HttpGet("{id}")]
	public async Task<ActionResult<TrainDto>> GetTrainById(int id)
	{
		var train = await _context.Trains.FindAsync(id);
		if (train == null) return NotFound(new { message = "Train not found." });

		return Ok(new TrainDto
		{
			Id = train.Id,
			TrainName = train.TrainName,
			Source = train.Source,
			Destination = train.Destination,
			DepartureTime = train.DepartureTime,
			ArrivalTime = train.ArrivalTime
		});
	}

	// 🔹 Add a New Train (CREATE)
	[HttpPost]
	public async Task<ActionResult<Train>> AddTrain(CreateTrainDto trainDto) // [FromHeader(Name = "apiKey")] string apiKey)
	{
		//if (apiKey != "ADMIN_KEY")
		//	return Unauthorized(new { message = "Unauthorized access. Only railway owners can add trains." });

		if (trainDto.DepartureTime >= trainDto.ArrivalTime)
			return BadRequest(new { message = "Departure time must be earlier than arrival time." });

		var train = new Train
		{
			TrainName = trainDto.TrainName,
			Source = trainDto.Source,
			Destination = trainDto.Destination,
			DepartureTime = trainDto.DepartureTime,
			ArrivalTime = trainDto.ArrivalTime
		};

		_context.Trains.Add(train);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetTrains), new { id = train.Id }, train);
	}

	// 🔹 Update Train Details (UPDATE)
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateTrain(int id, UpdateTrainDto trainDto)
	{
		var train = await _context.Trains.FindAsync(id);
		if (train == null)
			return NotFound(new { message = "Train not found." });

		// 🔹 Validate departure & arrival times if both are provided
		if (trainDto.DepartureTime.HasValue && trainDto.ArrivalTime.HasValue)
		{
			if (trainDto.DepartureTime.Value >= trainDto.ArrivalTime.Value)
				return BadRequest(new { message = "Departure time must be earlier than arrival time." });

			train.DepartureTime = trainDto.DepartureTime.Value;
			train.ArrivalTime = trainDto.ArrivalTime.Value;
		}

		// 🔹 Update only if values are provided (Prevent overwriting with null)
		if (!string.IsNullOrEmpty(trainDto.TrainName))
			train.TrainName = trainDto.TrainName;

		if (!string.IsNullOrEmpty(trainDto.Source))
			train.Source = trainDto.Source;

		if (!string.IsNullOrEmpty(trainDto.Destination))
			train.Destination = trainDto.Destination;

		// 🔹 Save changes with error handling
		try
		{
			await _context.SaveChangesAsync();
			return Ok(new { message = "Train updated successfully." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "An error occurred while updating the train.", error = ex.Message });
		}
	}

	// 🔹 Delete a Train (DELETE)
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteTrain(int id)
	{
		var train = await _context.Trains.FindAsync(id);
		if (train == null)
			return NotFound(new { message = "Train not found." });

		_context.Trains.Remove(train);
		await _context.SaveChangesAsync();

		return NoContent(); // Return `204 No Content` instead of `200 OK`
	}
}