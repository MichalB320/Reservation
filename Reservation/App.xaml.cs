using Microsoft.EntityFrameworkCore;
using Reservation.DbContexts;
using Reservation.Models;
using Reservation.Services.ReservationConflictValidators;
using Reservation.Services.ReservationCreators;
using Reservation.Services.ReservationProviders;
using Reservation.Stores;
using Reservation.ViewModels;
using System.Windows;

namespace Reservation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Data Source=reservroom.db";

        private readonly Hotel _hotel;
        private readonly NavigationStore _navigationStore;
        private readonly ReservroomDbContextFactory _reservroomDbContextFactory;

        public App()
        {
            _reservroomDbContextFactory = new ReservroomDbContextFactory(CONNECTION_STRING);
            IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservroomDbContextFactory);
            IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservroomDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservroomDbContextFactory);

            ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflictValidator);

            _hotel = new Hotel("My Suites", reservationBook);
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(CONNECTION_STRING).Options;
            using (ReservroomDbContext dbContext = _reservroomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            _navigationStore.CurrentViewModel = CreateReservationViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            
            base.OnStartup(e);
        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotel, new Services.NavigationService(_navigationStore, CreateReservationViewModel));
        }

        private ReservationListingViewModel CreateReservationViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotel, new Services.NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }
    }

}
