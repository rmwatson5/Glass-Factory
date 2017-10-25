using System;

namespace SBR.GlassFactory
{
    public interface IGlassSpawn
    {
        /// <summary>
        /// Gets the glass type from template.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns></returns>
        Type GetGlassTypeFromTemplate(Guid templateId);

        /// <summary>
        /// Gets the type of the template identifier from.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        Guid GetTemplateIdFromType(Type interfaceType);
    }
}