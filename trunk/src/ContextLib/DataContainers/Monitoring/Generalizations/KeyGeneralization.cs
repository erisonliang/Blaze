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
    public class KeyGeneralization : Generalization
    {
        #region Properties
        private string _last_val;
        #endregion

        #region Accessors
        public string LastValue { get { return _last_val; } }
        #endregion

        #region Constructors
        public KeyGeneralization(string key_description, TimeSpan time, int occurrences)
            : base(GeneralizationType.KeyGeneralization, time, occurrences)
        {
            _last_val = key_description;
        }
        #endregion

        #region Public Methods
        public static Generalization[] Generate(string quick_description_1, string quick_description_2, TimeSpan time)
        {
            List<Generalization> gens = new List<Generalization>();

            if (quick_description_1 == quick_description_2)
                gens.Add(new KeyGeneralization(quick_description_2, time, 2));

            return gens.ToArray();
        }

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();

            if (prev_gens.Length > 0 && prev_gens.Length == new_gens.Length && prev_gens[0].Type == new_gens[0].Type)
            {
                for (int i = 0; i < prev_gens.Length; i++)
                {
                    KeyGeneralization prev_gen = (KeyGeneralization)prev_gens[i];
                    KeyGeneralization new_gen = (KeyGeneralization)new_gens[i];
                    if (prev_gen.LastValue == new_gen.LastValue) // if both have the same value, everything is OK
                    {
                        gens.Add(new KeyGeneralization(new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                    }
                    else // if not, then there is no merging
                    {
                        return new Generalization[0];
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

            KeyGeneralization generalization = (KeyGeneralization)obj;
            if (generalization == null) // check if it can be casted
                return false;

            if (generalization.LastValue == this.LastValue)
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
            return new KeyGeneralization(this.LastValue, this.Time, this.Occurrences);
        }

        public override string ToString()
        {
            return "repeat";
        }
        #endregion
    }
}
