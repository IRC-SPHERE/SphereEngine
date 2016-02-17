//
// CurrentActivity.cs
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Current activity.
    /// </summary>
    public class CurrentActivity : OneOfNSensor, IContextualSensor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.CurrentActivity"/> class.
        /// </summary>
        public CurrentActivity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.CurrentActivity" /> class.
        /// </summary>
        /// <param name="resident">The resident.</param>
        /// <param name="activities">The activities.</param>
        public CurrentActivity(Resident resident, IEnumerable<Activity> activities)
        {            
            this.Description = "CurrentActivity";
            this.Id = resident.Name;
            this.StringFormat = "[{0}]";
            this.Buckets =
                activities.Select((activity, i) => new SensorBucket { Index = i, Name = activity.Name, Sensor = this }).ToList();
        }

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
            return this.Buckets.FirstOrDefault(ia => ia.Name == sensorReading) ?? this.Buckets[0];
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Description + this.Id;
        }
    }
}
