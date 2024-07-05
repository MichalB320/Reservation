using CommunityToolkit.Mvvm.Input;
using Reservation.Models;
using Reservation.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Reservation.ViewModels;

public class ReservationListingViewModel : ViewModelBase
{
    public ObservableCollection<ReservationViewModel> _reservations;

    public IEnumerable<ReservationViewModel> Reservations => _reservations;
    public ICommand MakeReservationCommand { get; }
    public ICommand LoadReservationsCommand { get; }

    public ReservationListingViewModel(Hotel hotel, NavigationService navigationService)
    {
        _reservations = new ObservableCollection<ReservationViewModel>();

        MakeReservationCommand = new RelayCommand(() => OnClickMakeReservation(navigationService));
        LoadReservationsCommand = new AsyncRelayCommand(() => Loading(hotel));

        //_reservations.Add(new ReservationViewModel(new Reservation.Models.Reservation(new RoomID(1, 2), "Michal", DateTime.Now, DateTime.Now)));
        //_reservations.Add(new ReservationViewModel(new Reservation.Models.Reservation(new RoomID(3, 2), "Jojo", DateTime.Now, DateTime.Now)));
        //_reservations.Add(new ReservationViewModel(new Reservation.Models.Reservation(new RoomID(2, 2), "Adam", DateTime.Now, DateTime.Now)));

        //UpdateReservations(hotel); 
    }

    private async Task Loading(Hotel hotel)
    {
        try
        {
            IEnumerable<Models.Reservation> reservations = await hotel.GetAllReservations();

            UpdateReservations(reservations);
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to make reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public static ReservationListingViewModel LoadViewModel(Hotel hotel, NavigationService makeReservationNavigationService)
    {
        ReservationListingViewModel viewModel = new ReservationListingViewModel(hotel, makeReservationNavigationService);

        viewModel.LoadReservationsCommand.Execute(null);

        return viewModel;
    }

    private void UpdateReservations(IEnumerable<Models.Reservation> reservations/*Hotel hotel*/)
    {
        _reservations.Clear();

        foreach (Reservation.Models.Reservation reservation in reservations)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }
    }

    private void OnClickMakeReservation(NavigationService navigationService)
    {
        navigationService.Navigate();
    }
}
