using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace SBR.GlassFactory
{
    public class GlassSpawn : IGlassSpawn
    {
        protected static IEnumerable<Type> GlassTypes;

        public Type GetGlassTypeFromTemplate(Guid templateId)
        {
            if (GlassTypes == null)
            {
                this.LoadGlassTypes();
            }

            return GlassTypes.FirstOrDefault(t => IsCorrectType(t, templateId));
        }

        public Type GetAssignableType(Type interfaceType)
        {
            return GlassTypes.FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && IsGlassType(t));
        }

        protected void LoadGlassTypes()
        {
            var assemblyName = ConfigurationManager.AppSettings["GlassModel"];
            var loadedModel = Assembly.Load(assemblyName);
            GlassTypes = loadedModel.GetTypes().Where(this.IsGlassType);
        }

        protected bool IsCorrectType(Type t, Guid templateId)
        {
            var sitecoreTypeAttribute = Attribute.GetCustomAttribute(t, typeof(SitecoreTypeAttribute)) as SitecoreTypeAttribute;
            if (string.IsNullOrWhiteSpace(sitecoreTypeAttribute?.TemplateId))
            {
                return false;
            }

            return sitecoreTypeAttribute.TemplateId
                .Equals(
                    templateId.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        protected bool IsGlassType(Type t)
        {
            var hasSitecoreTypeAttribute =
                (Attribute.GetCustomAttribute(t, typeof(SitecoreTypeAttribute)) as SitecoreTypeAttribute) != null;

            return hasSitecoreTypeAttribute && t.IsInterface == false;
        }
    }
}
