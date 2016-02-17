//
// Resident.cs
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
    /// Resident.
    /// </summary>
    public class Resident : IEquatable<Resident>
    {
        /// <summary>
        /// Gets the unknown.
        /// </summary>
        /// <value>The unknown.</value>
        public static Resident Unknown
        {
            get
            {
                return new Resident { Name = "Unknown", Index = -1 };
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
        /// Determines whether the specified <see cref="Resident"/>, is equal to another instance.
        /// </summary>
        /// <param name="r1">R1.</param>
        /// <param name="r2">R2.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Resident"/> is equal to the other instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Resident r1, Resident r2)
        {
            return object.ReferenceEquals(r1, null) ? object.ReferenceEquals(r2, null) : r1.Equals(r2);

        }

        /// <summary>
        /// Determines whether the specified <see cref="Resident"/>, is not equal to another instance.
        /// </summary>
        /// <param name="r1">R1.</param>
        /// <param name="r2">R2.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Resident"/> is not equal to the other instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Resident r1, Resident r2)
        {
            return !(r1 == r2);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Casas.Resident"/> is equal to the current <see cref="Casas.Resident"/>.
        /// </summary>
        /// <param name="other">The <see cref="Casas.Resident"/> to compare with the current <see cref="Casas.Resident"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Casas.Resident"/> is equal to the current
        /// <see cref="Casas.Resident"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Resident other)
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
            return this.Equals(obj as Resident);
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
            return new { this.Name }.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Casas.Resident"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Casas.Resident"/>.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
