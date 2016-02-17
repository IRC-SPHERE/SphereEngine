//
// ISensor.cs
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
    ///     The Sensor interface.
    /// </summary>
    public interface ISensor
    {
        #region Public Properties

        /// <summary>
        ///     Gets the base type name.
        /// </summary>
        string BaseTypeName { get; }

        /// <summary>
        ///     Gets or sets the buckets.
        /// </summary>
        List<SensorBucket> Buckets { get; set; }

        /// <summary>
        ///     Gets the count.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Compute the specified sensor Reading and dateTime.
        /// </summary>
        /// <param name="sensorReading">Sensor reading.</param>
        /// <param name="dateTime">Date time.</param>
        /// <returns>
        /// The active sensor and its value
        /// </returns>
        SensorReading Compute(string sensorReading, DateTime dateTime);

        /// <summary>
        ///     Configure the sensor.
        /// </summary>
        void Configure();

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="i">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetDescription(int i);

        #endregion
    }
}