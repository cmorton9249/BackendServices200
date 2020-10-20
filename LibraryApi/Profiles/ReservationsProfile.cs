using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Reservations;

namespace LibraryApi.Profiles
{
	public class ReservationsProfile : Profile
	{
		public ReservationsProfile()
		{
			CreateMap<Reservation, ReservationDetailsResponse>();
			CreateMap<PostReservationRequest, Reservation>();

		}
	}
}