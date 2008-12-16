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
    /// A VectorND of arbitrary dimension.
    /// </summary>
    public class VectorND : List<double> {

        #region Properties

        /// <value>Gets the normalized VectorND.</value>
        public VectorND Normalized {
            get {
                this.Normalize();
                return this;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new VectorND of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">Elements of the VectorND</param>
        public VectorND(params double[] values) { this.AddRange(values); }

        /// <summary>
        /// Creates a new VectorND of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">An enumeration of elements of the VectorND</param>
        public VectorND(IEnumerable<double> values) { this.AddRange(values); }

        #endregion

        #region Methods

        #region Operators

        /// <summary>
        /// Normalizes the VectorND to a length of 1. The VectorND (0,0,0) is normalized to (1,0,0).
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
        /// Rounds this VectorND to the given number of decimals.
        /// </summary>
        /// <param name="decimals">Number of decimals to round to.</param>
        public void Round(int decimals) { for (int i = 0; i < this.Count; i++) { this[i] = Math.Round(this[i], decimals); } }

        /// <summary>
        /// Calculates the squared distance of this VectorND from the VectorND given by <paramref name="v2"/>.
        /// </summary>
        /// <param name="v2">Another VectorND</param>
        /// <returns>Squared distance between the VectorNDs</returns>
        public double SquaredDistanceFrom(VectorND v2) { return (this - v2).SquaredLength(); }

        /// <summary>
        /// Calculates the distance of this VectorND from the VectorND given by <paramref name="v2"/>.
        /// </summary>
        /// <param name="v2">Another VectorND</param>
        /// <returns>Distance between the VectorNDs</returns>
        public double DistanceFrom(VectorND v2) { return Math.Sqrt(SquaredDistanceFrom(v2)); }

        /// <summary>
        /// Calculates the square of the VectorNDs length.
        /// </summary>
        /// <returns>Square of the length of the VectorND</returns>
        public double SquaredLength() {
            double result = 0;
            for (int i = 0; i < this.Count; i++) {
                result += this[i] * this[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the VectorNDs length.
        /// </summary>
        /// <returns>Length of the VectorND</returns>
        public double Length() { return Math.Sqrt(SquaredLength()); }

        /// <summary>
        /// Calculate's the dot product (or scalar product) of this VectorND with another VectorND.
        /// </summary>
        /// <param name="v2">The second VectorND for the dot product</param>
        /// <returns>The Dot product</returns>
        public double Dot(VectorND v2) {
            if (this.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length for the dot product!");
            }
            double result = 0;
            for (int i = 0; i < this.Count; i++) {
                result += this[i] * v2[i];
            }
            return result;
        }


        /// <summary>
        /// Calculates the cross product (or vector product) of this VectorND with another VectorND.
        /// Only defined for 3D-Vectors.
        /// </summary>
        /// <param name="v2">The second VectorND for the cross product</param>
        /// <returns>The Cross product</returns>
        public VectorND Cross(VectorND v2) {
            if ((this.Count != v2.Count) || (this.Count != 3)) {
                throw new ArgumentException("The cross product is only defined for 3D-VectorNDs!");
            }
            return new VectorND(this[1] * v2[2] - this[2] * v2[1], this[2] * v2[0] - this[0] * v2[2],
                this[0] * v2[1] - this[1] * v2[0]);
        }

        #region Static

        /// <summary>
        /// Calculates the cosine of the angle between the given VectorNDs, when seen as VectorNDs with origin 0.
        /// </summary>
        /// <param name="v1">The first VectorND.</param>
        /// <param name="v2">The second VectorND.</param>
        /// <returns>The angle between the VectorNDs.</returns>
        public static double AngleCos(VectorND v1, VectorND v2) {
            double angleCos = Dot(v1, v2) / (v1.Length() * v2.Length());
            if (angleCos > 1) { angleCos = 1; } else if (angleCos < -1) { angleCos = -1; }
            return angleCos;
        }

        /// <summary>
        /// Calculates the angle between the given VectorNDs, when seen as VectorNDs with origin 0.
        /// </summary>
        /// <param name="v1">The first VectorND.</param>
        /// <param name="v2">The second VectorND.</param>
        /// <returns>The angle between the VectorNDs.</returns>
        public static double Angle(VectorND v1, VectorND v2) { return Math.Acos(AngleCos(v1, v2)); }

        /// <summary>
        /// Calculates the angle between the given VectorNDs, when seen as VectorNDs with origin 0.
        /// The resulting angle lies between 0 and Pi/2 (Pi = 0).
        /// </summary>
        /// <param name="v1">The first VectorND.</param>
        /// <param name="v2">The second VectorND.</param>
        /// <returns>The angle between the VectorNDs.</returns>
        public static double DirectedAngle(VectorND v1, VectorND v2) {
            double angle = Angle(v1, v2);
            if (angle > Math.PI / 2) { angle = Math.PI - angle; }
            return angle;
        }

        /// <summary>
        /// Calculates the angle in degrees between the given VectorNDs, when seen as VectorNDs with origin (0,0,0).
        /// </summary>
        /// <param name="v1">The first VectorND.</param>
        /// <param name="v2">The second VectorND.</param>
        /// <returns>The angle between the VectorNDs in degrees.</returns>
        public static double DegreeAngle(VectorND v1, VectorND v2) { return Angle(v1, v2) * 180 / Math.PI; }

        /// <summary>
        /// Calculates the squared distance between two VectorNDs.
        /// </summary>
        /// <param name="v1">First VectorND</param>
        /// <param name="v2">Second VectorND</param>
        /// <returns>Squared distance between the VectorNDs</returns>
        public static double SquaredDistance(VectorND v1, VectorND v2) { return (v1 - v2).SquaredLength(); }

        /// <summary>
        /// Calculates the distance between two VectorNDs.
        /// </summary>
        /// <param name="v1">First VectorND</param>
        /// <param name="v2">Second VectorND</param>
        /// <returns>Distance between the VectorNDs</returns>
        public static double Distance(VectorND v1, VectorND v2) { return Math.Sqrt(SquaredDistance(v1, v2)); }

        /// <summary>
        /// Rounds the given VectorND to the amount of decimals given by <paramref name="decimals"/>.
        /// </summary>
        /// <param name="VectorND">The VectorND.</param>
        /// <param name="decimals">The number of decimals to round to</param>
        /// <returns>The rounded VectorND.</returns>
        public static VectorND Round(VectorND v, int decimals) {
            v.Round(decimals);
            return v;
        }

        /// <summary>
        /// Calculate's the dot product (or scalar product) of two VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND for the dot product</param>
        /// <param name="v2">The second VectorND for the dot product</param>
        /// <returns>The Dot product</returns>
        public static double Dot(VectorND v1, VectorND v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length for the dot product!");
            }
            double result = 0;
            for (int i = 0; i < v1.Count; i++) {
                result += v1[i] * v2[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the cross product (or vector product) of two VectorNDs.
        /// Only defined for 3D-VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND for the cross product</param>
        /// <param name="v2">The second VectorND for the cross product</param>
        /// <returns>The Cross product</returns>
        public static VectorND Cross(VectorND v1, VectorND v2) {
            if ((v1.Count != v2.Count) || (v1.Count != 3)) {
                throw new ArgumentException("The cross product is only defined for 3D-VectorNDs!");
            }
            return new VectorND(v1[1] * v2[2] - v1[2] * v2[1], v1[2] * v2[0] - v1[0] * v2[2],
                v1[0] * v2[1] - v1[1] * v2[0]);
        }

        /// <summary>
        /// Calculates the product of the given VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND</param>
        /// <param name="v2">The second VectorND</param>
        /// <returns>The Product</returns>
        public static VectorND operator *(VectorND v1, VectorND v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length!");
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] * v2[i]);
            }
            return new VectorND(values);
        }

        /// <summary>
        /// Calculates the divisor of the given VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND</param>
        /// <param name="v2">The second VectorND</param>
        /// <returns>The Divisor</returns>
        public static VectorND operator /(VectorND v1, VectorND v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length!");
            }
#if !DEBUG
            try {
#endif
                List<double> values = new List<double>();
                for (int i = 0; i < v1.Count; i++) {
                    values.Add(v1[i] / v2[i]);
                }
                return new VectorND(values);
#if !DEBUG
            } catch (System.DivideByZeroException e) {
                throw (new System.DivideByZeroException("VectorND not dividible by zero", e));
            }
#endif
        }

        /// <summary>
        /// Calculates the sum of the given VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND</param>
        /// <param name="v2">The second VectorND</param>
        /// <returns>The Sum</returns>
        public static VectorND operator +(VectorND v1, VectorND v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length!");
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] + v2[i]);
            }
            return new VectorND(values);
        }

        /// <summary>
        /// Calculates the difference of the given VectorNDs.
        /// </summary>
        /// <param name="v1">The first VectorND</param>
        /// <param name="v2">The second VectorND</param>
        /// <returns>The Difference</returns>
        public static VectorND operator -(VectorND v1, VectorND v2) {
            if (v1.Count != v2.Count) {
                throw new ArgumentException("The VectorNDs need to be of equal length!");
            }
            List<double> values = new List<double>();
            for (int i = 0; i < v1.Count; i++) {
                values.Add(v1[i] - v2[i]);
            }
            return new VectorND(values);
        }

        /// <summary>
        /// Calculates the product of a factor and a VectorND.
        /// </summary>
        /// <param name="factor">The factor</param>
        /// <param name="v">The VectorND</param>
        /// <returns>The Product</returns>
        public static VectorND operator *(double factor, VectorND v) {
            List<double> values = new List<double>();
            for (int i = 0; i < v.Count; i++) {
                values.Add(factor * v[i]);
            }
            return new VectorND(values);
        }

        /// <summary>
        /// Calculates the product of a VectorND and a factor.
        /// </summary>
        /// <param name="v">The VectorND</param>
        /// <param name="factor">The factor</param>
        /// <returns>The Product</returns>
        public static VectorND operator *(VectorND v, double factor) {
            return factor * v;
        }

        /// <summary>
        /// Calculates the divisor of a VectorND and a factor.
        /// </summary>
        /// <param name="v">The VectorND</param>
        /// <param name="factor">The factor</param>
        /// <returns>The Divisor</returns>
        public static VectorND operator /(VectorND v, double factor) {
#if !DEBUG
            try {
#endif
                return (1 / factor) * v;
#if !DEBUG
            } catch (System.DivideByZeroException e) {
                throw (new System.DivideByZeroException("VectorND not dividable by zero", e));
            }
#endif
        }

        /// <summary>
        /// Short version of the Cross product.
        /// </summary>
        /// <param name="v1">The first VectorND for the cross product</param>
        /// <param name="v2">The second VectorND for the cross product</param>
        /// <returns>The Cross product</returns>
        public static VectorND operator %(VectorND v1, VectorND v2) { return Cross(v1, v2); }

        /// <summary>
        /// Calculates the additive inverse.
        /// </summary>
        /// <param name="v">The VectorND</param>
        /// <returns>The VectorND times -1</returns>
        public static VectorND operator -(VectorND v) { return -1 * v; }

        #endregion

        #endregion

        /// <summary>
        /// Converts this VectorND to a Vertex.
        /// </summary>
        /// <returns>This VectorND as a Vertex</returns>
        public Vertex ToVertex() { return new Vertex(this); }

        /// <summary>
        /// Returns this VectorND rounded to the given number of decimals.
        /// </summary>
        /// <param name="decimals">Number of decimals to round to</param>
        /// <returns>The rounded VectorND</returns>
        public VectorND Rounded(int decimals) {
            this.Round(decimals);
            return this;
        }

        /// <summary>
        /// Returns a copy of this VectorND.
        /// </summary>
        /// <returns>Copy of this VectorND.</returns>
        public VectorND Copy() { return new VectorND(this); }

        /// <summary>
        /// Returns the VectorND as a string.
        /// </summary>
        /// <returns>VectorND as a string</returns>
        public override string ToString() {
            string result = "[";
            for (int i = 0; i < this.Count; i++) {
                result += this[i].ToString();
                if (i < this.Count - 1) { result += ", "; } else { result += "]"; }
            }
            return result;
        }

        /// <summary>
        /// Compares this VectorND to a given object.
        /// Two VectorNDs are equal, when their values are the same.
        /// </summary>
        /// <param name="obj">An other VectorND</param>
        /// <returns>true, if obj == this</returns>
        public override bool Equals(object obj) {
            if (obj is VectorND) {
                if (this.Count != (obj as VectorND).Count) {
                    return false;
                } else {
                    for (int i = 0; i < this.Count; i++) { if (this[i] != (obj as VectorND)[i]) { return false; } }
                    return true;
                }
            } else { return base.Equals(obj); }
        }

        /// <summary>
        /// Just so that Visual Studio doesn't complain.
        /// </summary>
        /// <returns>base.GetHashCode()</returns>
        public override int GetHashCode() { return base.GetHashCode(); }

        #endregion

    }
}
