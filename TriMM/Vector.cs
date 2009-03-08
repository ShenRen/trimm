// Part of TriMM, the TriangleMesh Manipulator.
//
// Copyright (C) 2008  Christian Moritz
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>.
//
// For more information and contact details look at TriMMs website:
// http://trimm.sourceforge.net/

using System;
using System.Collections.Generic;


namespace TriMM {
    [Serializable()]

    /// <summary>
    /// A Vector of arbitrary dimension.
    /// </summary>
    public class Vector : List<double> {

        #region Properties

        /// <value>Gets the normalized Vector.</value>
        public Vector Normalized {
            get {
                this.Normalize();
                return this;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vector of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">Elements of the Vector</param>
        public Vector(params double[] values) { this.AddRange(values); }

        /// <summary>
        /// Creates a new Vector of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">An enumeration of elements of the Vector</param>
        public Vector(IEnumerable<double> values) { this.AddRange(values); }

        #endregion

        #region Methods

        #region Operators

        /// <summary>
        /// Normalizes the Vector to a length of 1. The Vector (0,0,0) is normalized to (1,0,0).
        /// </summary>
        public void Normalize() {
            this.Round(14);
            double length = Length();
            if (length != 0) {
                double l = 1 / Length();
                for (int i = 0; i < this.Count; i++) { this[i] *= l; }
            } else {
                this[0] = 1; this[1] = 0; this[2] = 0;
            }
        }

        /// <summary>
        /// Rounds this Vector to the given number of decimals.
        /// </summary>
        /// <param name="decimals">Number of decimals to round to.</param>
        public void Round(int decimals) { for (int i = 0; i < this.Count; i++) { this[i] = Math.Round(this[i], decimals); } }

        /// <summary>
        /// Calculates the squared distance of this Vector from the Vector given by <paramref name="v2"/>.
        /// </summary>
        /// <param name="v2">Another Vector</param>
        /// <returns>Squared distance between the Vectors</returns>
        public double SquaredDistanceFrom(Vector v2) { return (this - v2).SquaredLength(); }

        /// <summary>
        /// Calculates the distance of this Vector from the Vector given by <paramref name="v2"/>.
        /// </summary>
        /// <param name="v2">Another Vector</param>
        /// <returns>Distance between the Vectors</returns>
        public double DistanceFrom(Vector v2) { return Math.Sqrt(SquaredDistanceFrom(v2)); }

        /// <summary>
        /// Calculates the square of the Vectors length.
        /// </summary>
        /// <returns>Square of the length of the Vector</returns>
        public double SquaredLength() {
            double result = 0;
            for (int i = 0; i < this.Count; i++) {
                result += this[i] * this[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the Vectors length.
        /// </summary>
        /// <returns>Length of the Vector</returns>
        public double Length() { return Math.Sqrt(SquaredLength()); }

        /// <summary>
        /// Calculate's the dot product (or scalar product) of this Vector with another Vector.
        /// </summary>
        /// <param name="v2">The second Vector for the dot product</param>
        /// <returns>The Dot product</returns>
        public double Dot(Vector v2) {
            if (this.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("DotProductError")[0].InnerText);
            }
            double result = 0;
            for (int i = 0; i < this.Count; i++) {
                result += this[i] * v2[i];
            }
            return result;
        }


        /// <summary>
        /// Calculates the cross product (or vector product) of this Vector with another Vector.
        /// Only defined for 3D-Vectors.
        /// </summary>
        /// <param name="v2">The second Vector for the cross product</param>
        /// <returns>The Cross product</returns>
        public Vector Cross(Vector v2) {
            if ((this.Count != v2.Count) || (this.Count != 3)) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("CrossProductError")[0].InnerText);
            }
            return new Vector(this[1] * v2[2] - this[2] * v2[1], this[2] * v2[0] - this[0] * v2[2],
                this[0] * v2[1] - this[1] * v2[0]);
        }

        #region Static

        /// <summary>
        /// Calculates the cosine of the angle between the given Vectors, when seen as Vectors with origin 0.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>The angle between the Vectors.</returns>
        public static double AngleCos(Vector v1, Vector v2) {
            double angleCos = Dot(v1, v2) / (v1.Length() * v2.Length());
            if (angleCos > 1) { angleCos = 1; } else if (angleCos < -1) { angleCos = -1; }
            return angleCos;
        }

        /// <summary>
        /// Calculates the angle between the given Vectors, when seen as Vectors with origin 0.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>The angle between the Vectors.</returns>
        public static double Angle(Vector v1, Vector v2) { return Math.Acos(AngleCos(v1, v2)); }

        /// <summary>
        /// Calculates the angle between the given Vectors, when seen as Vectors with origin 0.
        /// The resulting angle lies between 0 and Pi/2 (Pi = 0).
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>The angle between the Vectors.</returns>
        public static double DirectedAngle(Vector v1, Vector v2) {
            double angle = Angle(v1, v2);
            if (angle > Math.PI / 2) { angle = Math.PI - angle; }
            return angle;
        }

        /// <summary>
        /// Calculates the angle in degrees between the given Vectors, when seen as Vectors with origin (0,0,0).
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>The angle between the Vectors in degrees.</returns>
        public static double DegreeAngle(Vector v1, Vector v2) { return Angle(v1, v2) * 180 / Math.PI; }

        /// <summary>
        /// Calculates the squared distance between two Vectors.
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>Squared distance between the Vectors</returns>
        public static double SquaredDistance(Vector v1, Vector v2) { return (v1 - v2).SquaredLength(); }

        /// <summary>
        /// Calculates the distance between two Vectors.
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>Distance between the Vectors</returns>
        public static double Distance(Vector v1, Vector v2) { return Math.Sqrt(SquaredDistance(v1, v2)); }

        /// <summary>
        /// Rounds the given Vector to the amount of decimals given by <paramref name="decimals"/>.
        /// </summary>
        /// <param name="Vector">The Vector.</param>
        /// <param name="decimals">The number of decimals to round to</param>
        /// <returns>The rounded Vector.</returns>
        public static Vector Round(Vector v, int decimals) {
            v.Round(decimals);
            return v;
        }

        /// <summary>
        /// Calculate's the dot product (or scalar product) of two Vectors.
        /// </summary>
        /// <param name="v1">The first Vector for the dot product</param>
        /// <param name="v2">The second Vector for the dot product</param>
        /// <returns>The Dot product</returns>
        public static double Dot(Vector v1, Vector v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("DotProductError")[0].InnerText);
            }
            double result = 0;
            for (int i = 0; i < v1.Count; i++) {
                result += v1[i] * v2[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the cross product (or vector product) of two Vectors.
        /// Only defined for 3D-Vectors.
        /// </summary>
        /// <param name="v1">The first Vector for the cross product</param>
        /// <param name="v2">The second Vector for the cross product</param>
        /// <returns>The Cross product</returns>
        public static Vector Cross(Vector v1, Vector v2) {
            if ((v1.Count != v2.Count) || (v1.Count != 3)) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("CrossProductError")[0].InnerText);
            }
            return new Vector(v1[1] * v2[2] - v1[2] * v2[1], v1[2] * v2[0] - v1[0] * v2[2],
                v1[0] * v2[1] - v1[1] * v2[0]);
        }

        /// <summary>
        /// Calculates the product of the given Vectors.
        /// </summary>
        /// <param name="v1">The first Vector</param>
        /// <param name="v2">The second Vector</param>
        /// <returns>The Product</returns>
        public static Vector operator *(Vector v1, Vector v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("LengthError")[0].InnerText);
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] * v2[i]);
            }
            return new Vector(values);
        }

        /// <summary>
        /// Calculates the divisor of the given Vectors.
        /// </summary>
        /// <param name="v1">The first Vector</param>
        /// <param name="v2">The second Vector</param>
        /// <returns>The Divisor</returns>
        public static Vector operator /(Vector v1, Vector v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("LengthError")[0].InnerText);
            }
