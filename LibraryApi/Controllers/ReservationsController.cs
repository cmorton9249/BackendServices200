using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Filters;
using LibraryApi.Models.Reservations;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
	public class ReservationsController : ControllerBase
	{
		private readonly LibraryDataContext _context;
		private readonly IMapper _mapper;
		private readonly MapperConfiguration _config;
		private readonly ILogReservations _reservationsLogger;

		public ReservationsController(LibraryDataContext context, IMapper mapper, MapperConfiguration config, ILogReservations reservationsLogger)
		{
			_context = context;
			_mapper = mapper;
			_config = config;
			_reservationsLogger = reservationsLogger;
		}

		[HttpPost("reservations")]
		[ValidateModel]
		public async Task<ActionResult> AddReservationAsync([FromBody] PostReservationRequest request)
		{
			var reservation = _mapper.Map<Reservation>(request);
			_context.Reservations.Add(reservation);
			await _context.SaveChangesAsync();
			var response = _mapper.Map<ReservationDetailsResponse>(reservation);
			response.Status = ReservationStatus.Pending;
			await _reservationsLogger.WriteAsync(reservation);
			return CreatedAtRoute("reservations#getbyId", new { id = response.Id }, response);
		}

		[HttpGet("reservations/{id}", Name = "reservations#getbyId")]
		public async Task<ActionResult> GetReservationByIdAsync(int id)
		{
			var reservation = await _context.Reservations
				.ProjectTo<ReservationDetailsResponse>(_config)
				.SingleOrDefaultAsync(x => x.Id == id);
			return this.Maybe(reservation);
		}

		[HttpPost("reservations/accepted")]
		[ValidateModel]
		public async Task<ActionResult> ApproveReservationAsync([FromBody] ReservationDetailsResponse reservation)
		{
			var res = await _context.Reservations.SingleOrDefaultAsync(r => r.Id == reservation.Id);
			if (res == null)
			{
				return BadRequest();
			}

			res.Status = ReservationStatus.Accepted;
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPost("reservations/rejected")]
		[ValidateModel]
		public async Task<ActionResult> RejectReservationAsync([FromBody] ReservationDetailsResponse reservation)
		{
			var res = await _context.Reservations.SingleOrDefaultAsync(r => r.Id == reservation.Id);
			if (res == null)
			{
				return BadRequest();
			}

			res.Status = ReservationStatus.Rejected;
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("/reservations")]
		public async Task<ActionResult> AllReservations()
		{
			var data = await _context.Reservations.ProjectTo<ReservationDetailsResponse>(_config).ToListAsync();
			return Ok(new { data, status = "all" });
		}

		[HttpGet("/reservations/accepted")]
		public async Task<ActionResult> AcceptedReservations()
		{
			var data = await _context.Reservations.Where(r => r.Status == ReservationStatus.Accepted).ProjectTo<ReservationDetailsResponse>(_config).ToListAsync();
			return Ok(new { data, status = "Accepted" });
		}

		[HttpGet("/reservations/rejected")]
		public async Task<ActionResult> RejectedReservations()
		{
			var data = await _context.Reservations.Where(r => r.Status == ReservationStatus.Rejected).ProjectTo<ReservationDetailsResponse>(_config).ToListAsync();
			return Ok(new { data, status = "Rejected" });
		}

		[HttpGet("/reservations/pending")]
		public async Task<ActionResult> PendingReservations()
		{
			var data = await _context.Reservations.Where(r => r.Status == ReservationStatus.Pending).ProjectTo<ReservationDetailsResponse>(_config).ToListAsync();
			return Ok(new { data, status = "Pending" });
		}
	}
}
