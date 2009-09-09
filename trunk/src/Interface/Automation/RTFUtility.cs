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

namespace Blaze.Automation
{
    static class RTFUtility
    {
        public static string GenerateRTF(string partial_rtf)
        {
            return @"{\rtf1\ansi\ansicpg1252\deff0\deflang2070{\fonttbl{\f0\fnil\fcharset0 Verdana;}}" +
                    @"{\colortbl;\red245\green155\blue155;\red155\green235\blue155;}" + Environment.NewLine +
                    @"\viewkind4\uc1\pard\f0\fs20 " +
                    partial_rtf +
                    @"\par}";
        }
    }
}
