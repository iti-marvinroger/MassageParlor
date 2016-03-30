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
    public class T2MasseurManagement
    {
        [Test]
        public void t1_creating_masseurs()
        {
            MassageCompany c = new MassageCompany();
            Masseur r1 = c.Masseurs.FindOrCreateMasseur( "Jürgen" );
            Assert.That( r1 != null );
            Assert.That( r1.Company == c );
            Assert.That( r1.Name == "Jürgen" );

            Masseur r2 = c.Masseurs.FindOrCreateMasseur( "Lila" );
            Assert.That( r2 != null );
            Assert.That( r2.Company == c );
            Assert.That( r2.Name == "Lila" );

            Masseur r3 = c.Masseurs.FindOrCreateMasseur( "Lila" );
            Assert.That( r2 == r3 );

            Masseur r4 = c.Masseurs.FindOrCreateMasseur( "Jürgen" );
            Assert.That( r4 == r1 );
        }

        [Test]
        public void t2_masseur_names_must_be_valid()
        {
            MassageCompany c = new MassageCompany();
            Assert.Throws<ArgumentException>( () => c.Masseurs.FindOrCreateMasseur( null ) );
            Assert.Throws<ArgumentException>( () => c.Masseurs.FindOrCreateMasseur( "" ) );
        }

        [Test]
        public void t3_masseurs_can_be_found_by_their_names()
        {
            MassageCompany c = new MassageCompany();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in names )
            {
                c.Masseurs.FindOrCreateMasseur( n );
            }
            Assert.That( c.Masseurs.Count, Is.EqualTo( names.Length ), "Since names are different." );
            foreach( var n in names )
            {
                Masseur r = c.Masseurs.FindByName( n );
                Assert.That( r.Name, Is.EqualTo( n ) );
            }
        }

        [Test]
        public void t4_masseurs_can_be_fired()
        {
            MassageCompany c = new MassageCompany();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names )
            {
                var m = c.Masseurs.FindOrCreateMasseur( n );
                Assert.That( m.Company == c );
            }

            int count = c.Masseurs.Count;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var n in names )
            {
                var m = c.Masseurs.FindByName( n );
                Assert.That( m, Is.Not.Null );
                c.Masseurs.Fire( m );
                Assert.That( c.Masseurs.FindByName( n ), Is.Null );
                Assert.That( m.Company, Is.Null, "When a masseur is fired, its Company is null." );
                Assert.That( c.Masseurs.Count, Is.EqualTo( --count ) );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }

        [Test]
        public void t5_masseurs_can_be_fired_only_if_there_is_no_appointment_for_them()
        {
            MassageCompany company = new MassageCompany();
            Masseur m = company.Masseurs.FindOrCreateMasseur( "X" );
            Client c = company.Clients.CreateClient( "C" );
            company.Planning.AddAppointment( m, c, DateTime.UtcNow, TimeSpan.FromHours( 1 ) );
            Assert.Throws<InvalidOperationException>( () => company.Masseurs.Fire( m ) );
        }


    }
}
