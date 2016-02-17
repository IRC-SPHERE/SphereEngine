//
// BinaryAnd.cs
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
    /// Binary and.
    /// </summary>
    public class BinaryAnd : BinarySensor, ICompoundFeature<BinarySensor, BinarySensor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.BinaryAnd"/> class.
        /// </summary>
        public BinaryAnd()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryAnd"/> class.
        /// </summary>
        /// <param name="sensor1">The first sensor.</param>
        /// <param name="sensor2">The second sensor.</param>
        public BinaryAnd(BinarySensor sensor1, BinarySensor sensor2)
        {
            this.Sensor1 = sensor1;
            this.Sensor2 = sensor2;
            this.Description = sensor1.Description  + " AND " + sensor2.Description;
            this.Configure();
        }

        /// <summary>
        /// Gets or sets the base sensor 1.
        /// </summary>
        public BinarySensor Sensor1 { get; set; }

        /// <summary>
        /// Gets or sets the base sensor 2.
        /// </summary>
        public BinarySensor Sensor2 { get; set; }

        /// <summary>
        /// Gets the bucket names.
        /// </summary>
        /// <value>The bucket names.</value>
        public override string[] BucketNames
        {
            get
            {
                return this.Buckets.Select(ia => ia.Name).ToArray();
            }
        }

        /// <summary>
        /// Configure the feature.
        /// </summary>
        public override void Configure()
        {
            this.Buckets = new List<SensorBucket>
                {
                    new SensorBucket
                        {
                            Name =
                                string.Format(
                                    "{0} AND {1}",
                                    this.Sensor1.Description,
                                    this.Sensor2.Description),
                            Index = 0,
                            Sensor = this
                        }
                };
        }

        /// <summary>
        /// Computes the sensor.
        /// </summary>
        /// <param name="message">The sensor reading.</param>
        /// <returns>
        /// The <see cref="System.Boolean" />
        /// </returns>
        public override bool ComputeSensor(string sensorReading)
        {
            return this.Sensor1.ComputeSensor(sensorReading) && this.Sensor2.ComputeSensor(sensorReading);
        }
    }
}
