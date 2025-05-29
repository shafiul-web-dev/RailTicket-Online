using System.ComponentModel.DataAnnotations;

namespace RailTicket_Online.DTO
{
	public class UpdateTicketDto
	{
		[StringLength(100, MinimumLength = 3, ErrorMessage = "Passenger name must be between 3 to 100 characters.")]
		public string PassengerName { get; set; }

		[DataType(DataType.Date)]
		public DateTime? TravelDate { get; set; }

		[StringLength(20, ErrorMessage = "Ticket tier must not exceed 20 characters.")]
		public string TicketTier { get; set; }

		[StringLength(20, ErrorMessage = "Payment status must not exceed 20 characters.")]
		public string PaymentStatus { get; set; }
	}
}
