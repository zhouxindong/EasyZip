using System;

namespace EasyZip.Core.Exceptions
{
    /// <summary>
    /// Indicates that a value was outside of the expected range when decoding an input stream
    /// </summary>
    public class ValueOutOfRangeException : StreamDecodingException
    {
        /// <summary>
        /// Initializes a new instance of the ValueOutOfRangeException class naming the the causing variable
        /// </summary>
        /// <param name="name_of_value">Name of the variable, use: nameof()</param>
        public ValueOutOfRangeException(string name_of_value)
            : base($"{name_of_value} out of range") { }

        /// <summary>
        /// Initializes a new instance of the ValueOutOfRangeException class naming the the causing variable,
        /// it's current value and expected range.
        /// </summary>
        /// <param name="name_of_value">Name of the variable, use: nameof()</param>
        /// <param name="value">The invalid value</param>
        /// <param name="max_value">Expected maximum value</param>
        /// <param name="min_value">Expected minimum value</param>
        public ValueOutOfRangeException(string name_of_value, long value, long max_value, long min_value = 0)
            : this(name_of_value, value.ToString(), max_value.ToString(), min_value.ToString()) { }

        /// <summary>
        /// Initializes a new instance of the ValueOutOfRangeException class naming the the causing variable,
        /// it's current value and expected range.
        /// </summary>
        /// <param name="name_of_value">Name of the variable, use: nameof()</param>
        /// <param name="value">The invalid value</param>
        /// <param name="max_value">Expected maximum value</param>
        /// <param name="min_value">Expected minimum value</param>
        public ValueOutOfRangeException(string name_of_value, string value, string max_value, string min_value = "0") :
            base($"{name_of_value} out of range: {value}, should be {min_value}..{max_value}")
        { }

        private ValueOutOfRangeException()
        {
        }

        private ValueOutOfRangeException(string message, Exception inner_exception) : base(message, inner_exception)
        {
        }
    }

}