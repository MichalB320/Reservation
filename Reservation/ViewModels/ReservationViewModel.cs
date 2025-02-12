﻿using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.ViewModels;

public class ReservationViewModel : ViewModelBase
{
    private readonly Reservation.Models.Reservation _reservation;

    public string RoomID => _reservation.RoomID.ToString();
    public string Username => _reservation.Username;
    public string StartDate => _reservation.StartTime.ToString("d");
    public string EndDate => _reservation.EndTime.ToString("d");

    public ReservationViewModel(Reservation.Models.Reservation reservation)
    {
        _reservation = reservation;
    }
}
