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
using System.Drawing;

namespace SystemCore.Settings
{
    public class InterfaceInfo
    {
        #region Properties
        private int _number_of_suggestions;
        private Point _window_location;
        #endregion

        #region Accessors
        public int NumberOfSuggestions
        {
            get { return _number_of_suggestions; }
            set { _number_of_suggestions = value; }
        }

        public Point WindowLocation
        {
            get { return _window_location; }
            set { _window_location = value; }
        }
        #endregion

        #region Constructors
        public InterfaceInfo(int number_of_suggestions, Point window_location)
        {
            _number_of_suggestions = number_of_suggestions;
            _window_location = window_location;
        }

        public InterfaceInfo(InterfaceInfo info)
        {
            _number_of_suggestions = info.NumberOfSuggestions;
            _window_location = new Point(info.WindowLocation.X, info.WindowLocation.Y);
        }
        #endregion
    }
}
