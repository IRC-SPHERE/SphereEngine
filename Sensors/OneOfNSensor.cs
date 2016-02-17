//
// OneOfNSensor.cs
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
    using System.Linq;

    /// <summary>
    /// The one of n sensor.
    /// </summary>
    public abstract class OneOfNSensor : Sensor
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the bucket names.
        /// </summary>
        public string[][] BucketNames { get; set; }

        /// <summary>
        ///     Gets or sets the sensor bucket function.
        /// </summary>
        public Func<string[], int, SensorBucket> SensorBucketFunc { get; set; }

        #endregion

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
            return new SensorReading { Bucket = this.ComputeSensor(sensorReading), Value = 1.0, DateTime = dateTime };
        }

        /// <summary>
        /// Computes the sensor.
        /// </summary>
        /// <param name="sensorReading">
        /// The sensor reading.
        /// </param>
        /// <returns>
        /// The <see cref="SensorBucket"/>
        /// </returns>
        public abstract SensorBucket ComputeSensor(string sensorReading);

        /// <summary>
        ///     Configure the sensor.
        /// </summary>
        public override void Configure()
        {
            if (this.BucketNames != null && this.SensorBucketFunc != null)
            {
                this.Buckets = this.BucketNames.Select(this.SensorBucketFunc).ToList();
            }
        }

        #endregion
    }
}