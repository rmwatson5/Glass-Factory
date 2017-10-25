using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;

using Sitecore.Data;
using Sitecore.Data.Items;

namespace SBR.GlassFactory
{
    using Helpers;

    public class GlassFactory : IGlassFactory
    {
        private static IGlassFactory current;
        public static IGlassFactory Current => current ?? (current = new GlassFactory());

        protected readonly IGlassSpawn GlassSpawn;

        public GlassFactory()
        {
            this.GlassSpawn = new GlassSpawn();
        }

        public TCi GetGlassItem<TCi>(Item item, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class
        {
            if (item == null)
            {
                return null;
            }

            TCi contentItem;
            var inheritedType = this.GlassSpawn.GetGlassTypeFromTemplate(item.TemplateID.Guid);
            using (var sitecoreService = new SitecoreService(Sitecore.Context.Database))
            {
                if (inheritedType == null)
                {
                    contentItem = sitecoreService.GetItem<TCi>(item.ID.Guid);
                }
                else
                {
                    contentItem = sitecoreService.CreateType(inheritedType, item, isLazy, inferType, null) as TCi;
                }
            }

            return this.FilterContentItem(contentItem, filter);
        }

        public TCi GetGlassItem<TCi>(Guid itemId, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(new ID(itemId));
            return this.GetGlassItem(sitecoreItem, isLazy, inferType, filter);
        }

        public TCi GetGlassItem<TCi>(string itemPath, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(itemPath);
            return this.GetGlassItem(sitecoreItem, isLazy, inferType, filter);
        }

        public IEnumerable<TCi> GetChildren<TCi>(ID parentId, bool isLazy = false, bool inferType = true, bool deep = false,
            Func<TCi, bool> filter = null) where TCi : class
        {
            var filteredChildren = this.GetChildSitecoreItems<TCi>(parentId, deep);
            var childItems = filteredChildren?.Select(si => this.GetGlassItem(si, isLazy, inferType, filter));
            return childItems?.Where(child => child != default(TCi));
        }

        public TCi GetChild<TCi>(Guid parentId, bool isLazy = false, bool inferType = true, bool deep = false)
            where TCi : class
        {
            var filteredChildren = this.GetChildSitecoreItems<TCi>(new ID(parentId), deep);
            var firstChild = filteredChildren?.FirstOrDefault();
            return this.GetGlassItem<TCi>(firstChild, isLazy, inferType);
        }

        public TCi GetParent<TCi>(Guid itemId, bool includeSelf = false, bool includeBaseTemplates = false)
            where TCi : class
        {
            var ancestorSitecoreItem = this.GetAncestor<TCi>(itemId, includeSelf, includeBaseTemplates);
            return this.GetGlassItem<TCi>(ancestorSitecoreItem);
        }

        public bool HasParent<TCi>(Guid itemId, bool includeSelf = false, bool includeBaseTemplates = false)
            where TCi : class
        {
            var ancestorSitecoreItem = this.GetAncestor<TCi>(itemId, includeSelf, includeBaseTemplates);
            return ancestorSitecoreItem != null;
        }

        public TCi GetSibling<TCi>(Guid itemId, bool previousSibling = false)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(new ID(itemId));
            var sibling = previousSibling ? sitecoreItem.Axes.GetPreviousSibling() : sitecoreItem.Axes.GetNextSibling();
            return this.GetGlassItem<TCi>(sibling);
        }

        public TCi GetCurrentGlassItem<TCi>(bool isLazy = false, bool inferType = true)
            where TCi : class
        {
            return this.GetGlassItem<TCi>(Sitecore.Context.Item, isLazy, inferType);
        }

        public TCi GetSiteRoot<TCi>()
            where TCi : class
        {
            if (string.IsNullOrWhiteSpace(Sitecore.Context.Site?.RootPath))
            {
                return null;
            }

            var item = Sitecore.Context.Database.GetItem(Sitecore.Context.Site?.RootPath);
            var siteRoot = this.GetGlassItem<TCi>(item);
            return siteRoot;
        }

        public TCi GetHomePage<TCi>()
            where TCi : class
        {
            if (string.IsNullOrWhiteSpace(Sitecore.Context.Site?.RootPath))
            {
                return null;
            }

            var item = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
            var homePage = this.GetGlassItem<TCi>(item);
            return homePage;
        }

        public bool HasTemplate(Type type, Item item)
        {
            if (item == null)
            {
                return false;
            }

            var templateId = this.GlassSpawn.GetTemplateIdFromType(type);
            return item.HasTemplate(templateId);
        }

        protected TCi FilterContentItem<TCi>(TCi contentItem, Func<TCi, bool> filter = null)
         where TCi : class
        {
            if (filter == null || filter(contentItem))
            {
                return contentItem;
            }

            return default(TCi);
        }

        protected Item GetAncestor<TCi>(Guid itemId, bool includeSelf = false, bool includeBaseTemplates = false)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(new ID(itemId));
            if (sitecoreItem == null)
            {
                return null;
            }

            var templateId = this.GlassSpawn.GetTemplateIdFromType(typeof(TCi));
            var ancestorSitecoreItem = sitecoreItem.GetAncestorWithTemplate(templateId, includeSelf, includeBaseTemplates);
            return ancestorSitecoreItem;
        }

        protected IEnumerable<Item> GetChildSitecoreItems<TCi>(
            ID parentId,
            bool deep = false)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(parentId);
            if (sitecoreItem.HasChildren == false)
            {
                return null;
            }

            var templateId = this.GlassSpawn.GetTemplateIdFromType(typeof(TCi));

            // If deep is selected, then get all the children recursively
            var children = deep ? sitecoreItem.Axes.GetDescendants() : sitecoreItem.Children.ToArray();
            return children.Where(c => templateId.Equals(Guid.Empty) || c.HasTemplate(templateId, true));
        }
    }
}
