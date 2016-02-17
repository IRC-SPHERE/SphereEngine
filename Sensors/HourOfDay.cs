//
// HourOfDay.cs
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
    /// The hour of day.
    /// </summary>
    internal class HourOfDay : OneOfNSensor, IContextualSensor
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HourOfDay" /> class.
        /// </summary>
        public HourOfDay()
        {
            this.Description = "HourOfDay";
            this.StringFormat = "[{0}]";
            // this.Id = "HourOfDay";
            this.Buckets =
                Enumerable.Range(0, 24).Select(i => new SensorBucket { Index = i, Name = string.Format("{0}", i), Sensor = this }).ToList();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compute sensor.
        /// </summary>
        /// <param name="sensorReading">
        /// The sensor reading.
        /// </param>
        /// <returns>
        /// The <see cref="SensorBucket"/>.
        /// </returns>
        public override SensorBucket ComputeSensor(string sensorReading)
        {
            DateTime dateTime = DateTime.Parse(sensorReading);
            return this.Buckets[dateTime.Hour];
        }

        #endregion
    }
}