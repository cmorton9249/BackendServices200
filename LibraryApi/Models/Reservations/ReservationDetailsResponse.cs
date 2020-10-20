using System;

namespace LibraryApi.Models.Reservations
{
	public class ReservationDetailsResponse
	{
		public int Id { get; set; }
		public string For { get; set; }
		public string Items { get; set; }
		public DateTime? AvailableOn { get; set; }
		public ReservationStatus Status { get; set; }
	}
}
