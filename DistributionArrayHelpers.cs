//
// DistributionArrayHelpers.cs
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
    using System.Collections.Generic;
    using System.Linq;

    using MicrosoftResearch.Infer.Distributions;
    using MicrosoftResearch.Infer.Maths;

    /// <summary>
    /// The distribution array helpers.
    /// </summary>
    public static class DistributionArrayHelpers
    {
        /// <summary>
        /// Create a distribution array.
        /// </summary>
        /// <typeparam name="TDistribution">The type of the distribution.</typeparam>
        /// <param name="distributions">The distributions.</param>
        /// <returns>
        /// The DistributionStructArray{TDistribution, double}
        /// </returns>
        public static DistributionStructArray<TDistribution, double> DistributionArray<TDistribution>(IEnumerable<TDistribution> distributions)
            where TDistribution : struct, IDistribution<double>, SettableToProduct<TDistribution>, SettableToRatio<TDistribution>,
                SettableToPower<TDistribution>, SettableToWeightedSum<TDistribution>, CanGetLogAverageOf<TDistribution>,
                CanGetLogAverageOfPower<TDistribution>, CanGetAverageLog<TDistribution>, Sampleable<double>, SettableToUniform
        {
            return distributions == null ? null : (DistributionStructArray<TDistribution, double>)Distribution<double>.Array(distributions.ToArray());
        }

        /// <summary>
        /// Copy the distribution array.
        /// </summary>
        /// <typeparam name="TDistribution">The type of the distribution.</typeparam>
        /// <param name="arrayToCopy">The array to copy.</param>
        /// <returns>
        /// The <see cref="DistributionStructArray{TDistribution, Double}" />.
        /// </returns>
        public static DistributionStructArray<TDistribution, double> Copy<TDistribution>(IEnumerable<TDistribution> arrayToCopy)
            where TDistribution : struct, IDistribution<double>, SettableToProduct<TDistribution>, SettableToRatio<TDistribution>,
                SettableToPower<TDistribution>, SettableToWeightedSum<TDistribution>, CanGetLogAverageOf<TDistribution>,
                CanGetLogAverageOfPower<TDistribution>, CanGetAverageLog<TDistribution>, Sampleable<double>
        {
            return (DistributionStructArray<TDistribution, double>)Distribution<double>.Array(arrayToCopy.ToArray());
        }

        /// <summary>
        /// Copy the distribution array.
        /// </summary>
        /// <typeparam name="TDistribution">The type of the distribution.</typeparam>
        /// <param name="arrayToCopy">The array to copy.</param>
        /// <returns>
        /// The <see cref="DistributionStructArray{TDistribution, Double}" /> array.
        /// </returns>
        public static DistributionStructArray<TDistribution, double>[] Copy<TDistribution>(IEnumerable<IList<TDistribution>> arrayToCopy)
            where TDistribution : struct, IDistribution<double>, SettableToProduct<TDistribution>, SettableToRatio<TDistribution>,
                SettableToPower<TDistribution>, SettableToWeightedSum<TDistribution>, CanGetLogAverageOf<TDistribution>,
                CanGetLogAverageOfPower<TDistribution>, CanGetAverageLog<TDistribution>, Sampleable<double>
        {
            return arrayToCopy.Select(Copy).ToArray();
        }

        /// <summary>
        /// The uniform gaussian array.
        /// </summary>
        /// <typeparam name="TDistribution">The type of the distribution.</typeparam>
        /// <param name="count">The count.</param>
        /// <returns>
        /// The array of distributions.
        /// </returns>
        public static DistributionStructArray<TDistribution, double> UniformArray<TDistribution>(int count)
            where TDistribution : struct, IDistribution<double>, SettableToProduct<TDistribution>, SettableToRatio<TDistribution>,
                SettableToPower<TDistribution>, SettableToWeightedSum<TDistribution>, CanGetLogAverageOf<TDistribution>,
                CanGetLogAverageOfPower<TDistribution>, CanGetAverageLog<TDistribution>, Sampleable<double>, SettableToUniform
        {
            Func<TDistribution, TDistribution> uniform = x =>
                {
                    x.SetToUniform();
                    return x;
                };

            return
                (DistributionStructArray<TDistribution, double>)
                Distribution<double>.Array(
                    Enumerable.Repeat(uniform((TDistribution)Activator.CreateInstance(typeof(TDistribution))), count).ToArray());
        }

        /// <summary>
        /// Creates the gaussian array.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="variance">The variance.</param>
        /// <returns>The <see cref="DistributionStructArray{Gaussian, Double}" /> array.</returns>
        public static DistributionStructArray<Gaussian, double> CreateGaussianArray(int count, double mean, double variance)
        {
            return
                (DistributionStructArray<Gaussian, double>)
                Distribution<double>.Array(Enumerable.Repeat(Gaussian.FromMeanAndVariance(mean, variance), count).ToArray());
        }

        /// <summary>
        /// Create an array of Gaussian distributions with random mean and unit variance
        /// </summary>
        /// <param name="row">Number of rows</param>
        /// <param name="col">Number of columns</param>
        /// <returns>The array as a distribution over a jagged double array domain</returns>
        public static IDistribution<double[][]> RandomGaussianArray(int row, int col)
        {
            var array = new Gaussian[row][];
            for (int i = 0; i < row; i++)
            {
                array[i] = new Gaussian[col];
                for (int j = 0; j < col; j++)
                {
                    array[i][j] = Gaussian.FromMeanAndVariance(Rand.Normal(), 1);
                }
            }
            return Distribution<double>.Array(array);
        }

        /// <summary>
        /// Creates the gamma array.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The <see cref="DistributionStructArray{Gamma, Double}" /> array.</returns>
        public static DistributionStructArray<Gamma, double> CreateGammaArray(int count, double shape, double scale)
        {
            return
                (DistributionStructArray<Gamma, double>)
                Distribution<double>.Array(Enumerable.Repeat(Gamma.FromShapeAndScale(shape, scale), count).ToArray());
        }

        /// <summary>
        /// Repeats the specified distribution.
        /// </summary>
        /// <typeparam name="TDistribution">The type of the distribution.</typeparam>
        /// <param name="distribution">The distribution.</param>
        /// <param name="count">The count.</param>
        /// <returns>The <see cref="DistributionStructArray{TDistribution, Double}" /> array.</returns>
        public static DistributionStructArray<TDistribution, double> Repeat<TDistribution>(TDistribution distribution, int count)
            where TDistribution : struct, IDistribution<double>, SettableToProduct<TDistribution>, SettableToRatio<TDistribution>,
                SettableToPower<TDistribution>, SettableToWeightedSum<TDistribution>, CanGetLogAverageOf<TDistribution>,
                CanGetLogAverageOfPower<TDistribution>, CanGetAverageLog<TDistribution>, Sampleable<double>, SettableToUniform
        {
            return (DistributionStructArray<TDistribution, double>)Distribution<double>.Array(Enumerable.Repeat(distribution, count).ToArray());
        }
    }
}