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

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public abstract class Generalization
    {
        #region Properties
        private GeneralizationType _type;
        private TimeSpan _time;
        private int _occurrences;
        #endregion

        #region Public Enums
        public enum GeneralizationType : int
        {
            KeyGeneralization = 0x0001,
            MouseGeneralization = 0x002,
            MouseDragGeneralization = 0x004,
            TextGeneralization = 0x0008,
            FileCreateGeneralization = 0x0010,
            FileDeleteGeneralization = 0x0020,
            FileRenameGeneralization = 0x0040,
            FileMoveGeneralization = 0x0080
        }
        #endregion

        #region Accessors
        public GeneralizationType Type { get { return _type; } }
        public TimeSpan Time { get { return _time; } }
        public int Occurrences { get { return _occurrences; } }
        #endregion

        #region Constructors
        public Generalization(GeneralizationType type, TimeSpan time, int occurrences)
        {
            _type = type;
            _time = time;
            _occurrences = occurrences;
        }
        #endregion

        #region Public Methods
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
        public abstract override string ToString();
        public abstract object Clone();
        #endregion
    }
}
