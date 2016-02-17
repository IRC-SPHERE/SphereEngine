//
// BinarySensor.cs
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

    /// <summary>
    ///     A binary sensor
    /// </summary>
    public abstract class BinarySensor : Sensor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.BinarySensor"/> class.
        /// </summary>
        protected BinarySensor()
        {
            this.Validity = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Gets the bucket names.
        /// </summary>
        public abstract string[] BucketNames { get; }

        /// <summary>
        /// Gets or sets the validity (how long this sensor can be on for).
        /// </summary>
        /// <value>The validity.</value>
        public TimeSpan Validity { get; set; }

        #region Public Methods and Operators

        /// <summary>
        /// Compute the specified sensorReading and dateTime.
        /// </summary>
        /// <param name="sensorReading">Sensor reading.</param>
        /// <param name="dateTime">Date time.</param>
        /// <returns>
        /// The active sensor and its value
        /// </returns>
        public override sealed SensorReading Compute(string sensorReading, DateTime dateTime)
        {
            // return new SensorReading { Bucket = this.Buckets[0], Value = this.ComputeSensor(sensorReading) ? 1.0 : 0.0, DateTime = dateTime };
            return new SensorReading { Bucket = this.ComputeSensor(sensorReading) ? this.Buckets[0] : this.Buckets[1], Value = 1.0, DateTime = dateTime };
        }

        /// <summary>
        /// Computes the Sensor.
        /// </summary>
        /// <param name="sensorReading">
        /// The sensor reading.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/>
        /// </returns>
        public abstract bool ComputeSensor(string sensorReading);

        /// <summary>
        ///     Configure the Sensor.
        /// </summary>
        public override void Configure()
        {
            this.Description = this.Name + this.Id;
            // this.Buckets = new List<SensorBucket> { new SensorBucket { Index = 0, Name = this.GetType().Name, Sensor = this } };
            this.Buckets = new List<SensorBucket> { 
                new SensorBucket { Index = 0, Name = this.GetType().Name + "-ON", Sensor = this },
                new SensorBucket { Index = 1, Name = this.GetType().Name + "-OFF", Sensor = this }};
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Description;
        }

        #endregion
    }
}