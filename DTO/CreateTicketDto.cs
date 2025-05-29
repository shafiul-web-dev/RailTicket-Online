using System.ComponentModel.DataAnnotations;

namespace RailTicket_Online.DTO
{
	public class CreateTicketDto
	{
		[Required(ErrorMessage = "Train ID is required.")]
		public int TrainId { get; set; }

		[Required(ErrorMessage = "Passenger name is required.")]
		public string PassengerName { get; set; }

		[Required(ErrorMessage = "Travel date is required.")]
		public DateTime? TravelDate { get; set; }

		[Required(ErrorMessage = "Ticket tier must be specified.")]
		public string TicketTier { get; set; }

		[Required(ErrorMessage = "Seat number is required.")]
		public int SeatNumber { get; set; }
	}
}
