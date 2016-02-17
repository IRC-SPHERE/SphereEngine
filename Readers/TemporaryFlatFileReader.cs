//
// TemporaryFlatFileReader.cs
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

namespace SphereEngine.Readers
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Temporary flat file reader.
	/// </summary>
	public class TemporaryFlatFileReader : DataReader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SphereEngine.Readers.TemporaryFlatFileReader"/> class.
		/// </summary>
		public TemporaryFlatFileReader()
		{
		}

		/// <summary>
		/// Gets or sets the name of the electricity file.
		/// </summary>
		/// <value>The name of the electricity file.</value>
		public string ElectricityFileName { get; set; }

		/// <summary>
		/// Gets or sets the name of the libelium file.
		/// </summary>
		/// <value>The name of the libelium file.</value>
		public string LibeliumFileName { get; set; }

		/// <summary>
		/// Gets or sets the name of the wearable file.
		/// </summary>
		/// <value>The name of the wearable file.</value>
		public string WearableFileName { get; set; }

		/// <summary>
		/// Gets or sets the name of the W p1 file.
		/// </summary>
		/// <value>The name of the W p1 file.</value>
		public string WP1FileName { get; set; }

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public override IEnumerator<Instance> GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}

