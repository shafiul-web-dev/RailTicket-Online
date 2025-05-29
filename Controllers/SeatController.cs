using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RailTicket_Online.Data;
using RailTicket_Online.DTO;
using RailTicket_Online.Models;

[Route("api/seats")]
[ApiController]
public class SeatController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public SeatController(ApplicationDbContext context)
	{
		_context = context;
	}

	// 🔹 Get Available Seats for a Train (READ)
	[HttpGet("{trainId}")]
	public async Task<ActionResult<IEnumerable<SeatDto>>> GetAvailableSeats(int trainId)
	{
		var seats = await _context.Seats
			.Where(s => s.TrainId == trainId)
			.Select(s => new SeatDto { SeatNumber = s.SeatNumber, IsReserved = s.IsReserved })
			.ToListAsync();

		return Ok(seats);
	}

	// 🔹 Add New Seats for a Train (CREATE)
	[HttpPost]
	public async Task<ActionResult<Seat>> AddSeat(CreateSeatDto seatDto)
	{
		var trainExists = await _context.Trains.AnyAsync(t => t.Id == seatDto.TrainId);
		if (!trainExists) return NotFound(new { message = "Train not found." });

		var seat = new Seat
		{
			TrainId = seatDto.TrainId,
			SeatNumber = seatDto.SeatNumber,
			IsReserved = false
		};

		_context.Seats.Add(seat);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetAvailableSeats), new { id = seat.Id }, seat);
	}

	// 🔹 Reserve a Seat (UPDATE)
	[HttpPut("reserve/{seatId}")]
	public async Task<IActionResult> ReserveSeat(int seatId)
	{
		var seat = await _context.Seats.FindAsync(seatId);
		if (seat == null) return NotFound(new { message = "Seat not found." });
		if (seat.IsReserved) return BadRequest(new { message = "Seat already reserved." });

		seat.IsReserved = true;
		await _context.SaveChangesAsync();
		return Ok(new { message = "Seat reserved successfully." });
	}

	// 🔹 Free a Reserved Seat (UPDATE)
	[HttpPut("free/{seatId}")]
	public async Task<IActionResult> FreeSeat(int seatId)
	{
		var seat = await _context.Seats.FindAsync(seatId);
		if (seat == null) return NotFound(new { message = "Seat not found." });

		seat.IsReserved = false;
		await _context.SaveChangesAsync();
		return Ok(new { message = "Seat is now available." });
	}

	// 🔹 Delete a Seat from Train (DELETE)
	[HttpDelete("{seatId}")]
	public async Task<IActionResult> DeleteSeat(int seatId)
	{
		var seat = await _context.Seats.FindAsync(seatId);
		if (seat == null) return NotFound(new { message = "Seat not found." });

		_context.Seats.Remove(seat);
		await _context.SaveChangesAsync();
		return Ok(new { message = "Seat removed successfully." });
	}
}