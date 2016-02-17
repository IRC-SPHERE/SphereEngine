//
// NumericSensor.cs
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
    using System.Globalization;
    using System.Linq;

    /// <summary>
    ///     A 1-of-N (bucketed) Sensor
    /// </summary>
    public abstract class NumericSensor : Sensor
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NumericSensor" /> class.
        /// </summary>
        protected NumericSensor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericSensor"/> class.
        /// </summary>
        /// <param name="bins">
        /// The bins.
        /// </param>
        protected NumericSensor(double[] bins)
        {
            this.Bins = bins;
            this.AllReadings = new List<double>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the all readings.
        /// </summary>
        public List<double> AllReadings { get; set; }

        /// <summary>
        ///     Gets or sets the bins.
        /// </summary>
        public double[] Bins { get; set; }

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
            return new SensorReading { Bucket = this.Buckets[this.ComputeSensor(sensorReading)], Value = 1.0, DateTime = dateTime };
        }

        /// <summary>
        /// Computes the Sensor.
        /// </summary>
        /// <param name="sensorReading">
        /// The sensor reading.
        /// </param>
        /// <returns>
        /// The index of the Sensor that is on.
        /// </returns>
        public int ComputeSensor(string sensorReading)
        {
            double sensorValue = double.Parse(sensorReading);
            this.AllReadings.Add(sensorValue);
            int val = this.Buckets.Count - 1;
            for (int i = 0; i < this.Buckets.Count; i++)
            {
                if (sensorValue > this.Bins[i])
                {
                    continue;
                }

                val = i;
                break;
            }

            return val;
        }

        /// <summary>
        ///     Configure the Sensor.
        /// </summary>
        public override void Configure()
        {
			this.Buckets =
                this.Bins.Select((ia, i) => new SensorBucket { Index = i, Name = GetLengthStrings(this.Bins, i), Sensor = this })
					.ToList();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.NumericSensor"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.NumericSensor"/>.</returns>
        public override string ToString()
        {
            return this.Name + this.Id;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the length strings.
        /// </summary>
        /// <param name="lengths">
        /// The lengths.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The string.
        /// </returns>
        internal static string GetLengthStrings(double[] lengths, int i)
        {
            if (i == 0)
            {
                return lengths[0].ToString(CultureInfo.InvariantCulture);
            }

            if (Math.Abs(lengths[i] - lengths[i - 1] - 1) < double.Epsilon)
            {
                return lengths[i].ToString(CultureInfo.InvariantCulture);
            }

            return lengths[i] < int.MaxValue
                       ? string.Format("{0}-{1}", lengths[i - 1] + 1, lengths[i])
                       : string.Format(">{0}", lengths[i - 1]);
        }

        #endregion
    }
}