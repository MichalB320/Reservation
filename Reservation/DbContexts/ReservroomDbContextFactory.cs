using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.DbContexts;

public class ReservroomDbContextFactory
{
    private readonly string _connectionString;

    public ReservroomDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ReservroomDbContext CreateDbContext()
    {
        DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

        return new ReservroomDbContext(options);
    }
}
