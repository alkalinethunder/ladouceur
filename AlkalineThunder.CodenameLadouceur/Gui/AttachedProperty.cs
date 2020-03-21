using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    /// <summary>
    /// Represents the structure of an attached property.
    /// </summary>
    public interface IAttachedProperty
    {
        /// <summary>
        /// Gets the name of the attached property.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the value stored in the attached property.
        /// </summary>
        public object Value { get; }
    }

    /// <summary>
    /// Represents a generic <see cref="IAttachedProperty"/> used to store values of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of data held inside this attached property.</typeparam>
    public class AttachedProperty<T> : IAttachedProperty
    {
        private T _value;

        /// <summary>
        /// Gets the name of the attached property.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the value stored inside the attached property.
        /// </summary>
        object IAttachedProperty.Value => _value;

        /// <summary>
        /// Gets the value stored inside the attached property.
        /// </summary>
        public T Value => _value;

        /// <summary>
        /// Creates a new instance of the <see cref="AttachedProperty{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the attached property.</param>
        /// <param name="value">The value of the attached property.</param>
        public AttachedProperty(string name, T value)
        {
            Name = name;
            _value = value;
        }

        /// <summary>
        /// Sets the value of the attached property.
        /// </summary>
        /// <param name="value">The new value to store in the property.</param>
        public void SetValue(T value)
        {
            _value = value;
        }
    }
}
