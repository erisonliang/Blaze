// Blaze: Automated Desktop Experience
// Copyright (C) 2008,2009  Gabriel Barata
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
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#define CORRECTION

using System;
using System.Collections.Generic;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class MouseGeneralization : Generalization
    {
        #region Properties
        private int _avg_x;
        private int _avg_y;
        private int _avg_inc_x;
        private int _avg_inc_y;
        private bool _is_sequential;
        #endregion

        #region Accessors
        public int AverageX { get { return _avg_x; } set { _avg_x = value; } }
        public int AverageY { get { return _avg_y; } set { _avg_y = value; } }
        public int AverageIncX { get { return _avg_inc_x; } set { _avg_inc_x = value; } }
        public int AverageIncY { get { return _avg_inc_y; } set { _avg_inc_y = value; } }
        public bool IsSequential { get { return _is_sequential; } }
        #endregion

        #region Constructors
        public MouseGeneralization(int avg_x, int avg_y, int avg_inc_x, int avg_inc_y, bool is_sequential, TimeSpan time, int occurrences)
            : base(GeneralizationType.MouseGeneralization, time, occurrences)
        {
            _avg_x = avg_x;
            _avg_y = avg_y;
            _avg_inc_x = avg_inc_x;
            _avg_inc_y = avg_inc_y;
            _is_sequential = is_sequential;
        }
        #endregion

        #region Public Methods
        public static Generalization[] Generate(int x1, int y1, int x2, int y2, TimeSpan time)
        {
            List<Generalization> gens = new List<Generalization>();

            int dx = x2 - x1;
            int dy = y2 - y1;
            int incx = (Math.Abs(dx) <= MouseAction.MAX_X_OFFSET ? 0 : dx);
            int incy = (Math.Abs(dy) <= MouseAction.MAX_Y_OFFSET ? 0 : dy);
#if CORRECTION
            int avg_x = GetIntAverage(x1, x2);
            int avg_y = GetIntAverage(y1, y2);
#else
            int avg_x = GetIntAverage(x1, x2, 1);
            int avg_y = GetIntAverage(y1, y2, 1);
#endif
            bool seq = (incx != 0 || incy != 0 ? true : false);

            if (seq)
                gens.Add(new MouseGeneralization(x1, y1, incx, incy, seq, time, 2));
            else
                gens.Add(new MouseGeneralization(avg_x, avg_y, incx, incy, seq, time, 2));
            return gens.ToArray();
        }

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();

            if (prev_gens.Length > 0 && prev_gens.Length == new_gens.Length && prev_gens[0].Type == new_gens[0].Type)
            {
                for (int i = 0; i < prev_gens.Length; i++)
                {
                    MouseGeneralization prev_gen = (MouseGeneralization)prev_gens[i];
                    MouseGeneralization new_gen = (MouseGeneralization)new_gens[i];
                    int x1 = prev_gen.AverageX, x2 = new_gen.AverageX, y1 = prev_gen.AverageY, y2 = new_gen.AverageY;
                    int incx1 = prev_gen.AverageIncX, incy1 = prev_gen.AverageIncY, incx2 = new_gen.AverageIncX, incy2 = new_gen.AverageIncY;
#if CORRECTION
                    int dx = x2 - x1;
                    int dy = y2 - y1;
#else
                    int dx = (int)(((double)x2 / (double)(prev_gen.Occurrences + 1)) - ((double)x1 * ((double)prev_gen.Occurrences / (double)(prev_gen.Occurrences + 1))));
                    int dy = (int)(((double)y2 / (double)(prev_gen.Occurrences + 1)) - ((double)y1 * ((double)prev_gen.Occurrences / (double)(prev_gen.Occurrences + 1))));
#endif

                    if (prev_gen.IsSequential == new_gen.IsSequential)
                    {
                        if (prev_gen.IsSequential == true)
                        {
#if CORRECTION
                            int dincx = incx2 - incx1;
                            int dincy = incy2 - incy1;
#else
                            int dincx = (int)(((double)incx2 / (double)(prev_gen.Occurrences + 1)) - ((double)incx1 * ((double)prev_gen.Occurrences / (double)(prev_gen.Occurrences + 1))));
                            int dincy = (int)(((double)incy2 / (double)(prev_gen.Occurrences + 1)) - ((double)incy1 * ((double)prev_gen.Occurrences / (double)(prev_gen.Occurrences + 1))));
#endif
                            bool is_dincx_valid = (Math.Abs(dincx) <= MouseAction.MAX_X_OFFSET ? true : false);
                            bool is_dincy_valid = (Math.Abs(dincy) <= MouseAction.MAX_Y_OFFSET ? true : false);

                            if (is_dincx_valid && is_dincy_valid)
                            {
#if CORRECTION
                                int avg_incx = GetIntAverage(incx1, incx2);
                                int avg_incy = GetIntAverage(incy1, incy2);
#else
                                int avg_incx = GetIntAverage(incx1, incx2, prev_gen.Occurrences);
                                int avg_incy = GetIntAverage(incy1, incy2, prev_gen.Occurrences);
#endif
                                bool is_dx_valid = (Math.Abs(dx - avg_incx) <= MouseAction.MAX_X_OFFSET ? true : false);
                                bool is_dy_valid = (Math.Abs(dy - avg_incy) <= MouseAction.MAX_Y_OFFSET ? true : false);

                                if (is_dx_valid && is_dy_valid)
                                {
#if CORRECTION
                                    int avg_x = GetIntAverage(x1, x2);
                                    int avg_y = GetIntAverage(y1, y2);
#else
                                    int avg_x = GetIntAverage(x1, x2, prev_gen.Occurrences);
                                    int avg_y = GetIntAverage(y1, y2, prev_gen.Occurrences);
#endif
                                    gens.Add(new MouseGeneralization(x2, y2, avg_incx, avg_incy, true, new_gen.Time, prev_gen.Occurrences + 1));
                                }
                            }
                        }
                        else
                        {
                            int incx = (Math.Abs(dx) <= MouseAction.MAX_X_OFFSET ? 0 : dx);
                            int incy = (Math.Abs(dy) <= MouseAction.MAX_Y_OFFSET ? 0 : dy);
#if CORRECTION
                            int avg_x = GetIntAverage(x1, x2);
                            int avg_y = GetIntAverage(y1, y2);
#else
                            int avg_x = GetIntAverage(x1, x2, prev_gen.Occurrences);
                            int avg_y = GetIntAverage(y1, y2, prev_gen.Occurrences);
#endif
                            bool seq = (incx != 0 || incy != 0 ? true : false);
                            if (!seq)
                            {
                                gens.Add(new MouseGeneralization(avg_x, avg_y, incx, incy, seq, new_gen.Time, prev_gen.Occurrences + 1));
                            }
                        }
                    }
                }
            }

            return gens.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            MouseGeneralization generalization = (MouseGeneralization)obj;
            if (generalization == null) // check if it can be casted
                return false;

            if (generalization.AverageX == this.AverageX &&
                generalization.AverageY == this.AverageY)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override object Clone()
        {
            return new MouseGeneralization(this.AverageX, this.AverageY, this.AverageIncX, this.AverageIncY, this.IsSequential, this.Time, this.Occurrences);
        }

        public override string ToString()
        {
            if (this._is_sequential)
                return "last: (X = " + (_avg_x + _avg_inc_x).ToString() + ", Y = " + (_avg_y + _avg_inc_y).ToString() + "), with an increment of (X = " + _avg_inc_x.ToString() + ", Y = " + _avg_inc_y.ToString() + ")";
            else
                return "repeat : (X = " + _avg_x.ToString() + ", Y = " + _avg_y.ToString() + ")";
        }
        #endregion

        #region Private Methods
#if CORRECTION
        private static int GetIntAverage(int a, int b)
        {
            return (int)(((double)(a + b)) / (double)2);
        }
#else
        private static int GetIntAverage(int a, int b, int ocurrences_a)
        {
            return (int)(((double)a) * ((double)ocurrences_a / (double)ocurrences_a + 1))
                        + (int)(((double)b) / ((double)ocurrences_a + 1));
            //return (int)(((double)(a + b)) / (double)2);
        }
#endif
        #endregion
    }
}
