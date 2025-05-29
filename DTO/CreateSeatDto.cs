using System.ComponentModel.DataAnnotations;

namespace RailTicket_Online.DTO
{
	public class CreateSeatDto
	{
		[Required(ErrorMessage = "Train ID is required.")]
		public int TrainId { get; set; }

		[Required(ErrorMessage = "Seat number is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "Seat number must be a positive value.")]
		public int SeatNumber { get; set; }

		[Required(ErrorMessage = "Reservation status is required.")]
		public bool IsReserved { get; set; } // True when booked

	}
}
