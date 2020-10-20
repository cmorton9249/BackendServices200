using LibraryApi.Models.Reservations;
using System;

namespace LibraryApi.Domain
{
	public class Reservation
	{
		public int Id { get; set; }
		public string For { get; set; }
		public string Items { get; set; }
		public DateTime? AvailableOn { get; set; }
		public ReservationStatus Status { get; set; }
	}
}
