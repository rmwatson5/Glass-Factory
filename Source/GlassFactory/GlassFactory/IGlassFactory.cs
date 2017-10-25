using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace SBR.GlassFactory
{
    public interface IGlassFactory
    {
        /// <summary>
        /// Gets the glass item.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        TCi GetGlassItem<TCi>(Item item, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class;

        /// <summary>
        /// Gets the glass item.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        TCi GetGlassItem<TCi>(Guid itemId, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class;

        /// <summary>
        /// Gets the glass item.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="itemPath">The item path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        TCi GetGlassItem<TCi>(string itemPath, bool isLazy = false, bool inferType = true, Func<TCi, bool> filter = null)
            where TCi : class;

        /// <summary>
        /// Gets the current glass item.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        TCi GetCurrentGlassItem<TCi>(bool isLazy = false, bool inferType = true)
            where TCi : class;

        /// <summary>
        /// Gets the site root.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <returns></returns>
        TCi GetSiteRoot<TCi>() where TCi : class;

        /// <summary>
        /// Gets the home page.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <returns></returns>
        TCi GetHomePage<TCi>() where TCi : class;

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<TCi> GetChildren<TCi>(ID parentId, bool isLazy = false, bool inferType = true, bool deep = false,
            Func<TCi, bool> filter = null) where TCi : class;

        /// <summary>
        /// Gets the child.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        TCi GetChild<TCi>(Guid parentId, bool isLazy = false, bool inferType = true, bool deep = false)
            where TCi : class;

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="includeSelf">if set to <c>true</c> [include self].</param>
        /// <param name="includeBaseTemplates">if set to <c>true</c> [include base templates].</param>
        /// <returns></returns>
        TCi GetParent<TCi>(Guid itemId, bool includeSelf = false, bool includeBaseTemplates = false)
            where TCi : class;

        /// <summary>
        /// Determines whether the specified item identifier has parent.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="includeSelf">if set to <c>true</c> [include self].</param>
        /// <param name="includeBaseTemplates">if set to <c>true</c> [include base templates].</param>
        /// <returns></returns>
        bool HasParent<TCi>(Guid itemId, bool includeSelf = false, bool includeBaseTemplates = false)
            where TCi : class;

        /// <summary>
        /// Gets the sibling.
        /// </summary>
        /// <typeparam name="TCi">The type of the ci.</typeparam>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="previousSibling">if set to <c>true</c> [previous sibling].</param>
        /// <returns></returns>
        TCi GetSibling<TCi>(Guid itemId, bool previousSibling = false)
            where TCi : class;

        /// <summary>
        /// Determines whether the specified item has template.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool HasTemplate(Type type, Item item);
    }
}