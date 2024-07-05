using Reservation.Exceptions;
using Reservation.Services.ReservationConflictValidators;
using Reservation.Services.ReservationCreators;
using Reservation.Services.ReservationProviders;

namespace Reservation.Models;

public class ReservationBook
{
    //private readonly List<Reservation> _reservations;
    private readonly IReservationProvider _reservationProvider;
    private readonly IReservationCreator _reservationCreator;
    private readonly IReservationConflictValidator _reservationConflictValidator;

    public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflictValidator reservationConflictValidator)
    {
        //_reservations = new List<Reservation>();
        _reservationProvider = reservationProvider;
        _reservationCreator = reservationCreator;
        _reservationConflictValidator = reservationConflictValidator;
    }

    //public IEnumerable<Reservation> GetReservationsForUser(string username)
    //{
    //    return _reservations.Where(r => r.Username == username);
    //}

    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        //return _reservations;
        return await _reservationProvider.GetAllReservations();
    }

    public async Task AddReservation(Reservation reservation)
    {
        Reservation conflictingReservation = await _reservationConflictValidator.GetConflictingReservation(reservation);

        if (conflictingReservation != null)
            throw new ReservationConflictException(conflictingReservation, reservation);

        await _reservationCreator.CreateReservation(reservation);
    }
}
