using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Common.Db;

public static class ToDoListContextExstensions
{
    public static IServiceCollection AddToDoListContext(this IServiceCollection services,
        string connect = "")
    {
        if (connect == "")
        {
            connect = "Data Source=.;" +
                "Initial Catalog=ToDoList;" +
                "Integrated Security=true;" +
                "MultipleActiveResultSets=true;" +
                "TrustServerCertificate=True;";
        }

        services.AddDbContext<ToDoListContext>(option => option.UseSqlServer(connect));

        return services;
    }
}
