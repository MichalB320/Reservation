using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models;

public class Hotel
{
    private readonly ReservationBook _reservationBook;
    public string Name { get; }

    public Hotel(string name, ReservationBook reservationBook)
    {
        Name = name;
        _reservationBook = reservationBook;
    }

    //public IEnumerable<Reservation> GetReservationsForUser(string username)
    //{
    //    return _reservationBook.GetReservationsForUser(username);
    //}

    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        return await _reservationBook.GetAllReservations();
    }

    public async Task MakeReservations(Reservation reservation)
    {
        await _reservationBook.AddReservation(reservation);
    }
}
