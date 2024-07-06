using CommunityToolkit.Mvvm.Input;
using Reservation.Exceptions;
using Reservation.Models;
using Reservation.Services;
using System.Windows;
using System.Windows.Input;

namespace Reservation.ViewModels;

public class MakeReservationViewModel : ViewModelBase
{
    private string _username = string.Empty;
    public string Username { get => _username;
        set 
        { 
            _username = value; 
            OnPropertyChanged(nameof(Username)); } }

    private int _floorNumber;
    public int FloorNumber { get => _floorNumber; set { 
            _floorNumber = value; 
            OnPropertyChanged(nameof(FloorNumber)); } }

    private int _roomNumber;
    public int RoomNumber { get => _roomNumber; set { 
            _roomNumber = value; 
            OnPropertyChanged(nameof(RoomNumber)); } }

    private DateTime _startDate = DateTime.Today;
    public DateTime StartDate { get => _startDate; set { 
            _startDate = value; 
            OnPropertyChanged(nameof(StartDate)); } }

    private DateTime _endDate = DateTime.Today;
    public DateTime EndDate { get => _endDate; set { 
            _endDate = value; 
            OnPropertyChanged(nameof(EndDate)); } }

    public ICommand SubmitCommand { get; }
    public ICommand CancelCommand { get; }

    private readonly Hotel _hotel;

    public MakeReservationViewModel(Hotel hotel, NavigationService<ReservationListingViewModel> navigationService)
    {
        SubmitCommand = new AsyncRelayCommand(() => OnClickSubmit(navigationService));
        CancelCommand = new RelayCommand(() => OnClickCancel(navigationService));

        _hotel = hotel;
    }

    private void OnClickCancel(NavigationService<ReservationListingViewModel> reservationViewNavigationService)
    {
        reservationViewNavigationService.Navigate();
    }

    private async Task OnClickSubmit(NavigationService<ReservationListingViewModel> reservationViewNavigationService)
    {
        Reservation.Models.Reservation reservation = new Models.Reservation(new RoomID(FloorNumber, RoomNumber), Username, StartDate, EndDate);

        try
        {
            await _hotel.MakeReservations(reservation);
            MessageBox.Show("Successfully reserved room.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            reservationViewNavigationService.Navigate();
        }
        catch (ReservationConflictException)
        {
            MessageBox.Show("This room is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to make reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
