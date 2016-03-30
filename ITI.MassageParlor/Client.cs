using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{
    /// <summary>
    /// Simple model of a client: first and last name and a phone number.
    /// </summary>
    public class Client
    {
        readonly string _code;
        string _firstName;
        string _lastName;
        string _phoneNumber;
        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal Client( string code )
        {
            _code = code;
            _firstName = string.Empty;
            _lastName = string.Empty;
        }

        /// <summary>
        /// Gets the client's code. Never null nor empty and always unique in a <see cref="MassageCompany"/>.
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets or sets the first name of the client. Never null (defaults to the empty string).
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if ( value == null ) value = string.Empty;
                _firstName = value;
            }
        }

        /// <summary>
        /// Gets or sets the last name of the client. Never null (defaults to the empty string).
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if ( value == null ) value = string.Empty;
                _lastName = value;
            }
        }

        /// <summary>
        /// Gets or set the phone number. It is null or a string of at 
        /// least 6 characters and at most 12 characters (otherwise a <see cref="ArgumentException"/> is thrown).
        /// </summary>
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if ( value != null && (value.Length < 6 || value.Length > 12) ) throw new ArgumentException( "The phone number must be between 6 and 12 characters.", nameof(value) );

                _phoneNumber = value;
            }
        }

    }
}
