using Microsoft.EntityFrameworkCore;
using Reservation.DbContexts;
using Reservation.DTOs;
using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationProviders;

public class DatabaseReservationProvider : IReservationProvider
{
    private readonly ReservroomDbContextFactory _dbContextFactory;

    public DatabaseReservationProvider(ReservroomDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<Models.Reservation>> GetAllReservations()
    {
        using (ReservroomDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<ReservationDTO> reservationDTOs = await context.Reservations.ToListAsync();

            return reservationDTOs.Select(r => new Models.Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime));
        }
    }
}
