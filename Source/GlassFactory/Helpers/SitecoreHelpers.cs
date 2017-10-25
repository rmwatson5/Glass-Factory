namespace SBR.GlassFactory.Helpers
{
    using System;
    using System.Linq;

    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;

    public static class SitecoreHelpers
    {
        public static Item GetAncestorWithTemplate(
          this Item item,
          Guid templateId,
          bool includeSelf = false,
          bool includeBaseTemplates = false)
        {
            if (item == null)
            {
                return null;
            }

            if (includeSelf && item.HasTemplate(templateId, includeBaseTemplates))
            {
                return item;
            }

            var ancestors = item.Axes.GetAncestors();
            return ancestors.FirstOrDefault(ancestor => ancestor.HasTemplate(templateId, includeBaseTemplates));
        }

        public static bool HasTemplate(this Item item, Guid templateId, bool includeBaseTemplates = false)
        {
            if (item.TemplateID.Guid.Equals(templateId))
            {
                return true;
            }

            var template = TemplateManager.GetTemplate(item);
            if (template == null)
            {
                return false;
            }

            var baseTemplates = template.GetBaseTemplates();
            var allIds = baseTemplates.Select(bt => bt.ID);
            if (includeBaseTemplates && allIds.Any(bt => bt.Guid.Equals(templateId)))
            {
                return true;
            }

            return false;
        }
    }
}
