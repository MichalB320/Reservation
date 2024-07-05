using Microsoft.EntityFrameworkCore;
using Reservation.DTOs;
using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.DbContexts;

public class ReservroomDbContext : DbContext
{
    public ReservroomDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ReservationDTO> Reservations { get; set; }
}
