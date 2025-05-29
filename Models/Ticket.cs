namespace RailTicket_Online.Models
{
	public class Ticket
	{
		public int Id { get; set; }
		public int TrainId { get; set; }  // 🔹 Foreign Key

		public string PassengerName { get; set; }
		public DateTime TravelDate { get; set; }
		public string TicketTier { get; set; }
		public decimal Price { get; set; }
		public string PaymentStatus { get; set; }
		public int SeatNumber { get; set; }

		// 🔹 Navigation Property for Train Relationship
		public Train Train { get; set; }
	}
}
