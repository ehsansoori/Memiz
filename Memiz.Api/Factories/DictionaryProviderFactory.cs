using Memiz.Api.Interfaces;

namespace Memiz.Api.Factories
{
    public class DictionaryProviderFactory
    {
        private readonly IEnumerable<IDictionaryProvider> _providers;

        public DictionaryProviderFactory(IEnumerable<IDictionaryProvider> providers)
        {
            _providers = providers;
        }

        public IDictionaryProvider GetProvider(string name)
        {
            var provider = _providers.FirstOrDefault(p => p.Name == name);

            if (provider == null)
                throw new Exception($"Dictionary provider '{name}' not found");

            return provider;
        }
    }
}
