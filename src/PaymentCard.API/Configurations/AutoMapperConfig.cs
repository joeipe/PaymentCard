namespace PaymentCard.API.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}