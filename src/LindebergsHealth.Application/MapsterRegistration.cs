using LindebergsHealth.Application.Termine.Mapping;

namespace LindebergsHealth.Application
{
    public static class MapsterRegistration
    {
        public static void RegisterMappings()
        {
            TerminMappingConfig.Register();
        }
    }
}
