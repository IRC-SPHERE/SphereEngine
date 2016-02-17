//
// DayOfWeek.cs
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
    /// The day of the week.
    /// </summary>
    internal class DayOfWeek : OneOfNSensor, IContextualSensor
    {
        /// <summary>
        /// The names. Note that System.DayOfWeek enum starts with Sunday, so easiest to stick with that.
        /// </summary>
        private static readonly string[] names =
            {
                "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"
            };

        #region Constructors and Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DayOfWeek" /> class.
        /// </summary>
        public DayOfWeek()
        {
            this.Description = "DayOfWeek";
            this.StringFormat = "[{0}]";
            this.Buckets =
                Enumerable.Range(0, 7).Select(i => new SensorBucket { Index = i, Name = names[i], Sensor = this }).ToList();
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
            return this.Buckets[(int)dateTime.DayOfWeek];
        }

        #endregion
    }
}