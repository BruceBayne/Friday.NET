using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Friday.Base.Extensions.Reflection
{
    public static class ReflectionExtensions
    {



        public static IEnumerable<T> GetPropertiesThatImplements<T>(this object propertyHolder) where T : class
        {
            var properties = propertyHolder.GetType().GetProperties();
            return properties.Select(property =>
                property.GetValue(propertyHolder, null)
            ).OfType<T>();
        }


        public static void MapPropertiesWithFieldsTo(this object source, object destination)
        {
            source.CopyProperties(destination);
            source.CopyFields(destination);
        }

        public static void MapPropertiesWithFieldsFrom(this object destination, object source)
        {
            destination.CopyProperties(source);
            destination.CopyFields(source);
        }

        public static T MapPropertiesWithFieldsFrom<T>(this T destination, object source)
        {
            destination.CopyProperties(source);
            destination.CopyFields(source);
            return destination;
        }

        public static T MapPropertiesWithFieldsTo<T>(this object source) where T : new()
        {
            var destination = new T();
            source.CopyProperties(destination);
            source.CopyFields(destination);
            return destination;
        }



        /// <summary>
        /// Extension for 'Object' that copies the fields to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyFields(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their destination counterparts  
            FieldInfo[] srcProps = typeSrc.GetFields();
            foreach (var srcProp in srcProps)
            {
                var targetField = typeDest.GetField(srcProp.Name);
                targetField?.SetValue(destination, srcProp.GetValue(source));
            }
        }


        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their destination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}