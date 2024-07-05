using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationCreators;

public interface IReservationCreator
{
    Task CreateReservation(Models.Reservation reservation);
}
