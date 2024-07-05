using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.ReservationConflictValidators;

public interface IReservationConflictValidator
{
    Task<Models.Reservation> GetConflictingReservation(Models.Reservation reservation);
}
