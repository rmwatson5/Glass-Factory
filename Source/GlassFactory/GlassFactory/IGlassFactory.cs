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
    }
}