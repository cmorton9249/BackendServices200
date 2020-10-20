using LibraryApi.Domain;
using LibraryApi.Models.Reservations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Policy;

namespace LibraryApi.Controllers
{
	public class ReservationsController : ControllerBase
	{
		private readonly LibraryDataContext _context;

		public ReservationsController(LibraryDataContext context)
		{
			_context = context;
		}

		[HttpPost("reservations")]
		public ActionResult AddReservation([FromBody] PostReservationRequest request)
		{
			return Ok();
		}
		
		[HttpGet("reservations/{id}")]
		public ActionResult GetReservationById(int id)
		{

			return Ok();
		}
	}
}
