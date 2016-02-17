//
// SensorBucket.cs
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
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    ///     The sensor bucket.
    /// </summary>
    public class SensorBucket : IEquatable<SensorBucket>
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        public int Index { get; set; }

        #if FALSE
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; }
        #endif

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the parent sensor for this bucket.
        /// </summary>
        [BsonIgnore]
        public ISensor Sensor { get; set; }

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
        public bool Equals(SensorBucket other)
        {
            return this.Name == other.Name 
                && this.Sensor.Name == other.Sensor.Name 
                && this.Sensor.Id == other.Sensor.Id
                && (!(this.Sensor is CompoundHourOfDay) || !(other.Sensor is CompoundHourOfDay) 
                    || ((CompoundHourOfDay)this.Sensor).Sensor1.Id == ((CompoundHourOfDay)other.Sensor).Sensor1.Id 
                    && ((CompoundHourOfDay)this.Sensor).Sensor2.Id == ((CompoundHourOfDay)other.Sensor).Sensor2.Id)
                && this.Index == other.Index;
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
            var bucket = obj as SensorBucket;
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
            // Use anonymous type to generate hash code
            var sensorName = this.Sensor.Name;
            var sensor = this.Sensor as CompoundHourOfDay;
            if (sensor != null)
            {
                var sensor1Id = sensor.Sensor1.Id;
                var sensor2Id = sensor.Sensor2.Id;
                return new {
                    this.Name,
                    this.Index,
                    this.Sensor.Id,
                    sensor1Id,
                    sensor2Id,
                    sensorName
                }.GetHashCode();
            }

            return new {
                this.Name,
                this.Index,
                this.Sensor.Id,
                sensorName
            }.GetHashCode();
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override sealed string ToString()
        {
            return this.Sensor is BinarySensor || this.Sensor is Bias 
                    ? this.Sensor.Name + this.Sensor.Id 
                    : string.Format("{0}{1}[{2}]", this.Sensor.Name, this.Sensor.Id, this.Name);
        }

        #endregion
    }
}