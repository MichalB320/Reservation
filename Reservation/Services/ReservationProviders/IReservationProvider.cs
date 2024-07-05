using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationProviders;

public interface IReservationProvider
{
    Task<IEnumerable<Models.Reservation>> GetAllReservations();
}
