namespace Memiz.Api.Templates
{
    public class TemplateFactory
    {
        private readonly IEnumerable<ICardTemplate> _templates;

        public TemplateFactory(IEnumerable<ICardTemplate> templates)
        {
            _templates = templates;
        }

        public ICardTemplate GetTemplate(string name)
        {
            var template = _templates.FirstOrDefault(t => t.Name == name);

            if (template == null)
                throw new Exception($"Template '{name}' not found");

            return template;
        }
    }
}
