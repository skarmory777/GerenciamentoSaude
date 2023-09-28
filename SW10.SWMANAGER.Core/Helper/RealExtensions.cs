using System;
using System.Diagnostics.CodeAnalysis;

namespace SW10.SWMANAGER.Helper
{
    public static class RealExtensions
    {
        /// <summary>
        /// The e 3.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static double E3 = 1E-3D;

        /// <summary>
        /// The e 4.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static double E4 = 1E-4D;

        /// <summary>
        /// The e 5.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static double E5 = 1E-5D;

        /// <summary>
        /// The e 6.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static double E6 = 1E-6D;

        /// <summary>
        /// The e max.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static double EMax = 1E-15D;

        /// <summary>
        /// The epsilon max.
        /// </summary>
        public static Epsilon EpsilonMax => new Epsilon(EMax);

        /// <summary>
        /// The epsilon 3.
        /// </summary>
        public static Epsilon Epsilon3 => new Epsilon(EMax);

        /// <summary>
        /// The epsilon 4.
        /// </summary>
        public static Epsilon Epsilon4 => new Epsilon(EMax);

        /// <summary>
        /// The epsilon 5.
        /// </summary>
        public static Epsilon Epsilon5 => new Epsilon(EMax);

        /// <summary>
        /// The epsilon 6.
        /// </summary>
        public static Epsilon Epsilon6 => new Epsilon(EMax);
        
        /// <summary>
        /// The equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Equal(this double a, double b, Epsilon e) => e.IsEqual(a, b);

        /// <summary>
        /// The less or equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool LessOrEqual(this double a, double b, Epsilon e) => e.IsEqual(a, b) || (a < b);

        /// <summary>
        /// The greater or equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GreaterOrEqual(this double a, double b, Epsilon e) => e.IsEqual(a, b) || (a > b);

        /// <summary>
        /// The not equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NotEqual(this double a, double b, Epsilon e) => e.IsNotEqual(a, b);

        /// <summary>
        /// The less than.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool LessThan(this double a, double b, Epsilon e) => e.IsNotEqual(a, b) && (a < b);

        /// <summary>
        /// The greater than.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GreaterThan(this double a, double b, Epsilon e) => e.IsNotEqual(a, b) && (a > b);

        /// <summary>
        /// The equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Equal(this double a, double b) => EpsilonMax.IsEqual(a, b);

        /// <summary>
        /// The less or equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool LessOrEqual(this double a, double b) => EpsilonMax.IsEqual(a, b) || (a < b);

        /// <summary>
        /// The greater or equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GreaterOrEqual(this double a, double b) => EpsilonMax.IsEqual(a, b) || (a > b);

        /// <summary>
        /// The not equal.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NotEqual(this double a, double b) => EpsilonMax.IsNotEqual(a, b);

        /// <summary>
        /// The less than.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool LessThan(this double a, double b) => EpsilonMax.IsNotEqual(a, b) && (a < b);

        /// <summary>
        /// The greater than.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GreaterThan(this double a, double b) => EpsilonMax.IsNotEqual(a, b) && (a > b);

        /// <summary>
        /// The epsilon.
        /// </summary>
        public struct Epsilon
        {
            /// <summary>
            /// The _value.
            /// </summary>
            private readonly double value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Epsilon"/> struct.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public Epsilon(double value)
            {
                this.value = value;
            }

            /// <summary>
            /// The is equal.
            /// </summary>
            /// <param name="a">
            /// The a.
            /// </param>
            /// <param name="b">
            /// The b.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            internal bool IsEqual(double a, double b) => (a == b) || (Math.Abs(a - b) < this.value);

            /// <summary>
            /// The is not equal.
            /// </summary>
            /// <param name="a">
            /// The a.
            /// </param>
            /// <param name="b">
            /// The b.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            internal bool IsNotEqual(double a, double b) => (a != b) && !(Math.Abs(a - b) < this.value);
        }
    }
}