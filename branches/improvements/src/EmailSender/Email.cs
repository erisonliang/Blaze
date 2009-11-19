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
namespace EmailSender
{
    public class Email
    {
        #region Properties
        private string _contact;
        private string _subject;
        private string _body;
        #endregion

        #region Accessors
        public string Contact { get { return _contact; } }
        public string Subject { get { return _subject; } }
        public string Body { get { return _body; } }
        #endregion

        #region Constructors
        public Email()
        {
            _contact = string.Empty;
            _subject = string.Empty;
            _body = string.Empty;
        }

        public Email(string contact)
        {
            _contact = contact;
            _subject = string.Empty;
            _body = string.Empty;
        }

        public Email(string contact, string subject)
        {
            _contact = contact;
            _subject = subject;
            _body = string.Empty;
        }

        public Email(string contact, string subject, string body)
        {
            _contact = contact;
            _subject = subject;
            _body = body;
        }

        public Email(Email email)
        {
            _contact = email.Contact;
            _subject = email.Subject;
            _body = email.Body;
        }
        #endregion
    }
}
