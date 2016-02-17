//
// Utils.cs
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
// AAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.



namespace SphereEngine
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
	using MathNet.Numerics;
    using MicrosoftResearch.Infer.Distributions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The index of the max of the array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public static int ArgMax(this IList<double> array)
        {
            int index = -1;
            double max = double.MinValue;
            for (int i = 0; i < array.Count; i++)
            {
                if (!(array[i] > max))
                {
                    continue;
                }

                max = array[i];
                index = i;
            }

            return index;
        }

        public static int ArgMax(this IList<int> array)
        {
            int index = -1;
            int max = int.MinValue;
            for (int i = 0; i < array.Count; i++)
            {
                if (!(array[i] > max))
                {
                    continue;
                }

                max = array[i];
                index = i;
            }

            return index;
        }

        /// <summary>
        /// The index of the max of the dict.
        /// </summary>
        /// <param name="dict">The dict.</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public static Activity ArgMax(this Dictionary<Activity, Bernoulli> dict)
        {
            return dict.ArgMax(x => x.GetProbTrue());
        }

        /// <summary>
        /// Arguments the max.
        /// </summary>
        /// <returns>The max.</returns>
        /// <param name="dict">The dictionary.</param>
        /// <param name="mapper">The mapper.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static TKey ArgMax<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<TValue, double> mapper)
        {
            TKey index = default(TKey);
            double max = double.MinValue;
            foreach (var kvp in dict)
            {
                double probTrue = mapper(kvp.Value);
                if (!(probTrue > max))
                {
                    continue;
                }

                max = probTrue;
                index = kvp.Key;
            }

            return index;
        }

        /// <summary>
        /// Parses the enumerated type.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns>
        /// The <see cref="TEnum" />.
        /// </returns>
        public static TEnum ParseEnum<TEnum>(string stringToParse)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), stringToParse, true);
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="enumerationValue">Enumeration value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string GetDescription<T>(this object enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    // Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            // If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        /// <summary>
        /// Page the specified source and predicate.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<IList<T>> Page<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<IEnumerable<IEnumerable<T>>>() != null);

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var currentPage = new List<T> { enumerator.Current };

                    while (enumerator.MoveNext() && predicate(enumerator.Current))
                    {
                        currentPage.Add(enumerator.Current);
                    }

                    yield return new ReadOnlyCollection<T>(currentPage);
                }
            }
        }

        /// <summary>
        /// Next batch.
        /// </summary>
        /// <returns>The batch.</returns>
        /// <param name="enumerator">The enumerator.</param>
        /// <param name="predicate">The predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="count">The count.</param>
        public static IList<T> NextBatch<T>(this IEnumerator<T> enumerator, Func<T, bool> predicate, int count = int.MaxValue)
        {
            if (!enumerator.MoveNext())
            {
                return null;
            }

            int i = 0;
            var currentPage = new List<T>();
            if (predicate(enumerator.Current) && i < count)
            {
                currentPage.Add(enumerator.Current);
                i++;
            }

            while (enumerator.MoveNext() && i < count)
            {
                if (predicate(enumerator.Current))
                {
                    currentPage.Add(enumerator.Current);
                    i++;
                }
            }

            return currentPage;
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="filename">Filename.</param>
        public static void WriteToFile(this object obj, string filename)
        {
            using (var file = new StreamWriter(filename))
            {
                string json = JsonConvert.SerializeObject(
                    obj, 
                    Formatting.Indented, 
                    new JsonSerializerSettings
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });
                file.Write(json);
            }
        }

        /// <summary>
        /// Creates the uniform gaussians.
        /// </summary>
        /// <returns>The uniform gaussians.</returns>
        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static Gaussian[][] CreateUniformGaussians(int first, int second)
        {
            return Enumerable.Range(0, first).Select(
                f => Enumerable.Range(0, second).Select(
                        t => Gaussian.Uniform()).ToArray()).ToArray();
        }

        /// <summary>
        /// Creates the uniform gaussians.
        /// </summary>
        /// <returns>The uniform gaussians.</returns>
        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        /// <param name="third">Third.</param>
        public static Gaussian[][][] CreateUniformGaussians(int first, int second, int third)
        {
            return Enumerable.Range(0, first).Select(
                f => Enumerable.Range(0, second).Select(
                    s => Enumerable.Range(0, third).Select(
                        t => Gaussian.Uniform()).ToArray()).ToArray()).ToArray();
        }

