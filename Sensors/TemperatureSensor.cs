//
// TemperatureSensor.cs
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

    /// <summary>
    ///     The Temperature Sensor
    /// </summary>
    [Serializable]
    public class TemperatureSensor : NumericSensor
    {
        #region Constructors and Destructors

//        /// <summary>
//        ///     Initializes a new instance of the <see cref="TemperatureSensor" /> class.
//        /// </summary>
//        public TemperatureSensor()
//            : this(SensorSet.TemperatureSensorBins)
//        {
//        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureSensor"/> class.
        /// </summary>
        /// <param name="bins">
        /// The bins.
        /// </param>
        public TemperatureSensor(double[] bins)
            : base(bins)
        {
            this.Description = "Temperature";
            this.StringFormat = "{0}";
        }

        #endregion
    }
}