using System;

namespace SBR.GlassFactory
{
    public interface IGlassSpawn
    {
        Type GetGlassTypeFromTemplate(Guid templateId);
    }
}