namespace RailTicket_Online.DTO
{
	public class TicketDto
	{
		public int Id { get; set; }
		public TrainDto Train { get; set; }
		public string PassengerName { get; set; }
		public DateTime TravelDate { get; set; }
		public string TicketTier { get; set; }
		public decimal Price { get; set; }
		public string PaymentStatus { get; set; }
		public int SeatNumber { get; set; }
	}
}
