using Microsoft.EntityFrameworkCore;
using RailTicket_Online.Models;

namespace RailTicket_Online.Data
{
	public class ApplicationDbContext: DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{
		}
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<Train> Trains { get; set; }
		public DbSet<Seat> Seats { get; set; }

	}
}
