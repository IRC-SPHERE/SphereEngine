//
// SensorSet.cs
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
    using System.Linq;
    using SphereEngine;
    using SensorCollection = System.Collections.Generic.SortedSet<Sensor>;
    using SensorBucketCollection = System.Collections.Generic.List<SensorBucket>;

    /// <summary>
    ///     A set of sensors
    /// </summary>
    public abstract class SensorSet
    {
        /// <summary>
        ///     The Sensor buckets.
        /// </summary>
		private SensorBucketCollection sensorBuckets;

        /// <summary>
        /// The sensor bucket indices.
        /// </summary>
        private Dictionary<SensorBucket, int> sensorBucketIndices;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SensorSet" /> class.
        /// </summary>
        protected SensorSet(Dictionary<string, Type> sensorTypes)
        {
            this.Sensors = new SensorCollection(new SensorComparer());
            this.SensorTypes = sensorTypes;
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Casas.Data.DataReader"/> include compound sensors.
        /// </summary>
        /// <value><c>true</c> if include compound sensors; otherwise, <c>false</c>.</value>
        public bool IncludeCompoundSensors { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sensor types.
        /// </summary>
        /// <value>The sensor types.</value>
        public Dictionary<string, Type> SensorTypes { get; private set; }

        /// <summary>
        ///     Gets the Sensor buckets.
        /// </summary>
		public SensorBucketCollection SensorBuckets
        {
            get
            {
                return this.Sensors == null 
                    ? null 
                    // : this.sensorBuckets ?? (this.sensorBuckets = new SensorBucketCollection(this.Sensors.SelectMany(ia => ia.Buckets), new SensorBucketComparer()));
                    : this.sensorBuckets ?? (this.sensorBuckets = this.Sensors.SelectMany(ia => ia.Buckets).ToList());

                #if FALSE
                if (this.sensorBuckets != null && this.sensorBuckets.Count == this.Sensors.Sum(ia => ia.Buckets.Count))
                {
                    return this.sensorBuckets;
                }

                this.sensorBuckets = new SensorBucketCollection(new SensorBucketComparer());
                int offset = 0;
                foreach (var sensor in this.Sensors)
                {
                    foreach (var bucket in sensor.Buckets)
                    {
                        bucket.Offset = offset;
                        this.sensorBuckets.Add(bucket);
                    }

                    offset += sensor.Count;
                }

                return this.sensorBuckets;
                #endif
            }
        }

        /// <summary>
        /// Gets the sensor bucket indices.
        /// </summary>
        /// <value>The sensor bucket indices.</value>
        public Dictionary<SensorBucket, int> SensorBucketIndices
        {
            get
            {
                return this.Sensors == null 
                    ? null 
                    : this.sensorBucketIndices ?? (this.sensorBucketIndices = this.SensorBuckets.Select((ia, i) => new { ia, i }).ToDictionary(pair => pair.ia, pair => pair.i));
            }
        }

        /// <summary>
        ///     Gets the Sensor vector length.
        /// </summary>
        public int SensorVectorLength
        {
            get
            {
                return this.Sensors == null ? 0 : this.Sensors.Sum(f => f.Count);
            }
        }

        /// <summary>
        ///     Gets or sets the Sensors.
        /// </summary>
        public SensorCollection Sensors { get; set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Add the specified sensor.
        /// </summary>
        /// <param name="sensor">Sensor.</param>
        public void Add(Sensor sensor)
        {
            this.Sensors.Add(sensor);
            this.sensorBuckets = null;
            this.sensorBucketIndices = null;
        }

        /// <summary>
        /// First this instance.
        /// </summary>
        /// <typeparam name="TSensor">The 1st type parameter.</typeparam>
        public Sensor First<TSensor>() where TSensor: Sensor
        {
            return this.Sensors.First(ia => ia is TSensor);
        }

        /// <summary>
        /// Parses the sensor string.
        /// </summary>
        /// <param name="sensorString">Sensor string.</param>
        /// <param name="dateTime">Date time.</param>
        /// <param name="sensorReadings">Sensor readings.</param>
        /// <param name="id">Sensor id.</param>
        /// <typeparam name="TSensor">The type of the sensor.</typeparam>
        public void ParseSensorString<TSensor>(string sensorString, DateTime dateTime, SensorReadingCollection sensorReadings, string id = null)
            where TSensor : Sensor, new()
        {
            var sensor = string.IsNullOrEmpty(id) 
                ? this.Sensors.FirstOrDefault(ia => ia is TSensor) 
                : this.Sensors.FirstOrDefault(ia => ia is TSensor && ia.Id == id);
            if (sensor == null)
            {
                sensor = string.IsNullOrEmpty(id)
                    ? new TSensor()
                    : (TSensor)Activator.CreateInstance(typeof(TSensor), id);
                sensor.Configure();
                this.Sensors.Add(sensor);
            }

            // sensorReadings = new SensorReadingDictionary { sensor.Compute(sensorString) };
            var reading = sensor.Compute(sensorString, dateTime);
            if (sensor is OneOfNSensor)
            {
                sensorReadings.RemoveAll(ia => ia.Bucket.Sensor is TSensor);
            }

            // sensorReadings[reading.Bucket] = reading.Value;
            sensorReadings.Add(reading);
            // sensorReadings.Add(sensor.Compute(sensorString));
        }

        /// <summary>
        /// Parses the sensor string.
        /// </summary>
        /// <param name="dateTimeString">Date time string.</param>
        /// <param name="sensorString">Sensor string.</param>
        /// <param name="sensorReading">Sensor reading.</param>
        /// <param name="sensorReadings">Sensor readings (to be returned).</param>
        /// <param name="error">Error message.</param>
        public void ParseSensorString(string dateTimeString, string sensorString, string sensorReading, SensorReadingCollection sensorReadings, out DataError error)
        {
            // sensorReadings = new SensorReadingDictionary();
            error = null;
            var dateTime = DateTime.Parse(dateTimeString);

            if (sensorReading.Contains(" "))
            {
                sensorReading = sensorReading.Split(' ')[0];
            }

            string sensorType = sensorString.Substring(0, 1).ToUpper();
            string sensorId = sensorString.Substring(1);
            Type type;

            if (SensorTypes.ContainsKey(sensorType))
            {
                type = SensorTypes[sensorType];
            }
            else
            {
                error = new DataError { Id = ErrorID.UnknownSensor, Sensor = sensorString, SensorReading = sensorReading };
                return;
            }

            Sensor sensor = this.Sensors.FirstOrDefault(ia => (ia.GetType() == type) && (ia.Id == sensorId));

            if (sensor == null)
            {
                sensor = (Sensor)Activator.CreateInstance(type);
                sensor.ShortName = sensorType;
                sensor.Id = sensorId;
                sensor.Configure();
                
                // Make sure it gets added
                int oldCount = this.Sensors.Count;
                this.Add(sensor);
                if (oldCount == this.Sensors.Count)
                {
                    throw new InvalidOperationException("Problem adding sensor");
                }
            }

            Sensor compoundSensor = null;
            var hourOfDay = this.Sensors.FirstOrDefault(ia => ia is HourOfDay);
            // var dayOfWeek = this.Sensors.FirstOrDefault(ia => ia is DayOfWeek);

            if (hourOfDay != null && this.IncludeCompoundSensors && sensor is BinarySensor)
            {
                // Also and compound with hour of day
                compoundSensor = this.Sensors.FirstOrDefault(ia => ia is CompoundHourOfDay && ((CompoundHourOfDay)ia).Sensor2 == sensor);
                if (compoundSensor == null)
                {
                    compoundSensor = new CompoundHourOfDay((HourOfDay)hourOfDay, (BinarySensor)sensor);
                    this.Add(compoundSensor);
                }
            }

            try
            {
                // sensorReadings.Add(sensor.Compute(sensorReading));
                var reading = sensor.Compute(sensorReading, dateTime);
                if (reading.Bucket.Sensor is BinarySensor)
                {
                    // Sensor value is zero
                    if (Math.Abs(reading.Value) < double.Epsilon)
                    {
                        if (!sensorReadings.ContainsKey(reading.Bucket))
                        {
                            error = new DataError { Id = ErrorID.IncorrectSensorState, Sensor = sensorString, SensorReading = sensorReading };
                        }
                        else
                        {
                            sensorReadings.Remove(reading.Bucket);
                        }
                    }
                    else
                    {
                        if (sensorReadings.ContainsKey(reading.Bucket))
                        {
                            error = new DataError { Id = ErrorID.IncorrectSensorState, Sensor = sensorString, SensorReading = sensorReading };
                        }
                        else
                        {
                            sensorReadings.Add(reading);
                        }
                    }
                }
                else
                {
                    // sensorReadings[reading.Bucket] = reading.Value;
                    sensorReadings.Add(reading);
                }

                // SensorReadingDictionary readings;

                if (hourOfDay != null)
                {
                    this.ParseSensorString<HourOfDay>(dateTimeString, dateTime, sensorReadings);
                    // sensorReadings.AddRange(readings);
                }

//                if (dayOfWeek != null)
//                {
//                    this.ParseSensorString<DayOfWeek>(dateTimeString, dateTime, ref sensorReadings);
//                    // sensorReadings.AddRange(readings);
//                }

                if (compoundSensor != null)
                {
                    sensorReadings.RemoveAll(
                        ia => ia.Bucket.Sensor is CompoundHourOfDay && 
                        ((CompoundHourOfDay)ia.Bucket.Sensor).Sensor2 == ((CompoundHourOfDay)compoundSensor).Sensor2);
                    sensorReadings.Add(compoundSensor.Compute(dateTimeString + "\t" + sensorReading, dateTime));
                }
            }
            catch (Exception e)
            {
                error = new DataError { Id = ErrorID.SensorReadingError, Sensor = sensorString, SensorReading = sensorReading, Message = e.Message };
                return;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.SensorSet"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.SensorSet"/>.</returns>
        public override string ToString()
        {
            return string.Format("[SensorSet: IncludeCompoundSensors={0}, Name={1}, SensorVectorLength={2}, Sensors={3}]", 
                IncludeCompoundSensors, 
                Name, 
                SensorVectorLength, 
                this.Sensors == null ? "[]" : "[" + string.Join(",", this.Sensors.Select(ia => ia.GetType().Name)) + "]");
        }
        #endregion
    }
}