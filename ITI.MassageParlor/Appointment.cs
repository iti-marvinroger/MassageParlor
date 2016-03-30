using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{

    /// <summary>
    /// Defines an appointment between a Client and a Masseur.
    /// </summary>
    public class Appointment
    {
        readonly Masseur _masseur;
        readonly Client _client;
        readonly DateTime _start;
        readonly DateTime _stop;

        /// <summary>
        /// This constructor should certainly be modified!
        /// </summary>
        internal Appointment( Masseur m, Client c, DateTime start, TimeSpan duration )
        {
            _masseur = m;
            _client = c;
            _start = start;
            _stop = start.Add( duration );
        }

        /// <summary>
        /// Gets the Client.
        /// This property is never null, even when the client is no more in the <see cref="MassageCompany"/>.
        /// </summary>
        public Client Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Gets the <see cref="Masseur"/>.
        /// </summary>
        public Masseur Masseur
        {
            get { return _masseur; }
        }

        /// <summary>
        /// Gets the start date and time of the meeting.
        /// </summary>
        public DateTime Start
        {
            get { return _start; }
        }

        /// <summary>
        /// Gets the stop date and time of the meeting.
        /// </summary>
        public DateTime Stop
        {
            get { return _stop; }
        }

        /// <summary>
        /// Gets the duration of the meeting.
        /// </summary>
        public TimeSpan Duration
        {
            get { return _stop - _start; }
        }

        /// <summary>
        /// Overridden to ease debugging.
        /// </summary>
        /// <returns>The details of the appointment.</returns>
        public override string ToString()
        {
            return string.Format( "Appointment for {0} with {1} at {2} ({3} minutes).", Masseur.Name, Client.Code, Start, Duration.TotalMinutes );
        }
    }
}
