using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Exceptions;

public class ReservationConflictException : Exception
{
    public Reservation.Models.Reservation ExistingReservation { get; }
    public Reservation.Models.Reservation IncomingReservation { get; }

    public ReservationConflictException(Models.Reservation existingReservation, Models.Reservation incomingReservation)
    {
        ExistingReservation = existingReservation;
        IncomingReservation = incomingReservation;
    }

    public ReservationConflictException(string? message, Models.Reservation existingReservation = null, Models.Reservation incomingReservation = null) : base(message)
    {
        ExistingReservation = existingReservation;
        IncomingReservation = incomingReservation;
    }

    public ReservationConflictException(string? message, Exception? innerException, Models.Reservation existingReservation, Models.Reservation incomingReservation) : base(message, innerException)
    {
        ExistingReservation = existingReservation;
        IncomingReservation = incomingReservation;
    }

    protected ReservationConflictException(SerializationInfo info, StreamingContext context, Models.Reservation existingReservation, Models.Reservation incomingReservation) : base(info, context)
    {
        ExistingReservation = existingReservation;
        IncomingReservation = incomingReservation;
    }
}
