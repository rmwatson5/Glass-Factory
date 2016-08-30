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
using Castle.DynamicProxy;

namespace Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction
{


    /// <summary>
    /// Class LazyObjectInterceptor
    /// </summary>
    public class LazyObjectInterceptor : IInterceptor
    {
        public Action<object> MappingAction { get; set; }
        private bool _isMapped = false;

        public object Actual { get; set; }

     

        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            //create class
            if (Actual != null && _isMapped == false)
            {
                lock (Actual)
                {
                    if (_isMapped == false)
                    {
                        _isMapped = true;
                        MappingAction(Actual);
                        
                    }
                }
            }
            invocation.Proceed();
        }

        #endregion


    }
}






