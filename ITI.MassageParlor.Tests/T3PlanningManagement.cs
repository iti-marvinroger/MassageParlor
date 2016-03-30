using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.MassageParlor.Tests
{
    [TestFixture]
    public class T3PlanningManagement
    {
        static readonly TimeSpan oneHour = TimeSpan.FromHours( 1 );
        static readonly TimeSpan oneHourAndHalf = TimeSpan.FromMinutes( 90 );
        static readonly TimeSpan twoHours = TimeSpan.FromHours( 2 );
        static readonly TimeSpan fiveMinutes = TimeSpan.FromMinutes( 5 );
        static readonly TimeSpan halfAnHour = TimeSpan.FromMinutes( 30 );

        [Test]
        public void t1_adding_an_appointment()
        {
            MassageCompany company = new MassageCompany();
            Client c = company.Clients.CreateClient( "C" );
            Masseur m = company.Masseurs.FindOrCreateMasseur( "Jürgen" );
            DateTime now = DateTime.UtcNow;
            Appointment a = company.Planning.AddAppointment( m, c, now, TimeSpan.FromMinutes( 30 ) );
            Assert.That( a.Client, Is.SameAs( c ) );
            Assert.That( a.Masseur, Is.SameAs( m ) );
            Assert.That( a.Start, Is.EqualTo( now ) );
            Assert.That( a.Duration, Is.EqualTo( TimeSpan.FromMinutes( 30 ) ) );
            Assert.That( a.Stop, Is.EqualTo( now + TimeSpan.FromMinutes( 30 ) ) );
        }

        [Test]
        public void t2_an_appointment_can_start_at_the_exact_stop_time_of_the_previous_one()
        {
            MassageCompany company = new MassageCompany();
            Client c = company.Clients.CreateClient( "C" );
            Masseur m = company.Masseurs.FindOrCreateMasseur( "Jürgen" );
            DateTime now = DateTime.UtcNow;
            Appointment a1 = company.Planning.AddAppointment( m, c, now, halfAnHour );
            Appointment a2 = company.Planning.AddAppointment( m, c, now + halfAnHour, halfAnHour );
            Assert.That( a1 != null && a1.Stop == now + halfAnHour );
            Assert.That( a2 != null && a2.Start == a1.Stop );
        }

        [Test]
        public void t3_an_appointment_for_a_masseur_prevents_other_overlapping_appointments_for_this_masseur()
        {
            MassageCompany company = new MassageCompany();
            Client c1 = company.Clients.CreateClient( "C1" );
            Client c2 = company.Clients.CreateClient( "C2" );
            Masseur m = company.Masseurs.FindOrCreateMasseur( "Jürgen" );
            DateTime start = new DateTime( 2016, 11, 03, 14, 0, 0 );
            DateTime stop = start + oneHour;
            DateTime oneHourBefore = start - oneHour;
            DateTime oneHourAfter = stop + oneHour;
            DateTime middle = start + halfAnHour;

            Appointment a = company.Planning.AddAppointment( m, c1, start, oneHour );

            // Another Client (c2) can not book the masseur m:
            Assert.That( company.Planning.CanAddAppointment( m, c2, start, oneHour ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, start, fiveMinutes ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, start, twoHours ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, oneHourBefore, twoHours ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, oneHourBefore, oneHourAndHalf ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, middle, fiveMinutes ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m, c2, middle, oneHour ), Is.False );

            // But it works if range does not intersects:
            Assert.That( company.Planning.CanAddAppointment( m, c2, oneHourBefore, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m, c2, oneHourBefore, oneHour ) );
            Assert.That( company.Planning.CanAddAppointment( m, c2, oneHourAfter, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m, c2, stop, fiveMinutes ) );

            // And it works for another masseur:
            Masseur m2 = company.Masseurs.FindOrCreateMasseur( "Lola" );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, oneHour ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, twoHours ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, oneHourBefore, twoHours ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, oneHourBefore, oneHourAndHalf ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, middle, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, middle, oneHour ) );
        }

        [Test]
        public void t4_an_appointment_for_a_client_prevents_other_overlapping_appointments_for_this_client()
        {
            MassageCompany company = new MassageCompany();
            Client c = company.Clients.CreateClient( "C" );
            Masseur m1 = company.Masseurs.FindOrCreateMasseur( "Jürgen" );
            Masseur m2 = company.Masseurs.FindOrCreateMasseur( "Lola" );
            DateTime start = new DateTime( 2016, 11, 03, 14, 0, 0 );
            DateTime stop = start + oneHour;
            DateTime oneHourBefore = start - oneHour;
            DateTime oneHourAfter = stop + oneHour;
            DateTime middle = start + halfAnHour;

            Appointment a = company.Planning.AddAppointment( m1, c, start, oneHour );

            // The client (c) can not book an appointment for another masseur (m2):
            Assert.That( company.Planning.CanAddAppointment( m2, c, start, oneHour ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, start, fiveMinutes ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, start, twoHours ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, oneHourBefore, twoHours ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, oneHourBefore, oneHourAndHalf ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, middle, fiveMinutes ), Is.False );
            Assert.That( company.Planning.CanAddAppointment( m2, c, middle, oneHour ), Is.False );

            // But client (c) can book another masseur if range does not intersects:
            Assert.That( company.Planning.CanAddAppointment( m2, c, oneHourBefore, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c, oneHourBefore, oneHour ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c, oneHourAfter, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c, stop, fiveMinutes ) );

            // Of course, another client (c2) can book another masseur (while m1 is with c):
            Client c2 = company.Clients.CreateClient( "C2" );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, oneHour ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, start, twoHours ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, oneHourBefore, twoHours ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, oneHourBefore, oneHourAndHalf ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, middle, fiveMinutes ) );
            Assert.That( company.Planning.CanAddAppointment( m2, c2, middle, oneHour ) );
        }

        [Test]
        public void t5_when_CanAddAppointment_returns_false_AddAppointment_throws_an_InvalidOperationException()
        {
            MassageCompany company = new MassageCompany();
            Client c1 = company.Clients.CreateClient( "C1" );
            Client c2 = company.Clients.CreateClient( "C2" );
            Masseur m1 = company.Masseurs.FindOrCreateMasseur( "Jürgen" );
            Masseur m2 = company.Masseurs.FindOrCreateMasseur( "Lola" );

            Random r = new Random();
            DateTime baseTime = DateTime.UtcNow;
            int nbClash = 0;
            while( nbClash < 10 )
            {
                nbClash += FillRandomAppointment( company, r, m1, c1, baseTime, fiveMinutes, twoHours, 4 );
                nbClash += FillRandomAppointment( company, r, m2, c1, baseTime, fiveMinutes, twoHours, 4 );
                nbClash += FillRandomAppointment( company, r, m1, c2, baseTime, fiveMinutes, twoHours, 4 );
                nbClash += FillRandomAppointment( company, r, m2, c2, baseTime, fiveMinutes, twoHours, 4 );
            }
        }

        int FillRandomAppointment( 
            MassageCompany company, 
            Random r, 
            Masseur m, 
            Client c, 
            DateTime baseTime, 
            TimeSpan duration, 
            TimeSpan totalDuration, 
            int count )
        {
            int nbClash = 0;
            int remainder = count;
            while( remainder > 0 && nbClash < count * 2 )
            {
                int width = r.Next( 3 );
                TimeSpan tryDuration = TimeSpan.FromTicks( duration.Ticks * width );
                DateTime tryStart = baseTime.AddMinutes( r.NextDouble() * (totalDuration - tryDuration).TotalMinutes );
                if( company.Planning.CanAddAppointment( m, c, tryStart, tryDuration ) )
                {
                    Assert.DoesNotThrow( () => company.Planning.AddAppointment( m, c, tryStart, tryDuration ) );
                    remainder--;
                }
                else
                {
                    ++nbClash;
                    Assert.Throws<InvalidOperationException>( () => company.Planning.AddAppointment( m, c, tryStart, tryDuration ) );
                }
            }
            return nbClash;
        }

        [Test]
        public void t6_getting_the_list_of_appointments()
        {
            MassageCompany company = new MassageCompany();
            Client c1 = company.Clients.CreateClient( "C1" );
            Client c2 = company.Clients.CreateClient( "C2" );
            Masseur m1 = company.Masseurs.FindOrCreateMasseur( "M1" );
            Masseur m2 = company.Masseurs.FindOrCreateMasseur( "M2" );
            DateTime d = new DateTime( 2016, 3, 30 );

            int i = 0;
            Appointment[] a = new Appointment[6];

            a[i++] = company.Planning.AddAppointment( m1, c1, d, oneHour );
            a[i++] = company.Planning.AddAppointment( m1, c1, d + oneHourAndHalf, fiveMinutes );
            a[i++] = company.Planning.AddAppointment( m1, c1, d + twoHours, oneHour );
            a[i++] = company.Planning.AddAppointment( m2, c2, d, oneHour );

            List<Appointment> all = company.Planning.GetAllAppointments( d, TimeSpan.FromDays( 1 ) );
            Assert.That( all.Count, Is.EqualTo( 4 ) );
            Assert.That( all.Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 + 60 ) );

            a[i++] = company.Planning.AddAppointment( m2, c2, d + oneHourAndHalf, fiveMinutes );
            a[i++] = company.Planning.AddAppointment( m2, c2, d + twoHours, oneHour );

            all = company.Planning.GetAllAppointments( d, TimeSpan.FromDays( 1 ) );
            Assert.That( all.Count, Is.EqualTo( 6 ) );
            Assert.That( all.Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 2 * (60 + 5 + 60) ) );
            Assert.That( all.Where( x => x.Masseur == m1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Masseur == m2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );

            all = company.Planning.GetAllAppointments( d + fiveMinutes, TimeSpan.FromDays( 1 ) );
            Assert.That( all.Count, Is.EqualTo( 6 ) );
            Assert.That( all.Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 2 * (60 + 5 + 60) ) );
            Assert.That( all.Where( x => x.Masseur == m1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Masseur == m2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );

            all = company.Planning.GetAllAppointments( d - fiveMinutes, TimeSpan.FromDays( 1 ) );
            Assert.That( all.Count, Is.EqualTo( 6 ) );
            Assert.That( all.Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 2 * (60 + 5 + 60) ) );
            Assert.That( all.Where( x => x.Masseur == m1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Masseur == m2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c1 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );
            Assert.That( all.Where( x => x.Client == c2 ).Select( x => x.Duration.TotalMinutes ).Sum(), Is.EqualTo( 60 + 5 + 60 ) );

        }


    }
}
