//
// Sensor.cs
//
// Author:
//       Tom Diethe <tom.diethe@bristol.ac.uk>
//
// Copyright (c) 2015 University of Bristol
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SphereEngine.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Base class for creating sensors
    /// </summary>
    public abstract class Sensor : ISensor, IEquatable<Sensor>
    {
        #region Fields

        /// <summary>
        ///     The base type name.
        /// </summary>
        private string baseTypeName;

        /// <summary>
        ///     Whether this Sensor is shared between users.
        /// </summary>
        private bool isShared = true;

        /// <summary>
        ///     The name.
        /// </summary>
        private string name;

        /// <summary>
        ///     The string format to produce a short description.
        /// </summary>
        private string stringFormat = "{0}";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sensor" /> class.
        /// </summary>
        protected Sensor()
        {
            this.Buckets = new List<SensorBucket>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the base type name.
        /// </summary>
        public string BaseTypeName
        {
            get
            {
                if (this.baseTypeName != null)
                {
                    return this.baseTypeName;
                }

                Type baseType = this.GetType().BaseType;
                this.baseTypeName = baseType != null ? baseType.Name.Split('`')[0] : string.Empty;

                return this.baseTypeName;
            }
        }

        /// <summary>
        ///     Gets or sets the buckets.
        ///     These are created by the constructors of the derived classes,
        ///     so don't need to be serialized.
        /// </summary>
        public List<SensorBucket> Buckets { get; set; }

        /// <summary>
        ///     Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Buckets == null ? 0 : this.Buckets.Count;
            }
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the first description.
        /// </summary>
        public string FirstDescription { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is shared.
        /// </summary>
        public bool IsShared
        {
            get
            {
                return this.isShared;
            }

            set
            {
                this.isShared = value;
            }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name ?? (this.name = this.GetType().Name);
            }
        }

        /// <summary>
        ///     Gets the Sensor names.
        /// </summary>
        public string[] SensorNames
        {
            get
            {
                return this.Buckets == null ? null : this.Buckets.Select(ia => ia.Name).ToArray();
            }
        }

        /// <summary>
        ///     Gets or sets the string format to produce a short description.
        /// </summary>
        public string StringFormat
        {
            get
            {
                return this.stringFormat;
            }

            set
            {
                this.stringFormat = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Sensor other)
        {
            return this.Name == other.Name && this.Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var bucket = obj as Sensor;
            return bucket != null && this.Equals(bucket);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            // Use anonymous tye to generate hash code
            return new { this.Name, this.Id }.GetHashCode();
        }

        /// <summary>
        /// Compute the specified sensorReading and dateTime.
        /// </summary>
        /// <param name="sensorReading">Sensor reading.</param>
        /// <param name="dateTime">Date time.</param>
        /// <returns>
        /// The active sensor and its value
        /// </returns>
        public abstract SensorReading Compute(string sensorReading, DateTime dateTime);

        /// <summary>
        ///     Configure the Sensor.
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetDescription(int i)
        {
            if ((i == 0) && (this.FirstDescription != null))
            {
                return this.FirstDescription;
            }

            if ((this.Count > 1) && (this.SensorNames.Length > 1))
            {
                return string.Format(this.StringFormat, this.SensorNames[i]);
            }

            return this.StringFormat;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Description;
        }

        #endregion
    }
}