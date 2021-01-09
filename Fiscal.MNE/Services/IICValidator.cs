using Fiscal.MNE.Models;
using System;
using System.Reflection;

namespace Fiscal.MNE.Services
{
    internal class IICValidator
    {
        public static void ThrowExceptionIfNotValid(IICConfig iicConfig)
        {
            if (iicConfig == null) throw new ArgumentNullException("IICConfig can't be null!");

            PropertyInfo[] iicConfigProperties = typeof(IICConfig).GetProperties();
            foreach (PropertyInfo property in iicConfigProperties)
            {
                if (string.IsNullOrEmpty(property.GetValue(iicConfig)?.ToString()))
                {
                    throw new ArgumentNullException($"IICConfig: {property.Name} property can't be null or empty!");
                }
            }
        }
    }
}
