namespace RailTicket_Online.DTO
{
	public class TrainDto
	{
		public int Id { get; set; }
		public string TrainName { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
		public DateTime? DepartureTime { get; set; }
		public DateTime? ArrivalTime { get; set; }
	}
}
