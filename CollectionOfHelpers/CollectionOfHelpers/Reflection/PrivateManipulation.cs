using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CollectionOfHelpers.Reflection
{
    /// <summary>
    /// This class gives several helper methods for reading (and even writing to) private fields.
    /// This is esspessially useful for debug purposes. However, it should never be used in production code on any classes you don't have direct
    /// control of: any third party libraries, or even native .net classes, could have their internals change without you knowing.
    /// It's use should also be limited even on classes you do control, as you create potentially tight coupling.
    /// An example of suitable production use is when deserialising a file into a class instance, you can check if a certain field
    /// exists (and if so what it's value is) - allowing you to modify how you handle the dynamically created object.
    /// </summary>
    public static class PrivateManipulation
    {
        /// <summary>
        /// Takes an instance of any class, and tries to return a reference to one of it's private reference-type instance fields (no static fields).
        /// </summary>
        /// <typeparam name="TInstance">The class type of the instance being reflected upon</typeparam>
        /// <typeparam name="TOutput">The expected type associated with the private field in questionThe </typeparam>
        /// <param name="classInstance">An instance of a class to read from</param>
        /// <param name="privateFieldName">The name of the private field you want a reference to</param>
        /// <param name="result">A reference to the field (if it was found). If is wan't found, then a default value is assigned (typically null)</param>
        /// <returns>True if the field was read, false if is failed.</returns>
        public static bool TryGetPrivateFieldReference<TInstance, TOutput>(TInstance classInstance, string privateFieldName, out TOutput result) where TOutput : class
        {
            FieldInfo fi = typeof(TInstance).GetField(privateFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null)
            {
                result = default(TOutput);
                return false;
            }

            //Handle when the field exists, but has a null value anyway
            object fieldValue = fi.GetValue(classInstance);
            if (fieldValue == null)
            {
                result = null;
                return true;
            }

            //Handle when the field exists, but it can't be cast to the expected type - note that upcasting could occur and would succeed (eg int[] to IList)
            var castValue = fieldValue as TOutput;
            if (castValue == null)
            {
                result = default(TOutput);
                return false;
            }

            result = castValue;
            return true;
        }
    }
}
