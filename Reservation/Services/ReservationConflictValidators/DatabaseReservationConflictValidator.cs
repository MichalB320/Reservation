using Microsoft.EntityFrameworkCore;
using Reservation.DbContexts;
using Reservation.DTOs;
using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationConflictValidators;

public class DatabaseReservationConflictValidator : IReservationConflictValidator
{
    private readonly ReservroomDbContextFactory _dbContextFactory;

    public DatabaseReservationConflictValidator(ReservroomDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Models.Reservation> GetConflictingReservation(Models.Reservation reservation)
    {
        using (ReservroomDbContext context = _dbContextFactory.CreateDbContext())
        {
            //return await context.Reservations.Select(r => new Models.Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime)).AnyAsync(r => r.Conflicts(reservation));
            ReservationDTO reservationDTO = await context.Reservations
                .Where(r => r.FloorNumber == reservation.RoomID.FloorNumber)
                .Where(r => r.RoomNumber == reservation.RoomID.RoomNumber)
                .Where(r => r.StartTime > reservation.StartTime)
                .Where(r => r.EndTime < reservation.EndTime)
                .FirstOrDefaultAsync();

            if (reservationDTO == null)
            {
                return null;
            }

            return new Models.Reservation(new RoomID(reservationDTO.FloorNumber, reservationDTO.RoomNumber), reservationDTO.Username, reservationDTO.StartTime, reservationDTO.EndTime);

            //Select(r => new Models.Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime))
        }
    }
}
