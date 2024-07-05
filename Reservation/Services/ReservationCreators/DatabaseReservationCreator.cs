using Reservation.DbContexts;
using Reservation.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationCreators;

public class DatabaseReservationCreator : IReservationCreator
{
    private readonly ReservroomDbContextFactory _dbContextFactory;

    public DatabaseReservationCreator(ReservroomDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task CreateReservation(Models.Reservation reservation)
    {
        using (ReservroomDbContext context = _dbContextFactory.CreateDbContext())
        {
            ReservationDTO reservationDTO = ToReservationDTO(reservation);

            context.Reservations.Add(reservationDTO);
            
            await context.SaveChangesAsync();
        }
    }

    private ReservationDTO ToReservationDTO(Models.Reservation reservation)
    {
        return new ReservationDTO()
        {
            FloorNumber = reservation.RoomID?.FloorNumber ?? 0,
            RoomNumber = reservation.RoomID?.RoomNumber ?? 0,
            Username = reservation.Username,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
        };
    }
}
