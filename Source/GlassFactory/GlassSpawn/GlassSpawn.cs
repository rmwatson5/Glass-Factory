using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace SBR.GlassFactory
{
    using Sitecore.Mvc.Extensions;

    public class GlassSpawn : IGlassSpawn
    {
        protected static IEnumerable<Type> GlassTypes;

        public Type GetGlassTypeFromTemplate(Guid templateId)
        {
            if (GlassTypes == null)
            {
                this.LoadGlassTypes();
            }

            return GlassTypes.FirstOrDefault(t => this.IsCorrectType(t, templateId));
        }

        public Type GetAssignableType(Type interfaceType)
        {
            return GlassTypes.FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && this.IsGlassType(t));
        }

        public Guid GetTemplateIdFromType(Type interfaceType)
        {
            var sitecoreTypeAttribute = Attribute.GetCustomAttribute(interfaceType, typeof(SitecoreTypeAttribute)) as SitecoreTypeAttribute;
            if (string.IsNullOrWhiteSpace(sitecoreTypeAttribute?.TemplateId))
            {
                return Guid.Empty;
            }

            return sitecoreTypeAttribute.TemplateId.ToGuid();
        }

        protected void LoadGlassTypes()
        {
            var assemblyName = ConfigurationManager.AppSettings["GlassModel"];
            var loadedModel = Assembly.Load(assemblyName);
            GlassTypes = loadedModel.GetTypes().Where(this.IsGlassType);
        }

        protected bool IsCorrectType(Type t, Guid templateId)
        {
            var matchedTemplateId = this.GetTemplateIdFromType(t);
            return matchedTemplateId.Equals(templateId);
        }

        protected bool IsGlassType(Type t)
        {
            var hasSitecoreTypeAttribute =
                (Attribute.GetCustomAttribute(t, typeof(SitecoreTypeAttribute)) as SitecoreTypeAttribute) != null;

            return hasSitecoreTypeAttribute && t.IsInterface == false;
        }
    }
}
