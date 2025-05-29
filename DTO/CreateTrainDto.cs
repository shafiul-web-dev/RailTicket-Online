using System.ComponentModel.DataAnnotations;

namespace RailTicket_Online.DTO
{
	public class CreateTrainDto
	{
		[Required(ErrorMessage = "Train name is required.")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Train name must be between 3 to 50 characters.")]
		public string TrainName { get; set; }

		[Required(ErrorMessage = "Source station is required.")]
		public string Source { get; set; }

		[Required(ErrorMessage = "Destination station is required.")]
		public string Destination { get; set; }

		[Required(ErrorMessage = "Departure time is required.")]
		public DateTime? DepartureTime { get; set; }

		[Required(ErrorMessage = "Arrival time is required.")]
		public DateTime? ArrivalTime { get; set; }
	}
}
