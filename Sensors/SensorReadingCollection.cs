//
// SensorReadingDictionary.cs
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
    using System.Linq;
    using System.Collections.Generic;


    /// <summary>
    /// The sensor reading dictionary.
    /// </summary>
    /*
    public class SensorReadingCollection : IDictionary<SensorBucket, double>
    {
        /// <summary>
        /// The dictionary.
        /// </summary>
        private readonly Dictionary<SensorBucket, double> dictionary = new Dictionary<SensorBucket, double>();

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public ICollection<SensorBucket> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ICollection<double> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public double this[SensorBucket key]
        {
            get
            {
                return this.dictionary[key];
            }

            set
            {
                this.dictionary[key] = value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<SensorBucket, double>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<SensorBucket, double> item)
        {
            this.dictionary.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<SensorBucket, double> item)
        {
            return this.dictionary.ContainsKey(item.Key) && Math.Abs(this.dictionary[item.Key] - item.Value) < double.Epsilon;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<SensorBucket, double>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<SensorBucket, double> item)
        {
            return this.dictionary.Remove(item.Key);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(SensorBucket key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(SensorBucket key, double value)
        {
            this.dictionary.Add(key, value);
        }

        /// <summary>
        /// Adds the specified sensor reading.
        /// </summary>
        /// <param name="sensorReading">The sensor reading.</param>
        public void Add(SensorReading sensorReading)
        {
            this.dictionary.Add(sensorReading.Bucket, sensorReading.Value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(SensorBucket key)
        {
            return this.dictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(SensorBucket key, out double value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }


        public override string ToString()
        {
            return string.Format("[SensorReadingDictionary: Count={0}, Items={1}]", 
                this.Count, string.Join(", ", this.Select(ia => string.Format("({0}: {1})", ia.Key, ia.Value))));
        }
    }
*/
    /// <summary>
    /// Sensor reading collection.
    /// </summary>
    public class SensorReadingCollection : HashSet<SensorReading>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.SensorReadingCollection"/> class.
        /// </summary>
        public SensorReadingCollection() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Casas.Sensors.SensorReadingCollection"/> class.
        /// </summary>
        /// <param name="sensorReadings">Sensor readings.</param>
        public SensorReadingCollection(HashSet<SensorReading> sensorReadings) : base(sensorReadings) {}

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get
            {
                return string.Format("[Count={0}, Items={1}]", 
                    this.Count, string.Join(", ", this.Select(ia => string.Format("({0}: {1})", ia.Bucket, ia.Value))));
            }
        }

        /// <summary>
        /// Contains the key.
        /// </summary>
        /// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
        /// <param name="sensorBucket">Sensor bucket.</param>
        public bool ContainsKey(SensorBucket sensorBucket)
        {
            return this.Any(ia => ia.Bucket == sensorBucket);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public double this[SensorBucket key]
        {
            get
            {
                return this.First(ia => ia.Bucket == key).Value;
            }

//            set
//            {
//                this.First(ia => ia.Bucket == key).Value = value;
//            }
        }

        /// <Docs>The item to remove from the current collection.</Docs>
        /// <para>Removes the first occurrence of an item from the current collection.</para>
        /// <summary>
        /// Remove the specified sensorBucket.
        /// </summary>
        /// <param name="sensorBucket">Sensor bucket.</param>
        public void Remove(SensorBucket sensorBucket)
        {
            this.Remove(this.First(ia => ia.Bucket == sensorBucket));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.SensorReadingCollection"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Casas.Sensors.SensorReadingCollection"/>.</returns>
        public override string ToString()
        {
            return this.Summary;
        }
    }
}
