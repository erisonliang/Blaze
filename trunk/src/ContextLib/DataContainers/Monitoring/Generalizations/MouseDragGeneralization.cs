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
using System;
using System.Collections.Generic;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class MouseDragGeneralization : Generalization
    {
        #region Properties
        private int _avg_xi;
        private int _avg_yi;
        private int _avg_xf;
        private int _avg_yf;
        #endregion

        #region Accessors
        public int AverageXi { get { return _avg_xi; } set { _avg_xi = value; } }
        public int AverageYi { get { return _avg_yi; } set { _avg_yi = value; } }
        public int AverageXf { get { return _avg_xf; } set { _avg_xf = value; } }
        public int AverageYf { get { return _avg_yf; } set { _avg_yf = value; } }
        #endregion

        #region Constructors
        public MouseDragGeneralization(int avg_xi, int avg_yi, int avg_xf, int avg_yf, TimeSpan time, int occurrences)
            : base(GeneralizationType.MouseDragGeneralization, time, occurrences)
        {
            _avg_xi = avg_xi;
            _avg_yi = avg_yi;
            _avg_xf = avg_xf;
            _avg_yf = avg_yf;
        }
        #endregion

        #region Public Methods
        public static Generalization[] Generate(int xi1, int yi1, int xi2, int yi2, int xf1, int yf1, int xf2, int yf2, TimeSpan time)
        {
            List<Generalization> gens = new List<Generalization>();

            if (xi2 <= (int)(xi1 + MouseAction.MAX_X_OFFSET * 2) &&
                xi2 >= (int)(xi1 - MouseAction.MAX_X_OFFSET * 2) &&
                yi2 <= (int)(yi1 + MouseAction.MAX_Y_OFFSET * 2) &&
                yi2 >= (int)(yi1 - MouseAction.MAX_Y_OFFSET * 2) &&
                xf2 <= (int)(xf1 + MouseAction.MAX_X_OFFSET * 2) &&
                xf2 >= (int)(xf1 - MouseAction.MAX_X_OFFSET * 2) &&
                yf2 <= (int)(yf1 + MouseAction.MAX_Y_OFFSET * 2) &&
                yf2 >= (int)(yf1 - MouseAction.MAX_Y_OFFSET * 2))
            {

                int avg_xi = GetIntAverage(xi1, xi2);
                int avg_yi = GetIntAverage(yi1, yi2);
                int avg_xf = GetIntAverage(xf1, xf2);
                int avg_yf = GetIntAverage(yf1, yf2);

                gens.Add(new MouseDragGeneralization(avg_xi, avg_yi, avg_xf, avg_yf, time, 2));
            }

            return gens.ToArray();
        }

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();

            if (prev_gens.Length > 0 && prev_gens.Length == new_gens.Length && prev_gens[0].Type == new_gens[0].Type)
            {
                for (int i = 0; i < prev_gens.Length; i++)
                {
                    MouseDragGeneralization prev_gen = (MouseDragGeneralization)prev_gens[i];
                    MouseDragGeneralization new_gen = (MouseDragGeneralization)new_gens[i];
                    int xi1 = prev_gen.AverageXi, xi2 = new_gen.AverageXi, yi1 = prev_gen.AverageYi, yi2 = new_gen.AverageYi;
                    int xf1 = prev_gen.AverageXf, xf2 = new_gen.AverageXf, yf1 = prev_gen.AverageYf, yf2 = new_gen.AverageYf;
                    if (xi2 <= (int)(xi1 + MouseAction.MAX_X_OFFSET * 2) &&
                        xi2 >= (int)(xi1 - MouseAction.MAX_X_OFFSET * 2) &&
                        yi2 <= (int)(yi1 + MouseAction.MAX_Y_OFFSET * 2) &&
                        yi2 >= (int)(yi1 - MouseAction.MAX_Y_OFFSET * 2) &&
                        xf2 <= (int)(xf1 + MouseAction.MAX_X_OFFSET * 2) &&
                        xf2 >= (int)(xf1 - MouseAction.MAX_X_OFFSET * 2) &&
                        yf2 <= (int)(yf1 + MouseAction.MAX_Y_OFFSET * 2) &&
                        yf2 >= (int)(yf1 - MouseAction.MAX_Y_OFFSET * 2))
                    {
                        gens.Add(new MouseDragGeneralization(GetIntAverage(xi1, xi2), GetIntAverage(yi1, yi2), GetIntAverage(xf1, xf2), GetIntAverage(yf1, yf2), new_gen.Time, prev_gen.Occurrences + 1));
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

            MouseDragGeneralization generalization = (MouseDragGeneralization)obj;
            if (generalization == null) // check if it can be casted
                return false;

            if (generalization.AverageXi == this.AverageXi &&
                generalization.AverageYi == this.AverageYi)
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
            return new MouseDragGeneralization(this.AverageXi, this.AverageYi, this.AverageXf, this.AverageYf, this.Time, this.Occurrences);
        }

        public override string ToString()
        {
            return "repeat : from (X = " + _avg_xi.ToString() + ", Y = " + _avg_yi.ToString() + ") to (X = " + _avg_xf.ToString() + ", Y = " + _avg_yf.ToString() + ")";
        }
        #endregion

        #region Private Methods
        private static int GetIntAverage(int a, int b)
        {
            return (int)(((double)(a + b)) / (double)2);
        }
        #endregion
    }
}
