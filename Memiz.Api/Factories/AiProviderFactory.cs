using Memiz.Api.Interfaces;

namespace Memiz.Api.Factories
{
    public class AiProviderFactory
    {
        private readonly IEnumerable<IAiProvider> _providers;

        public AiProviderFactory(IEnumerable<IAiProvider> providers)
        {
            _providers = providers;
        }

        public IAiProvider GetProvider(string name)
        {
            var provider = _providers.FirstOrDefault(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
                throw new Exception($"AI provider '{name}' not found");

            return provider;
        }
    }
}
