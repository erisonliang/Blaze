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
namespace Prompt
{
    public class PromptCommand
    {
        #region Properties
        private string _name;
        private string _path;
        private string _arguments;
        private static string _arguments_token = "$$";
        #endregion

        #region Accessors
        public string Name { get { return _name; } set { _name = value; } }
        public string Path { get { return _path; } set { _path = value; } }
        public string Arguments { get { return _arguments; } set { _arguments = value; } }
        public static string ArgumentsToken { get { return _arguments_token; } }
        #endregion

        #region Constructors
        public PromptCommand(string name, string path, string arguments)
        {
            _name = name;
            _path = path;
            _arguments = arguments;
        }

        public PromptCommand(PromptCommand pcommand)
        {
            _name = pcommand.Name;
            _path = pcommand.Path;
            _arguments = pcommand.Arguments;
        }
        #endregion

        #region Public Methods
        public string GetArguments(string input)
        {
            if (_arguments == string.Empty)
                return input;
            else if (_arguments.Contains(_arguments_token))
            {
                try
                {
                    return _arguments.Replace(_arguments_token, input);
                }
                catch
                {
                    return input;
                }
            }
            else
            {
                return _arguments;
            }
        }
        #endregion
    }
}
