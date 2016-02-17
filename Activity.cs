//
// Activity.cs
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

    /// <summary>
    /// The Activity.
    /// </summary>
    public class Activity : IEquatable<Activity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SphereEngine.Activity"/> class.
        /// </summary>
        public Activity()
        {
            // Set the index to something nonsensical
            this.Index = int.MinValue;
        }

        /// <summary>
        /// Sets the unknown.
        /// </summary>
        /// <value>The unknown.</value>
        public static Activity Unknown
        {
            get
            {
                return new Activity { Name = "Unknown", Index = -1 };
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the resident.
        /// </summary>
        /// <value>The resident.</value>
        public Resident Resident { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Activity other)
        {
            return !ReferenceEquals(other, null) && this.Name == other.Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Activity);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            // Use anonymous tye to generate hash code
            // return new { this.Name, this.Resident }.GetHashCode();
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Activity"/>, is equal to another instance.
        /// </summary>
        /// <param name="a1">A1.</param>
        /// <param name="a2">A2.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Activity"/> is equal to the other instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Activity a1, Activity a2)
        {
            return object.ReferenceEquals(a1, null) ? object.ReferenceEquals(a2, null) : a1.Equals(a2);

        }

        /// <summary>
        /// Determines whether the specified <see cref="Activity"/>, is not equal to another instance.
        /// </summary>
        /// <param name="a1">A1.</param>
        /// <param name="a2">A2.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Activity"/> is not equal to the other instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Activity a1, Activity a2)
        {
            return !(a1 == a2);
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SphereEngine.Activity"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="SphereEngine.Activity"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Activity: Name={0}, Resident={1}]", this.Name, this.Resident);
        }
    }
}
