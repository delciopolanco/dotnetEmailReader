using System;
using EmailReader.Services.MailReader;
using Microsoft.Extensions.DependencyInjection;

namespace EmailReader.ExtentionMethods
{
    public static class ServiceExtentions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IMailReader, MailReader>();
        }
    }
}
