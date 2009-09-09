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
namespace ContextLib.DataContainers.Multimedia
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;
            Pair<T, U> pair = (Pair<T, U>)obj;
            if (pair == null) // check if it can be casted
                return false;

            return this.First.Equals(pair.First) && this.Second.Equals(pair.Second);
        }

        public bool Equals(Pair<T, U> pair)
        {
            if ((object)pair == null)
                return false;
            return this.First.Equals(pair.First) && this.Second.Equals(pair.Second);
        }
    };

}
