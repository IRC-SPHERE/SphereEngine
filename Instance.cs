//
// Instance.cs
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

namespace SphereEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using SphereEngine.Sensors;

    /// <summary>
    /// The instance.
    /// </summary>
    public class Instance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Instance"/> class.
        /// </summary>
        public Instance()
        {
            this.Activities = new List<Activity>();
            this.SensorReadings = new SensorReadingCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SphereEngine.Instance"/> class.
        /// </summary>
        /// <param name="instance">Instance.</param>
        public Instance(Instance instance)
        {
            if (instance == null)
            {
                return;
            }

            this.Activities = instance.Activities == null ? null : new List<Activity>(instance.Activities);
            this.SensorReadings = instance.SensorReadings == null ? null : new SensorReadingCollection(instance.SensorReadings);
            this.DateTime = instance.DateTime;
            this.SensorsOn = instance.SensorsOn == null ? null : new HashSet<SensorBucket>(instance.SensorsOn);
        }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        [Browsable(false)]
        public List<Activity> Activities { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the sensor readings.
        /// </summary>
        public SensorReadingCollection SensorReadings { get; set; }

        /// <summary>
        /// Gets or sets the sensors on.
        /// </summary>
        /// <value>The sensors on.</value>
        public HashSet<SensorBucket> SensorsOn { get; set; }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get
            {
                return string.Format(
                    "[Instance: DateTime={0}, Activities={1}, SensorReadings={2}]",
                    this.DateTime,
                    string.Join(", ", this.Activities),
                    this.SensorReadings);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SphereEngine.Instance"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SphereEngine.Instance"/>.</returns>
        public override string ToString()
        {
            return this.Summary;
        }
    }
}