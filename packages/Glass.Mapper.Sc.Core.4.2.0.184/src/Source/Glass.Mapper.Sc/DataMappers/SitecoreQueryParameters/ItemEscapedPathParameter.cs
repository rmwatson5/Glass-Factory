/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Class ItemEscapedPathParameter
    /// </summary>
    public class ItemEscapedPathParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "escapedPath"; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            string path = item.Paths.FullPath;
            string[] pathSections = path.Split('/');

            StringBuilder escapedPath = new StringBuilder();

            foreach (var pathSection in pathSections)
            {
                if (!string.IsNullOrEmpty(pathSection)) escapedPath.AppendFormat("/#{0}#", pathSection);
            }

            return escapedPath.ToString();
        }

        #endregion
    }

}




