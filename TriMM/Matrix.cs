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
using System.Linq;
using System.Text;

namespace TriMM {
    [Serializable()]

    /// <summary>
    /// A class representing a matrix offering matrix-operations.
    /// </summary>
    public class Matrix {

        #region Fields

        private double[,] matrix;

        #endregion

        #region Properties

        /// <value>Gets the number of rows.</value>
        public int Rows { get { return matrix.GetLength(0); } }

        /// <value>Gets the number of columns.</value>
        public int Columns { get { return matrix.GetLength(1); } }

        /// <value>Gets an element of the matrix or sets it.</value>
        public double this[int row, int column] { get { return matrix[row, column]; } set { matrix[row, column] = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a matrix with <paramref name="rows"/> rows and <paramref name="columns"/> columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(int rows, int columns) { matrix = new double[rows, columns]; }

        /// <summary>
        /// Creates a matrix with <paramref name="rows"/> rows and <paramref name="columns"/> columns
        /// and the value <paramref name="diag"/> in the main diagonal.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="diag">Value for the main diagonal.</param>
        public Matrix(int rows, int columns, double diag) {
            matrix = new double[rows, columns];
            for (int i = 0; i < rows; i++) { matrix[i, i] = 0; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the row of the chosen index as a Vector.
        /// </summary>
        /// <param name="index">Index of the row.</param>
        /// <returns>The row as a Vector.</returns>
        public Vector GetRow(int index) {
            Vector row = new Vector();
            for (int i = 0; i < Columns; i++) { row.Add(matrix[index, i]); }
            return row;
        }

        /// <summary>
        /// Gets the column of the chosen index as a Vector.
        /// </summary>
        /// <param name="index">Index of the column.</param>
        /// <returns>The column as a Vector.</returns>
        public Vector GetColumn(int index) {
            Vector column = new Vector();
            for (int i = 0; i < Rows; i++) { column.Add(matrix[i, index]); }
            return column;
        }

        #region Operators

        /// <summary>
        /// Multiplication of two Matrices.
        /// </summary>
        /// <param name="M">The first Matrix.</param>
        /// <param name="N">The second Matrix.</param>
        /// <returns>Product of the Matrices.</returns>
        public static Matrix operator *(Matrix M, Matrix N) {
            if (M.Columns != N.Rows) { throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("MatrixDimensionError")[0].InnerText); }
            Matrix result = new Matrix(M.Rows, N.Columns);

            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Columns; j++) {
                    result[i, j] = M.GetRow(i).Dot(N.GetColumn(j));
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplication of a Matrix and a Vector.
        /// </summary>
        /// <param name="M">The Matrix.</param>
        /// <param name="v">The Vector.</param>
        /// <returns>Product of the Matrix and the Vector.</returns>
        public static Vector operator *(Matrix M, Vector v) {
            if (M.Columns != v.Count) { throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("MatrixDimensionError")[0].InnerText); }
            Vector result = new Vector();

            for (int i = 0; i < M.Rows; i++) { result.Add(M.GetRow(i).Dot(v)); }

            return result;
        }

        /// <summary>
        /// Multiplication of a Vector and a Matrix.
        /// </summary>
        /// <param name="v">The Vector.</param>
        /// <param name="M">The Matrix.</param>
        /// <returns>Product of the Vector and the Matrix.</returns>
        public static Vector operator *(Vector v, Matrix M) {
            if (M.Rows != v.Count) { throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("MatrixDimensionError")[0].InnerText); }
            Vector result = new Vector();

            for (int i = 0; i < M.Columns; i++) { result.Add(M.GetColumn(i).Dot(v)); }

            return result;
        }

        /// <summary>
        /// Adds two Matrices.
        /// </summary>
        /// <param name="M">The first Matrix.</param>
        /// <param name="N">The second Matrix.</param>
        /// <returns>Sum of the Matrices.</returns>
        public static Matrix operator +(Matrix M, Matrix N) {
            if ((M.Rows != N.Rows) || (M.Columns != N.Columns)) { throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("MatrixDimensionError")[0].InnerText); }
            Matrix result = new Matrix(M.Rows, M.Columns);

            for (int i = 0; i < M.Rows; i++) {
                for (int j = 0; j < M.Columns; j++) {
                    result[i, j] = M[i, j] + N[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the difference of two Matrices.
        /// </summary>
        /// <param name="M">The first Matrix.</param>
        /// <param name="N">The second Matrix.</param>
        /// <returns>Difference of the Matrices.</returns>
        public static Matrix operator -(Matrix M, Matrix N) {
            if ((M.Rows != N.Rows) || (M.Columns != N.Columns)) { throw new ArgumentException(TriMMApp.Lang.GetElementsByTagName("MatrixDimensionError")[0].InnerText); }
            Matrix result = new Matrix(M.Rows, M.Columns);

            for (int i = 0; i < M.Rows; i++) {
                for (int j = 0; j < M.Columns; j++) {
                    result[i, j] = M[i, j] - N[i, j];
                }
            }

            return result;
        }

        #endregion

        #endregion

    }
}