#if !DEBUG
            try {
#endif
                List<double> values = new List<double>();
                for (int i = 0; i < v1.Count; i++) {
                    values.Add(v1[i] / v2[i]);
                }
                return new Vector(values);
#if !DEBUG
            } catch (System.DivideByZeroException e) {
                throw (new System.DivideByZeroException(TriMMApp.Lang.GetElementsByTagName("DivisionByZeroError")[0].InnerText, e));
            }
#endif
        }

        /// <summary>
        /// Calculates the sum of the given Vectors.
        /// </summary>
        /// <param name="v1">The first Vector</param>
        /// <param name="v2">The second Vector</param>
        /// <returns>The Sum</returns>
        public static Vector operator +(Vector v1, Vector v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("LengthError")[0].InnerText);
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] + v2[i]);
            }
            return new Vector(values);
        }

        /// <summary>
        /// Calculates the difference of the given Vectors.
        /// </summary>
        /// <param name="v1">The first Vector</param>
        /// <param name="v2">The second Vector</param>
        /// <returns>The Difference</returns>
        public static Vector operator -(Vector v1, Vector v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("LengthError")[0].InnerText);
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] - v2[i]);
            }
            return new Vector(values);
        }

        /// <summary>
        /// Calculates the product of a factor and a Vector.
        /// </summary>
        /// <param name="factor">The factor</param>
        /// <param name="v">The Vector</param>
        /// <returns>The Product</returns>
        public static Vector operator *(double factor, Vector v) {
            List<double> values = new List<double>();
            for (int i = 0; i < v.Count; i++) {
                values.Add(factor * v[i]);
            }
            return new Vector(values);
        }

        /// <summary>
        /// Calculates the product of a Vector and a factor.
        /// </summary>
        /// <param name="v">The Vector</param>
        /// <param name="factor">The factor</param>
        /// <returns>The Product</returns>
        public static Vector operator *(Vector v, double factor) {
            return factor * v;
        }

        /// <summary>
        /// Calculates the divisor of a Vector and a factor.
        /// </summary>
        /// <param name="v">The Vector</param>
        /// <param name="factor">The factor</param>
        /// <returns>The Divisor</returns>
        public static Vector operator /(Vector v, double factor) {
#if !DEBUG
            try {
#endif
                return (1 / factor) * v;
#if !DEBUG
            } catch (System.DivideByZeroException e) {
                throw (new System.DivideByZeroException(TriMMApp.Lang.GetElementsByTagName("DivisionByZeroError")[0].InnerText, e));
            }
#endif
        }

        /// <summary>
        /// Short version of the Cross product.
        /// </summary>
        /// <param name="v1">The first Vector for the cross product</param>
        /// <param name="v2">The second Vector for the cross product</param>
        /// <returns>The Cross product</returns>
        public static Vector operator %(Vector v1, Vector v2) { return Cross(v1, v2); }

        /// <summary>
        /// Calculates the additive inverse.
        /// </summary>
        /// <param name="v">The Vector</param>
        /// <returns>The Vector times -1</returns>
        public static Vector operator -(Vector v) { return -1 * v; }

        #endregion

        #endregion

        /// <summary>
        /// Converts this Vector to a Vertex.
        /// </summary>
        /// <returns>This Vector as a Vertex</returns>
        public Vertex ToVertex() { return new Vertex(this); }

        /// <summary>
        /// Returns this Vector rounded to the given number of decimals.
        /// </summary>
        /// <param name="decimals">Number of decimals to round to</param>
        /// <returns>The rounded Vector</returns>
        public Vector Rounded(int decimals) {
            this.Round(decimals);
            return this;
        }

        /// <summary>
        /// Returns a copy of this Vector.
        /// </summary>
        /// <returns>Copy of this Vector.</returns>
        public Vector Copy() { return new Vector(this); }

        /// <summary>
        /// Returns the Vector as a string.
        /// </summary>
        /// <returns>Vector as a string</returns>
        public override string ToString() {
            string result = "[";
            for (int i = 0; i < this.Count; i++) {
                result += this[i].ToString();
                if (i < this.Count - 1) { result += "; "; } else { result += "]"; }
            }
            return result;
        }

        /// <summary>
        /// Compares this Vector to a given object.
        /// Two Vectors are equal, when their values are the same.
        /// </summary>
        /// <param name="obj">An other Vector</param>
        /// <returns>true, if obj == this</returns>
        public override bool Equals(object obj) {
            if (obj is Vector) {
                if (this.Count != (obj as Vector).Count) {
                    return false;
                } else {
                    for (int i = 0; i < this.Count; i++) { if (this[i] != (obj as Vector)[i]) { return false; } }
                    return true;
                }
            } else { return base.Equals(obj); }
        }

        /// <summary>
        /// Needed for operations like Union.
        /// </summary>
        /// <returns>HashCode of the Vector as a string.</returns>
        public override int GetHashCode() { return this.ToString().GetHashCode(); }

        #endregion

    }
}