//		public static void RemoveAll<TEnumerable>(this ICollection<TEnumerable> enumerable, Func<TEnumerable, bool> selector)
//		{
//			var toRemove = enumerable.Where(selector);
//			foreach (var element in toRemove)
//			{
//				enumerable.Remove(element);
//			}
//		}

		/// <summary>
		/// Cumulative sum.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>The cumulative sum.</returns>
		public static IEnumerable<double> CumulativeSum(this IEnumerable<double> sequence)
		{
			double sum = 0.0;
			foreach (var d in sequence)
			{
				sum += d;
				yield return sum;
			}
    	}

		/// <summary>
		/// Cumulative sum.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="selector">The selector function.</param>
		/// <returns>The cumulative sum.</returns>
		public static IEnumerable<double> CumulativeSum<T>(this IEnumerable<T> sequence, Func<T, double> selector)
		{
			double sum = 0.0;
			foreach (var d in sequence)
			{
				sum += selector(d);
				yield return sum;
			}
		}
			

		/// <summary>
		/// Cumulative average.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>The cumulative average.</returns>
		public static IEnumerable<double> CumulativeAverage(this IEnumerable<double> sequence)
		{
			double sum = 0.0;
			int length = 0;
			foreach (var d in sequence)
			{
				sum += d;
				length += 1;
				yield return sum / length;
			}
		}

		/// <summary>
		/// Cumulative average.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="selector">The selector function.</param>
		/// <returns>The cumulative average.</returns>
		public static IEnumerable<double> CumulativeAverage<T>(this IEnumerable<T> sequence, Func<T, double> selector)
		{
			double sum = 0.0;
			int length = 0;
			foreach (var d in sequence)
			{
				sum += selector(d);
				length += 1;
				yield return sum / length;
			}
		}

		/// <summary>
		/// Column averages.
		/// </summary>
		/// <returns>The average.</returns>
		/// <param name="array">Array.</param>
		public static double[] ColumnAverage(this double[][] array)
		{
			int minLength = array.Min(ia => ia.Length);
			return Enumerable.Range(0, minLength).Select(i => array.Average(a => a[i])).ToArray();
		}

		/// <summary>
		/// Column standard deviations.
		/// </summary>
		/// <returns>The standard deviation.</returns>
		/// <param name="array">Array.</param>
		public static double[] ColumnStandardDeviation(this double[][] array)
		{
			int minLength = array.Min(ia => ia.Length);
			return Enumerable.Range(0, minLength).Select(i => array.StandardDeviation(a => a[i])).ToArray();
		}

		/// <summary>
		/// Standard deviation.
		/// </summary>
		/// <returns>The deviation.</returns>
		/// <param name="list">List.</param>
		/// <param name="values">Values.</param>
		/// <param name="unbiased">If set to <c>true</c> unbiased.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static double StandardDeviation<T>(this IEnumerable<T> list, Func<T, double> values, bool unbiased = true)
		{
			// ref: http://stackoverflow.com/questions/2253874/linq-equivalent-for-standard-deviation
			// ref: http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/ 
			var mean = 0.0;
			var sum = 0.0;
			var stdDev = 0.0;
			var n = 0;
			foreach (var v in list.Select(values))
			{
				n++;
				var delta = v - mean;
				mean += delta / n;
				sum += delta * (v - mean);
			}

			if (1 < n)
			{
				stdDev = Math.Sqrt(sum / (unbiased ? (n - 1) : n));
			}

			return stdDev; 

		}
        
        /// <summary>
        /// Returns the expected norm of a vector of normal variables of length n 
        /// with mean 0 and standard deviation sigma. 
        /// See http://math.stackexchange.com/questions/827826/average-norm-of-a-n-dimensional-vector-given-by-a-normal-distribution?rq=1
        /// </summary>
        /// <param name="n">The length of the vector.</param>
        /// <param name="sigma">The standard deviation of the normal variables.</param>
        public static double ExpectedNorm(int n, double sigma)
        {
            return Math.Sqrt(2.0) * sigma *  (SpecialFunctions.Gamma((n + 1.0) / 2.0) / SpecialFunctions.Gamma(n / 2.0));
        }
        
        /// <summary>
        /// The sigma for a normalised random sequnce of length n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="1"></param>
        /// <returns></returns>
        public static double NormalisedRandomSigma(int n, double norm = 1.0)
        {
            return norm / Math.Sqrt(2.0) * SpecialFunctions.Gamma(n / 2.0) / SpecialFunctions.Gamma((n + 1.0) / 2.0);
        }
        
        /// <summary>
        /// Generate a sequnce of n random variables with the given expected norm 
        /// </summary>
        /// <param name="n">The length of the sequnce.</param>
        /// <param name="norm">The expected norm of the vector.</param>
        /// <returns></returns>
        public static Gaussian[] NormalisedRandomSequence(int n, double norm = 1)
        {
            double sigma = NormalisedRandomSigma(n, norm);
            return Enumerable.Range(0, n).Select(x => new Gaussian(0.0, sigma * sigma)).ToArray();
        }
        
        private static void GetMeanAndVarianceImproper(Gaussian g, out double mean, out double variance)
        {
            if (g.IsPointMass)
            {
                variance = 0;
                mean = g.Point;
            }
            else if (g.Precision == 0.0)
            {
                variance = Double.PositiveInfinity;
                mean = 0.0;
            }
            else
            {
                variance = 1.0/g.Precision;
                // variance can be infinity even if Precision > 0
                if (double.IsPositiveInfinity(variance)) mean = 0;
                else mean = variance*g.MeanTimesPrecision;
            }
        }
  	}
}