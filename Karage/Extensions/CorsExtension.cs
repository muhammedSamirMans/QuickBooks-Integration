namespace Karage.APIs.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddAngularCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    // Allow requests from Angular app running on localhost:4200 (or the appropriate domain for production)
                    policy.WithOrigins("http://localhost:4200") // Allow Angular app URL
                          .AllowAnyHeader()    // Allow any headers
                          .AllowAnyMethod();   // Allow any HTTP methods (GET, POST, PUT, DELETE)
                });
            });
            return services;
        }
    }
}
