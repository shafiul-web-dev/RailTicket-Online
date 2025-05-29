using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using RailTicket_Online.Data;
using RailTicket_Online.DTO;
using RailTicket_Online.Models;

[Route("api/tickets")]
[ApiController]
public class TicketController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public TicketController(ApplicationDbContext context)
	{
		_context = context;
	}

	// 🔹 Get All Tickets (READ)
	[HttpGet]
	public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
	{
		var tickets = await _context.Tickets
			.Include(t => t.Train)
			.Select(t => new TicketDto
			{
				Id = t.Id,
				Train = new TrainDto
				{
					Id = t.Train.Id,
					TrainName = t.Train.TrainName,
					Source = t.Train.Source,
					Destination = t.Train.Destination,
					DepartureTime = t.Train.DepartureTime,
					ArrivalTime = t.Train.ArrivalTime
				},
				PassengerName = t.PassengerName,
				TravelDate = t.TravelDate,
				TicketTier = t.TicketTier,
				Price = t.Price,
				PaymentStatus = t.PaymentStatus,
				SeatNumber = t.SeatNumber
			})
			.ToListAsync();

		return Ok(tickets);
	}

	// 🔹 Book a Ticket (CREATE + Reserve Seat)
	[HttpPost]
	public async Task<ActionResult<Ticket>> BookTicket(CreateTicketDto ticketDto)
	{
		var train = await _context.Trains.FindAsync(ticketDto.TrainId);
		if (train == null) return NotFound(new { message = "Train not found." });

		var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatNumber == ticketDto.SeatNumber && s.TrainId == ticketDto.TrainId);
		if (seat == null) return NotFound(new { message = "Seat not found." });
		if (seat.IsReserved) return BadRequest(new { message = "Seat already reserved." });

		// Reserve the seat
		seat.IsReserved = true;
		await _context.SaveChangesAsync();

		decimal price = ticketDto.TicketTier switch
		{
			"Economy" => 25.00m,
			"Business" => 50.00m,
			"First-Class" => 100.00m,
			_ => throw new ArgumentException("Invalid ticket tier.")
		};

		var ticket = new Ticket
		{
			TrainId = ticketDto.TrainId,
			PassengerName = ticketDto.PassengerName,
			TravelDate = (DateTime)ticketDto.TravelDate,
			TicketTier = ticketDto.TicketTier,
			Price = price,
			PaymentStatus = "Pending",
			SeatNumber = ticketDto.SeatNumber
		};

		_context.Tickets.Add(ticket);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
	}

	// 🔹 Update Ticket Details (UPDATE)
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateTicket(int id, UpdateTicketDto ticketDto)
	{
		var ticket = await _context.Tickets.FindAsync(id);
		if (ticket == null) return NotFound(new { message = "Ticket not found." });

		ticket.PassengerName = ticketDto.PassengerName;
		ticket.TravelDate = (DateTime)ticketDto.TravelDate;
		ticket.TicketTier = ticketDto.TicketTier;
		ticket.PaymentStatus = ticketDto.PaymentStatus;

		await _context.SaveChangesAsync();
		return Ok(new { message = "Ticket updated successfully." });
	}

	// 🔹 Cancel a Ticket (DELETE + Free the Seat)
	[HttpDelete("{id}")]
	public async Task<IActionResult> CancelTicket(int id)
	{
		var ticket = await _context.Tickets.FindAsync(id);
		if (ticket == null) return NotFound(new { message = "Ticket not found." });

		var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatNumber == ticket.SeatNumber && s.TrainId == ticket.TrainId);
		if (seat != null)
		{
			seat.IsReserved = false; // Free the seat
		}

		_context.Tickets.Remove(ticket);
		await _context.SaveChangesAsync();
		return Ok(new { message = "Ticket canceled successfully, seat is now available." });
	}
}