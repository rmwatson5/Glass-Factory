using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace SBR.GlassFactory
{
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

            return  this.FilterContentItem(contentItem, filter);
        }

        public IEnumerable<TCi> GetChildren<TCi>(ID parentId, bool isLazy = false, bool inferType = true, bool deep = false,
            Func<TCi, bool> filter = null) where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(parentId);
            if (sitecoreItem.HasChildren == false)
            {
                return null;
            }

            // If deep is selected, then get all the children recursively
            var children = deep ? sitecoreItem.Axes.GetDescendants() : sitecoreItem.Children.ToArray();
            var childItems = children.Select(si => this.GetGlassItem(si, isLazy, inferType, filter));
            return childItems.Where(child => child != default(TCi));
        }

        public TCi GetGlassItem<TCi>(Guid itemId, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class
        {
            var sitecoreItem = Sitecore.Context.Database.GetItem(new ID(itemId));
            return this.GetGlassItem(sitecoreItem, isLazy, inferType, filter);
        }

        public TCi GetCurrentGlassItem<TCi>(bool isLazy = false, bool inferType = true)
            where TCi : class
        {
            return this.GetGlassItem<TCi>(Sitecore.Context.Item, isLazy, inferType);
        }

        public TCi GetSiteRoot<TCi>()
            where TCi : class
        {
            var item = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.RootPath);
            var siteRoot = this.GetGlassItem<TCi>(item);
            return siteRoot;
        }

        public TCi GetHomePage<TCi>()
            where TCi : class
        {
            var item = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
            var homePage = this.GetGlassItem<TCi>(item);
            return homePage;
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
    }
}
