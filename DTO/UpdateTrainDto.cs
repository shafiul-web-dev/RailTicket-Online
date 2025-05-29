using System.ComponentModel.DataAnnotations;

namespace RailTicket_Online.DTO
{
	public class UpdateTrainDto
	{
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Train name must be between 3 to 50 characters.")]
		public string TrainName { get; set; }

		public string Source { get; set; }

		public string Destination { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? DepartureTime { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? ArrivalTime { get; set; }
	}
}
