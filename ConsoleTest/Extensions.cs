using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleTest
{

    public static class DataReaderExtensions
    {
        #region Private Methods
        private static readonly Dictionary<Type, Delegate> cache = new Dictionary<Type, Delegate>();
        private static readonly object cacheLocker = new object();
        #endregion

        public static T Field<T>(this IDataReader reader, string name)
        {
            return reader[name].ConvertTo<T>(default(T), false);
        }


        public static T Field<T>(this IDataReader reader, int index)
        {
            return reader[index].ConvertTo<T>(default(T), false);
        }

        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            return Fill<T>(reader, DynamicCreateEntity<T>()).ToList();
        }

        public static List<T> ToList<T>(this IDataReader reader, Func<IDataReader, T> predicate) where T : class, new()
        {
            return Fill<T>(reader, predicate).ToList();
        }

        private static Func<IDataReader, T> DynamicCreateEntity<T>() where T : class, new()
        {
            var type = typeof(T);
            if (cache.ContainsKey(type))
                return (Func<IDataReader, T>)cache[type];
            lock (cacheLocker)
            {
                if (cache.ContainsKey(type))
                    return (Func<IDataReader, T>)cache[type];
                var result = DynamicCreateEntityLogic<T>();
                cache.Add(type, result);
                return result;
            }
        }

        private static Func<IDataReader, T> DynamicCreateEntityLogic<T>() where T : class, new()
        {
            // Compiles a delegate of the form (IDataReader r) => new T { Prop1 = r.Field<Prop1Type>("Prop1"), ... }            
            ParameterExpression r = Expression.Parameter(typeof(IDataReader), "r");
            // Get Properties of the property can read and write             
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite).ToArray();
            // Create property bindings for all writable properties             
            List<MemberBinding> bindings = new List<MemberBinding>(props.Length);
            // Get the binding method             
            var method = typeof(DataReaderExtensions).GetMethods().First(p =>
                p.Name == "Field" &&
                p.GetParameters().Length == 2 &&
                p.GetParameters()[1].ParameterType == typeof(string));
            foreach (PropertyInfo property in (typeof(T).GetProperties()))
            {
                // Create expression representing r.Field<property.PropertyType>(property.Name)                 
                MethodCallExpression propertyValue = Expression.Call(method.MakeGenericMethod(property.PropertyType), r, Expression.Constant(property.Name));
                
                // Assign the property value to property through a member binding                 
                MemberBinding binding = Expression.Bind(property, propertyValue);
                bindings.Add(binding);
            }
            // Create the initializer, which instantiates an instance of T and sets property values               
            // using the member bindings we just created             
            Expression initializer = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            // Create the lambda expression, which represents the complete delegate (r => initializer)  
            Expression<Func<IDataReader, T>> lambda = Expression.Lambda<Func<IDataReader, T>>(initializer, r);
            return lambda.Compile();
        }

        private static IEnumerable<T> Fill<T>(IDataReader reader, Func<IDataReader, T> predicate) where T : class, new()
        {
            while (reader.Read())
                yield return predicate(reader);
        }
    }

    public static class ExtensionMethod
    {
        private static string typeIConvertibleFullName = typeof(IConvertible).FullName;
        public static T ConvertTo<T>(this object obj, T defaultValue)
        {
            if (obj != null)
            {
                if (obj is T)
                    return (T)obj;
                var sourceType = obj.GetType();
                var targetType = typeof(T);
                if (targetType.IsEnum)
                    return (T)Enum.Parse(targetType, obj.ToString(), true);
                if (sourceType.GetInterface(typeIConvertibleFullName) != null &&
                targetType.GetInterface(typeIConvertibleFullName) != null)
                    return (T)Convert.ChangeType(obj, targetType);
                var converter = TypeDescriptor.GetConverter(obj);
                if (converter != null && converter.CanConvertTo(targetType))
                    return (T)converter.ConvertTo(obj, targetType);
                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(sourceType))
                    return (T)converter.ConvertFrom(obj);
                throw new Exception("convert error.");
            }
            throw new ArgumentNullException("obj");
        }


        public static T ConvertTo<T>(this object obj)
        {
            return ConvertTo(obj, default(T));
        }

        public static T ConvertTo<T>(this object obj, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                try
                {
                    return obj.ConvertTo<T>(defaultValue);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return obj.ConvertTo<T>(defaultValue);
        }
    }
}
