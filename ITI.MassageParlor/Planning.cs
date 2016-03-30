using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{
    /// <summary>
    /// A planning is composed of all the <see cref="Appointment"/>s' masseurs.
    /// </summary>
    public class Planning
    {
        readonly MassageCompany _company;
        readonly List<Appointment> _appointments;

        internal Planning( MassageCompany c )
        {
            _company = c;
            _appointments = new List<Appointment>();
        }

        /// <summary>
        /// Creates a new <see cref="Appointment"/>. 
        /// This throws an <see cref="InvalidOperationException"/> if the appointment conflicts with any existing appointment
        /// for the masseur or the client.
        /// </summary>
        /// <param name="m">The masseur. Must not be null.</param>
        /// <param name="c">The client. Must not be null.</param>
        /// <param name="start">Start of the appointment.</param>
        /// <param name="duration">Duration of the appointment.</param>
        /// <returns>The new appointment object.</returns>
        public Appointment AddAppointment( Masseur m, Client c, DateTime start, TimeSpan duration )
        {
            if ( !CanAddAppointment( m, c, start, duration ) ) throw new InvalidOperationException( "The appointment must not conflict with existing ones." );

            Appointment appointment = new Appointment( m, c, start, duration );
            _appointments.Add( appointment );
            return appointment;
        }

        /// <summary>
        /// Checks whether an appointment can be created between a masseur and a client at a certain time.
        /// </summary>
        /// <param name="m">The masseur.</param>
        /// <param name="c">The client.</param>
        /// <param name="start">Start of the appointment.</param>
        /// <param name="duration">Duration of the appointment.</param>
        /// <returns>True if there is no conflict, false otherwise.</returns>
        public bool CanAddAppointment( Masseur m, Client c, DateTime start, TimeSpan duration )
        {
            foreach ( Appointment appointment in _appointments )
            {
                if ( appointment.Client == c || appointment.Masseur == m )
                {
                    if ( Intersect( appointment, start, duration ) ) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified masseur has appointments.
        /// </summary>
        /// <param name="m">The masseur.</param>
        /// <returns>True if the masseur has appointments, false otherwise</returns>
        internal bool HasAppointments( Masseur m )
        {
            foreach ( Appointment appointment in _appointments )
            {
                if ( appointment.Masseur == m ) return true;
            }

            return false;
        }


        /// <summary>
        /// Checks whether a given start and duration intersects the specified appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="start">The start.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>True if it intersects, false otherwise</returns>
        internal bool Intersect( Appointment appointment, DateTime start, TimeSpan duration )
        {
            return ( appointment.Start < start.Add( duration ) && start < appointment.Stop );
        }

        /// <summary>
        /// Gets a list filled with the registered <see cref="Appointment"/>s in a period.
        /// If an appointment intersects the <paramref name="start"/>-<paramref name="duration"/> range, it appears
        /// in the returned list.
        /// </summary>
        /// <param name="start">Start of the period.</param>
        /// <param name="duration">Duration of the period.</param>
        /// <returns>A list of appointments (can be empty but nerver null).</returns>
        public List<Appointment> GetAllAppointments( DateTime start, TimeSpan duration )
        {
            List <Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in _appointments)
            {
                if ( Intersect( appointment, start, duration ) ) appointments.Add( appointment );
            }

            return appointments;
        }

    }
}
