//
// FullSensorSet.cs
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
using MicrosoftResearch.Infer.Collections;

namespace SphereEngine.Sensors
{
	using System;
	using System.Collections.Generic;

    /// <summary>
    /// Full sensor set.
    /// </summary>
    public class FullSensorSet : SensorSet, IHasElectricitySensor, IHasTemperatureSensor
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.FullSensorSet"/> class.
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        /// <param name="includePreviousActivity">If set to <c>true</c> include previous activity.</param>
        /// <param name="includeCurrentActivity">If set to <c>true</c> include current activity.</param>
        public FullSensorSet(CasasInputs inputs, bool includePreviousActivity, bool includeCurrentActivity) : base(new Dictionary<string, Type>
            {
                { "M", typeof(MotionSensor) }, 
                { "L", typeof(LightSensor) }, 
                { "I", typeof(KitchenItemSensor) }, 
                { "D", typeof(DoorSensor) }, 
                { "T", typeof(TemperatureSensor) }, 
                { "P", typeof(ElectricitySensor) }, 
                { "E", typeof(ExperimenterSwitch) }
            })
        {
            this.Name = this.GetType().Name;
            this.Add(new HourOfDay());
            // this.Sensors.Add(new DayOfWeek());
            if (includePreviousActivity)
            {
                inputs.Residents.ForEach(resident => this.Add(new PreviousActivity(resident, inputs.GetActivities(true))));
            }

            if (includeCurrentActivity)
            {
                inputs.Residents.ForEach(resident => this.Add(new CurrentActivity(resident, inputs.GetActivities(true))));
            }
        }

        /// <summary>
        /// Gets the electricity sensor bins.
        /// </summary>
        /// <value>The electricity sensor bins.</value>
        public double[] ElectricitySensorBins
        {
            get
            {
                return new double[] { 0, 1590, 3179, 4768, 6357, 7946, 9535, 11124, 12713, 14302, 15891, double.PositiveInfinity };
            }
        }

        /// <summary>
        /// Gets the temperature sensor bins.
        /// </summary>
        /// <value>The temperature sensor bins.</value>
        public double[] TemperatureSensorBins
        {
            get
            {
                return new double[] { 0, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, double.PositiveInfinity };
            }
        }
	}

    /// <summary>
    /// Small sensor set (binary sensors only).
    /// </summary>
    public class SmallSensorSet : SensorSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.SmallSensorSet"/> class.
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        /// <param name="includePreviousActivity">If set to <c>true</c> include previous activity.</param>
        /// <param name="includeCurrentActivity">If set to <c>true</c> include current activity.</param>
        public SmallSensorSet(CasasInputs inputs, bool includePreviousActivity, bool includeCurrentActivity) : base(new Dictionary<string, Type>
            {
                { "M", typeof(MotionSensor) }, 
                { "L", typeof(LightSensor) }, 
                { "I", typeof(KitchenItemSensor) }, 
                { "D", typeof(DoorSensor) }, 
                { "E", typeof(ExperimenterSwitch) }
            })
        {
            this.Name = this.GetType().Name;
            this.Add(new HourOfDay());
            // this.Sensors.Add(new DayOfWeek());
            if (includePreviousActivity)
            {
                inputs.Residents.ForEach(resident => this.Add(new PreviousActivity(resident, inputs.GetActivities(true))));
            }

            if (includeCurrentActivity)
            {
                inputs.Residents.ForEach(resident => this.Add(new CurrentActivity(resident, inputs.GetActivities(true))));
            }

        }
    }
}