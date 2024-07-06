using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reservation.DbContexts;
using Reservation.Models;
using Reservation.Services;
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

        //private readonly Hotel _hotel;
        //private readonly NavigationStore _navigationStore;
        //private readonly ReservroomDbContextFactory _reservroomDbContextFactory;
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton(new ReservroomDbContextFactory(CONNECTION_STRING));
                services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
                services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
                services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();

                services.AddTransient<ReservationBook>();
                
                services.AddSingleton(s => new Hotel("My Suites", s.GetRequiredService<ReservationBook>()));
                services.AddSingleton<NavigationStore>();

                services.AddTransient((s) => CreateReservationListingViewModel(s));
                services.AddSingleton<Func<ReservationListingViewModel>>((s) => () => s.GetRequiredService<ReservationListingViewModel>());
                services.AddSingleton<NavigationService<ReservationListingViewModel>>();

                services.AddTransient<MakeReservationViewModel>();
                services.AddSingleton<Func<MakeReservationViewModel>>((s) => () => s.GetRequiredService<MakeReservationViewModel>());
                services.AddSingleton<NavigationService<MakeReservationViewModel>>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton(s => new MainWindow() { DataContext = s.GetRequiredService<MainViewModel>() });
            }).Build();

            //_reservroomDbContextFactory = new ReservroomDbContextFactory(CONNECTION_STRING);
            //IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservroomDbContextFactory);
            //IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservroomDbContextFactory);
            //IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservroomDbContextFactory);

            //ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflictValidator);

            //_hotel = new Hotel("My Suites", reservationBook);
            //_navigationStore = new NavigationStore();
        }

        private ReservationListingViewModel CreateReservationListingViewModel(IServiceProvider s)
        {
            return ReservationListingViewModel.LoadViewModel(s.GetRequiredService<Hotel>(), s.GetRequiredService<NavigationService<MakeReservationViewModel>>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            ReservroomDbContextFactory reservroomDbContextFactory = _host.Services.GetRequiredService<ReservroomDbContextFactory>();
            using (ReservroomDbContext dbContext = reservroomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            //NavigationStore navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            //navigationStore.CurrentViewModel = CreateReservationViewModel();

            NavigationService<ReservationListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<ReservationListingViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            
            base.OnStartup(e);
        }

        //private MakeReservationViewModel CreateMakeReservationViewModel()
        //{
        //    return new MakeReservationViewModel(_hotel, new Services.NavigationService(_navigationStore, CreateReservationViewModel));
        //}

        //private ReservationListingViewModel CreateReservationViewModel()
        //{
        //    return ReservationListingViewModel.LoadViewModel(_hotel, new Services.NavigationService(_navigationStore, CreateMakeReservationViewModel));
        //}
        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
