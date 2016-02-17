//
// Bias.cs
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
    /// Bias.
    /// </summary>
    public class Bias : Sensor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bias"/> class.
        /// </summary>
        public Bias()
            : this(-Math.Sqrt(10))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bias"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public Bias(double value)
        {
            this.Value = value;
            this.IsShared = true;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Configure the feature.
        /// </summary>
        public override void Configure()
        {
            this.Buckets = new List<SensorBucket> { new SensorBucket { Index = 0, Name = "Bias", Sensor = this } };
        }

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
			return new SensorReading { Bucket = this.Buckets[0], Value = this.Value, DateTime = dateTime };
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SphereEngine.Sensors.Bias"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SphereEngine.Sensors.Bias"/>.</returns>
        public override string ToString()
        {
            return string.Format("Bias: Value={0}", this.Value);
        }
    }
}

