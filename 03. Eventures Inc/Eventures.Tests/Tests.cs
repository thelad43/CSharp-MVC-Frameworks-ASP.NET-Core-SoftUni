namespace Eventures.Tests
{
    using AutoMapper;
    using Common.Mapping;

    public class Tests
    {
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutomapperProfile>());
                testsInitialized = true;
            }
        }
    }
}