//
// CasasInputs.cs
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
    /// The inputs.
    /// </summary>
    public class CasasInputs
    {
        /// <summary>
        /// The activities.
        /// </summary>
        private readonly string[] activities =
            { 
                "WanderingInRoom", 
                "Sleep",
                "BedToToilet",
                "PersonalHygiene",
                "Bathing",
                "Work",
                "MealPreparation",
           		"LeaveHome",
                "Eating",
                "EnterHome",
                "WatchTv", 
                "Housekeeping"
            };

        /// <summary>
		/// Initializes a new instance of the <see cref="SphereEngine.Inputs"/> class.
        /// </summary>
        public CasasInputs()
        {
            this.TrainInstances = new List<Instance>();
            this.TestInstances = new List<Instance>();
        }

        /// <summary>
        /// Gets or sets the number of residents.
        /// </summary>
        /// <value>The number of residents.</value>
        public int NumberOfResidents { get; set; }

        /// <summary>
        /// Gets the residents.
        /// </summary>
        /// <value>The residents.</value>
        public IList<Resident> Residents
        {
            get { return this.GetResidents(); }
        }

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <value>The activities.</value>
        public IList<Activity> Activities
        {
            get { return this.GetActivities(); }
        }

        #region Public Properties
        /// <summary>
        /// Gets the sensor count.
        /// </summary>
        public int SensorCount
        {
            get
            {
                return this.SensorSet == null ? -1 : this.SensorSet.SensorVectorLength;
            }
        }

        /// <summary>
        /// Gets or sets the sensor set.
        /// </summary>
        public SensorSet SensorSet { get; set; }

        /// <summary>
        /// Gets or sets the test instances.
        /// </summary>
        public List<Instance> TestInstances { get; set; }

        /// <summary>
        /// Gets or sets the train instances.
        /// </summary>
        public List<Instance> TrainInstances { get; set; }

        #if FALSE
        /// <summary>
  SphereEngine /// Gets the sensor bucket counts.
        /// </summary>
        public Dictionary<Sensor, Dictionary<string, double>> SensorSphereEnginetCounts
        {
            get
            {  
                return this.SensorSet == null ? null : GetSensorBucketCounts(this.SensorSet, this.TrainInstances);
            }
        }

        /// <summary>
        /// Gets the sensor bucket counts by hour.
        /// </summary>
        public Dictionary<Sensor, Dictionary<int, Dictionary<string, double>>> SensorBucketCountsByHour
        {
            get
            {
                if (this.SensorSet == null)
                {
                    return null;
                }

                var d = new Dictionary<Sensor, Dictionary<int, Dictionary<string, double>>>();
                for (int i = 0; i < 24; i++)
                {
                    foreach (var kvp in GetSensorBucketCounts(this.SensorSet, this.TrainInstances.Where(ia => ia.DateTime.Hour == i).ToArray()))
                    {
                        if (i == 0)
                        {
                            d[kvp.Key] = new Dictionary<int, Dictionary<string, double>>();
                        }

                        d[kvp.Key].Add(i, kvp.Value);
                    }
                }

                return d;
            }
        }
        #endif

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
       
        #endregion

        #region Public Methods and Operators

//        /// <summary>
//        /// Gets the priors.
//        /// </summary>
//        /// <returns>The priors.</returns>
//        public Marginals GetPriors()
//        {
//            return new Marginals
//            {
//                Inputs = this,
//                ActivityWeights =
//                    this.Residents.ToDictionary(
//                        resident => resident,
//                        resident =>
//                        this.Activities.ToDictionary(
//                            activity => activity,
//                            activity => this.SensorSet.SensorBuckets.ToDictionary(s => s, s => new Gaussian(0, 1)))),
//                ResidentWeights = 
//                    this.Activities.ToDictionary(
//                        activity => activity,
//                        activity => 
//                        this.Residents.ToDictionary(
//                            resident => resident,
//                            resident => 
//                            this.SensorSet.SensorBuckets.ToDictionary(
//                                bucket => bucket,
//                                bucket => new Gaussian(0, 1))))
//                //// Thresholds = Inputs.Residents.ToDictionary(ia => ia, ia => Inputs.Activities.ToDictionary(x => x, x => new Gaussian(0, 1)))
//            };
//        }

        /// <summary>
        /// Gets the residents.
        /// </summary>
        /// <returns>The residents.</returns>
        /// <param name="includeUnknown">If set to <c>true</c> include unknown.</param>
        public IList<Resident> GetResidents(bool includeUnknown = false)
        {
            var residents = Enumerable.Range(0, this.NumberOfResidents).Select(ia => new Resident { Name = string.Format("R{0}", ia + 1), Index = ia });
            return includeUnknown ? new[] { Resident.Unknown }.Concat(residents).ToArray() : residents.ToArray();
        }

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <param name="includeUnknown">
        /// The include Unknown.
        /// </param>
        /// <returns>
        /// The activities.
        /// </returns>
        public IList<Activity> GetActivities(bool includeUnknown = false)
        {
            var a = this.activities.OrderBy(ia => ia).Select((ia, i) => new Activity { Name = ia, Index = i });
            return includeUnknown ? new[] { Activity.Unknown }.Concat(a).ToArray() : a.ToArray();
        }

        #endregion

        /// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="SphereEngine.Inputs"/>.
        /// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="SphereEngine.Inputs"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Inputs: SensorSet={0}, Name={1}]", SensorSet, this.Name);
        }

        #if FALSE
        /// <summary>
        /// Gets the sparse values.
        /// </summary>
        /// <param name="instances">The instances.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="values">The values.</param>
        /// <param name="counts">The counts.</param>
        public void GetSparseValues(IList<Instance> instances, out int[][] indices, out double[][] values, out int[] counts)
        {
            indices = new int[instances.Count][];
            values = new double[instances.Count][];
            counts = new int[instances.Count];

            for (int i = 0; i < instances.Count; i++)
            {
                indices[i] = new int[instances[i].SensorReadings.Count];
                values[i] = new double[instances[i].SensorReadings.Count];
                counts[i] = instances[i].SensorReadings.Count;

                int j = 0;
                foreach (var kvp in instances[i].SensorReadings)
                {
                    indices[i][j] = kvp.Bucket.Index + kvp.Bucket.Offset;
                    values[i][j] = kvp.Value;
                    j++;
                }
            }
        }

        /// <summary>
        /// Gets the sensor bucket counts.
        /// </summary>
        /// <param name="sensorSet">The sensor set.</param>
        /// <param name="instances">The instances.</param>
        /// <returns>The sensor bucket counts.</returns>
        private static Dictionary<Sensor, Dictionary<string, double>> GetSensorBucketCounts(SensorSet sensorSet, IList<Instance> instances)
        {
            var d = new Dictionary<Sensor, Dictionary<string, double>>();
            foreach (var sensor in sensorSet.Sensors)
            {
                d[sensor] = new Dictionary<string, double>();
                var binarySensor = sensor as BinarySensor;
                if (binarySensor != null)
                {
                    Sensor sensor1 = sensor;
                    d[sensor][binarySensor.BucketNames[0]] =
                           instances.Count(x => x.SensorReadings.ContainsKey(sensor1.Buckets[0]) && x.SensorReadings[sensor1.Buckets[0]] < 0.5);
                    d[sensor][binarySensor.BucketNames[1]] =
                           instances.Count(x => x.SensorReadings.ContainsKey(sensor1.Buckets[0]) && x.SensorReadings[sensor1.Buckets[0]] > 0.5);
                }
                else
                {
                    for (int i = 0; i < sensor.Buckets.Count; i++)
                    {
                        var bucket = sensor.Buckets[i];
                        d[sensor][sensor.GetDescription(i)] =
                            instances.Count(x => x.SensorReadings.ContainsKey(bucket) && x.SensorReadings[bucket] > 0.5);
                    }
                }
            }

            return d;
        }
        #endif
    }
}