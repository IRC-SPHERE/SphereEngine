//
// BPMUtils.cs
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
    using MicrosoftResearch.Infer.Maths;
    using MicrosoftResearch.Infer.Models;
    
    using VariableArray2DDouble = MicrosoftResearch.Infer.Models.VariableArray<MicrosoftResearch.Infer.Models.VariableArray<double>, double[][]>;

    /// <summary>
    /// Computes class scores and defines constraints for both dense and sparse feature representations.
    /// </summary>
    public static class BpmUtils
    {
        /// <summary>
        /// Computes the class scores for dense features.
        /// </summary>
        /// <param name="weights">Weights (per class)</param>
        /// <param name="features">Vector of values</param>
        /// <param name="noisePrecision">Noise precision</param>
        /// <returns>Score for each class</returns>
        public static VariableArray<double> ComputeClassScores(VariableArray<Vector> weights, Variable<Vector> features, Variable<double> noisePrecision)
        {
            Range c = weights.Range.Clone().Named("k");
            VariableArray<double> score = Variable.Array<double>(c).Named("score");
            VariableArray<double> scorePlusNoise = Variable.Array<double>(c).Named("scorePlusNoise");
            score[c] = Variable.InnerProduct(weights[c], features);
            scorePlusNoise[c] = Variable.GaussianFromMeanAndPrecision(score[c], noisePrecision);
            return scorePlusNoise;
        }

		/// <summary>
		/// Computes the class scores for dense features.
		/// </summary>
		/// <param name="weights">Weights (per class)</param>
		/// <param name="features">Vector of values</param>
		/// <param name="noisePrecision">Noise precision</param>
		/// <returns>Score for each class</returns>
		public static VariableArray<double> ComputeClassScores(VariableArray<VariableArray<double>, double[][]> weights, Variable<Vector> features, Variable<double> noisePrecision)
		{
			Range c = weights.Range.Clone().Named("k");
			VariableArray<double> score = Variable.Array<double>(c).Named("score");
			VariableArray<double> scorePlusNoise = Variable.Array<double>(c).Named("scorePlusNoise");
			score[c] = Variable.InnerProduct(weights[c], features);
			scorePlusNoise[c] = Variable.GaussianFromMeanAndPrecision(score[c], noisePrecision);
			return scorePlusNoise;
		}

		/// <summary>
		/// Computes the class scores for dense features.
		/// </summary>
		/// <param name="weights">Weights (per class)</param>
		/// <param name="features">Vector of values</param>
		/// <param name="noisePrecision">Noise precision</param>
		/// <returns>Score for each class</returns>
		public static VariableArray<double> ComputeClassScores(VariableArray<VariableArray<double>, double[][]> weights, VariableArray<double> features, Variable<double> noisePrecision)
		{
			// VMP version
			// return ComputeClassScores(weights, Variable.Vector(features).Named("vectorFeatures"), noisePrecision);

			// EP friendly version
			Range c = weights.Range.Clone().Named("k");
			Range f = features.Range.Clone().Named("f");
			VariableArray<double> score = Variable.Array<double>(c).Named("score");
			VariableArray<double> scorePlusNoise = Variable.Array<double>(c).Named("scorePlusNoise");
			using (Variable.ForEach(c))
			{
				var products = Variable.Array<double>(f).Named("products");
				using (Variable.ForEach(f))
				{
					products[f] = weights[c][f] * features[f];
				}

				score[c] = Variable.Sum(products);
				scorePlusNoise[c] = Variable.GaussianFromMeanAndPrecision(score[c], noisePrecision);
			}

			return scorePlusNoise;
		}

        /// <summary>
        /// Computes the class scores for sparse features.
        /// </summary>
        /// <param name="weights">Weight array per class.</param>
        /// <param name="values">Array of values.</param>
        /// <param name="indices">Array of indices.</param>
        /// <param name = "activeSensor">Active sensor.</param>
        /// <param name="noisePrecision">Noise precision.</param>
        /// <param name="prefix">Prefix for variable names.</param>
        /// <returns>Score for each class</returns>
        public static VariableArray<double> ComputeClassScores(
            VariableArray2DDouble weights, 
            VariableArray<double> values, 
            VariableArray<int> indices, 
            Range activeSensor,
            Variable<double> noisePrecision,
            string prefix = "activity")
        {
            var clone = weights.Range.Clone().Named(prefix + "Clone");
            var score = Variable.Array<double>(clone).Named(prefix + "Score");
            var scorePlusNoise = Variable.Array<double>(clone).Named(prefix + "NoisyScore");
            scorePlusNoise.AddAttribute(MicrosoftResearch.Infer.QueryTypes.Marginal);

            var sparseWeights = Variable.Array(Variable.Array<double>(activeSensor), clone).Named(prefix + "SparseWeights");
            var product = Variable.Array(Variable.Array<double>(activeSensor), clone).Named(prefix + "Product");
            
            sparseWeights[clone] = Variable.Subarray(weights[clone], indices);
            product[clone][activeSensor] = values[activeSensor] * sparseWeights[clone][activeSensor];
            score[clone] = Variable.Sum(product[clone]);
            scorePlusNoise[clone] = Variable.GaussianFromMeanAndPrecision(score[clone], noisePrecision);

            return scorePlusNoise;
        }

        /// <summary>
        /// Builds a multiclass switch for the specified integer variable
        /// which builds a set of <see cref="ConstrainArgMax"/> constraints based
        /// on the value of the variable.
        /// </summary>
        /// <param name="argmax">The specified integer variable</param>
        /// <param name="score">The vector of score variables</param>
        /// <param name="prefix">Prefix for variable names.</param>
        /// <param name="current">Index of the current activity/resident (optional).</param> 
        public static void ConstrainMaximum(Variable<int> argmax, VariableArray<double> score, string prefix = "activity", Variable<int> current = null)
        {
            var clone = score.Range.Clone();
            using (var block = Variable.ForEach(clone))
            {
                var isMax = (argmax == block.Index).Named(prefix + "IsMax");
                using (Variable.If(isMax))
                {
                    ConstrainArgMax(block.Index, score, prefix);

                    if (!ReferenceEquals(current, null))
                    {
                        Variable.ConstrainEqual(block.Index, current);
                    }
                }
            }
        }

        /// <summary>
        /// Constrains the score for the specified class to be larger 
        /// than all the scores at the other classes.
        /// </summary>
        /// <param name="argmax">The specified integer variable</param>
        /// <param name="score">The vector of score variables</param>
        /// <param name="prefix">Prefix for variable names.</param>
        public static void ConstrainArgMax(Variable<int> argmax, VariableArray<double> score, string prefix = "activity")
        {
            using (var block = Variable.ForEach(score.Range))
            {
                var isArgMax = (argmax == block.Index).Named(prefix + "IsArgMax");
                using (Variable.IfNot(isArgMax))
                {
                    // scoreDiff = Factor.Difference(scorePlusNoise__1_[activities[resident][example]], scorePlusNoise__1_[activityClone]);
                    var diff = (score[argmax] - score[block.Index]).Named(prefix + "ScoreDiff");
                    Variable.ConstrainTrue((diff > 0).Named(prefix + "PosDiff"));
                }
            }
        }
    }
}
