#region license

// Based on NCommon project: http://code.google.com/p/ncommon

// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 

// http://www.apache.org/licenses/LICENSE-2.0 

#endregion

using System;
using System.Diagnostics;
using System.Linq;

namespace LaazyDate.Utils
{
    /// <summary>
    /// Provides utility methods to guard parameter and local variables.
    /// </summary>
    internal static class Guard
    {
        /// <summary>
        /// Throws an ArgumentNullException if objectToTest is null.
        /// </summary>
        /// <typeparam name="T">Type of objectToTest</typeparam>
        /// <param name="objectToTest">Object to test for null</param>
        /// <param name="parameterName">(Optional) name of parameter being tested</param>
        [DebuggerNonUserCode]
        public static void AgainstNullArgument<T>(T objectToTest, string parameterName = null)
            where T : class
        {
            if (objectToTest != null) return;

            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            var typeName = method.DeclaringType?.Name ?? "(unknown type)";
            var callingMethod = $"{typeName}.{method.Name}";
            throw new ArgumentNullException(parameterName,
                $"{callingMethod}: Argument of type {typeof(T)} cannot be null.");
        }

        [DebuggerNonUserCode]
        public static void AgainstNullArguments(params object[] arguments)
        {
            if (arguments == null)
                throw new InvalidOperationException("No arguments provided for testing.");

            if (arguments.All(x => x != null)) return;

            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            var typeName = method.DeclaringType?.Name ?? "(unknown type)";
            var callingMethod = $"{typeName}.{method.Name}";
            throw new ArgumentNullException(null, $"{callingMethod}: One or more required arguments were null.");
        }

        /// <summary>
        /// Throws an ArgumentNullException if stringToTest is null, empty or whitespace.
        /// </summary>
        /// <param name="stringToTest">String to test</param>
        /// <param name="parameterName">(Optional) parameter name being tested</param>
        [DebuggerNonUserCode]
        public static void AgainstNullOrWhiteSpaceArgument(string stringToTest, string parameterName = null)
        {
            if (!String.IsNullOrWhiteSpace(stringToTest)) return;

            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            var typeName = method.DeclaringType?.Name ?? "(unknown type)";
            var callingMethod = $"{typeName}.{method.Name}";
            throw new ArgumentNullException(parameterName,
                $"{callingMethod}: String argument cannot be null, empty or whitespace.");
        }

        /// <summary>
        /// Tests the DateTime value falls within the acceptable range for MSSQL DateTime columns.
        /// Throws an ArgumentOutOfRangeException if test fails.
        /// </summary>
        /// <param name="valueToTest">DateTime value to test</param>
        /// <param name="parameterName">(Optional) parameter name being tested</param>
        [DebuggerNonUserCode]
        public static void AgainstDateTimeOutsideSqlRange(DateTime valueToTest, 
            string parameterName = null)
        {
            if (valueToTest < System.Data.SqlTypes.SqlDateTime.MinValue.Value ||
                valueToTest > System.Data.SqlTypes.SqlDateTime.MaxValue.Value)
                throw new ArgumentOutOfRangeException(parameterName,
                    $"DateTime value '{valueToTest}' is outside the range permitted for MSSQL DateTimes.");
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> with the specified message
        /// when the assertion statement is true.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw.</typeparam>
        /// <param name="assertion">The assertion to evaluate. If true then the <typeparamref name="TException"/> exception is thrown.</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void Against<TException>(bool assertion, string message, params object[] messageParameters) where TException : Exception
        {
            if (assertion)
                ThrowException<TException>(message, messageParameters);
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> with the specified message
        /// when the assertion statement is true.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw.</typeparam>
        /// <param name="assertion">The assertion to evaluate. If true then the <typeparamref name="TException"/> exception is thrown.</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void Against<TException>(Func<bool> assertion, string message, params object[] messageParameters) where TException : Exception
        {
            //Execute the lambda and if it evaluates to true then throw the exception.
            if (assertion())
                ThrowException<TException>(message, messageParameters);
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> when the specified object
        /// instance does not inherit from <typeparamref name="TBase"/> type.
        /// </summary>
        /// <typeparam name="TBase">The base type to check for.</typeparam>
        /// <param name="instance">The object to check if it inherits from <typeparamref name="TBase"/> type.</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void InheritsFrom<TBase>(object instance, string message, params object[] messageParameters) where TBase : Type
        {
            InheritsFrom<TBase>(instance.GetType(), message, messageParameters);
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> when the specified type does not
        /// inherit from the <typeparamref name="TBase"/> type.
        /// </summary>
        /// <typeparam name="TBase">The base type to check for.</typeparam>
        /// <param name="type">The <see cref="Type"/> to check if it inherits from <typeparamref name="TBase"/> type.</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void InheritsFrom<TBase>(Type type, string message, params object[] messageParameters)
        {
            if (type.BaseType != typeof(TBase))
                throw new InvalidOperationException(messageParameters == null ? message : string.Format(message, messageParameters));
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> when the specified object
        /// instance does not implement the <typeparamref name="TInterface"/> interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface type the object instance should implement.</typeparam>
        /// <param name="instance">The object instance to check if it implements the <typeparamref name="TInterface"/> interface</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void Implements<TInterface>(object instance, string message, params object[] messageParameters)
        {
            Implements<TInterface>(instance.GetType(), message, messageParameters);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when the specified type does not
        /// implement the <typeparamref name="TInterface"/> interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface type that the <paramref name="type"/> should implement.</typeparam>
        /// <param name="type">The <see cref="Type"/> to check if it implements from <typeparamref name="TInterface"/> interface.</param>
        /// <param name="message">string. The exception message to throw.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void Implements<TInterface>(Type type, string message, params object[] messageParameters)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(messageParameters == null ? message : string.Format(message, messageParameters));
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when the specified object instance is
        /// not of the specified type.
        /// </summary>
        /// <typeparam name="TType">The Type that the <paramref name="instance"/> is expected to be.</typeparam>
        /// <param name="instance">The object instance whose type is checked.</param>
        /// <param name="message">The message of the <see cref="InvalidOperationException"/> exception.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void TypeOf<TType>(object instance, string message, params object[] messageParameters)
        {
            if (!(instance is TType))
                throw new InvalidOperationException(messageParameters == null ? message : string.Format(message, messageParameters));
        }

        /// <summary>
        /// Throws an exception if an instance of an object is not equal to another object instance.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw when the guard check evaluates false.</typeparam>
        /// <param name="compare">The comparison object.</param>
        /// <param name="instance">The object instance to compare with.</param>
        /// <param name="message">string. The message of the exception.</param>
        /// <param name="messageParameters">Optional parameters for the message format string.</param>
        [DebuggerNonUserCode]
        public static void IsEqual<TException>(object compare, object instance, string message, params object[] messageParameters) where TException : Exception
        {
            if (compare != instance)
                ThrowException<TException>(message, messageParameters);
        }

        static void ThrowException<TException>(string message, object[] messageParameters) where TException : Exception
        {
            var formattedMessage = messageParameters == null ? message : string.Format(message, messageParameters);

            if (typeof(TException) == typeof(ArgumentNullException))
                throw (ArgumentNullException)
                      Activator.CreateInstance(typeof(ArgumentNullException), /*paramName*/ null, formattedMessage);

            throw (TException)Activator.CreateInstance(typeof(TException), formattedMessage);
        }
    }
}
