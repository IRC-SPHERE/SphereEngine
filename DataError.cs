//
// DataError.cs
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
using System;

namespace SphereEngine
{
	/// <summary>
	/// Error Identifier.
	/// </summary>
	public enum ErrorID
	{
		UnknownSensor,
		IncorrectSensorState,
		SensorReadingError,
		UnknownActivity,
		IncorrectActivityState,
		ActivityStatusError
	}

	/// <summary>
	/// Data error.
	/// </summary>
	public class DataError
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public ErrorID Id { get; set; }

		/// <summary>
		/// Gets or sets the sensor string.
		/// </summary>
		/// <value>The sensor string.</value>
		public string Sensor { get; set; }

		/// <summary>
		/// Gets or sets the sensor reading.
		/// </summary>
		/// <value>The message.</value>
		public string SensorReading { get; set; }

		/// <summary>
		/// Gets or sets the activity.
		/// </summary>
		/// <value>The activity.</value>
		public string Activity { get; set; }

		/// <summary>
		/// Gets or sets the resident.
		/// </summary>
		/// <value>The resident.</value>
		public string Resident { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		public override string ToString()
		{
			return string.Format("[DataError: Id={0}", Id) +
				(string.IsNullOrEmpty(Sensor) ? string.Empty : string.Format(", Sensor={0}", Sensor)) +
				(string.IsNullOrEmpty(SensorReading) ? string.Empty : string.Format(", SensorReading={0}", SensorReading)) +
				(string.IsNullOrEmpty(Activity) ? string.Empty : string.Format(", Activity={0}", Activity)) +
				(string.IsNullOrEmpty(Resident) ? string.Empty : string.Format(", Resident={0}", Resident)) +
				(string.IsNullOrEmpty(Message) ? string.Empty : string.Format(", Message={0}", Message)) +
				"]";
		}
	}
}

