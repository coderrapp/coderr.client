﻿using System;
using Coderr.Client.Contracts;
using Coderr.Client.Converters;
using Coderr.Client.Reporters;

namespace Coderr.Client.ContextProviders
{
    /// <summary>
    ///     Goes through the exception and maps all custom properties. Will be added into a collection called
    ///     <c>ExceptionProperties</c>.
    /// </summary>
    [DefaultProvider]
    public class ExceptionPropertiesProvider : IContextInfoProvider
    {
        /// <summary>
        ///     Returns "ExceptionProperties"
        /// </summary>
        public string Name => "ExceptionProperties";

        /// <summary>
        ///     Collect information
        /// </summary>
        /// <param name="context">Context information provided by the class which reported the error.</param>
        /// <returns>
        ///     Collection. Items with multiple values are joined using <c>";;"</c>
        /// </returns>
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            try
            {
                var converter = new ObjectToContextCollectionConverter
                {
                    MaxPropertyCount = Err.Configuration.MaxNumberOfPropertiesPerCollection
                };
                converter.FilterProperties((name,value) => name == "WatsonBuckets");
                var collection = converter.Convert(context.Exception);
                collection.Name = "ExceptionProperties";
                return collection;
            }
            catch (Exception ex)
            {
                var context2 = new ContextCollectionDTO("ExceptionProperties");
                context2.Properties.Add("Failed", ex.ToString());
                return context2;
            }
        }
    }
}