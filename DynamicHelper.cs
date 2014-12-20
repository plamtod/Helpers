using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamicHelperExtensions
{
    public static class DynamicHelper
    {
        private static MethodInfo GetMethod<T>() {
         
            Type type = Nullable.GetUnderlyingType(typeof(T));

            //Nullable.GetUnderlyingType(int?) == int -> true
            //Nullable.GetUnderlyingType(int) == null -> true
            if (type == null)
            {
                return typeof(T).GetMethod("Parse", new Type[] { typeof(string) });

            }
            
            return type.GetMethod("Parse", new Type[] { typeof(string) });
        }

        public static T Get<T>(dynamic expandoObject, string name)
        {
            object result, value;
            
            MethodInfo method = GetMethod<T>();
            IDictionary<string, object> dictionary = (IDictionary<string, object>)expandoObject;

            if (!dictionary.TryGetValue(name, out result))
            {
                //if value is not in ExpandoObject just return
                return default(T);
            }
            if (method != null)
            {
                if (result == null) return default(T);
                
                    try
                    {
                        value = method.Invoke(null, new object[] { result.ToString() });
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    return (T)value;
            }

            return (T)result;
        }
    }
}
