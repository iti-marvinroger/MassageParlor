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
    public class T1ClientManagement
    {
        [Test]
        public void t1_creating_clients()
        {
            MassageCompany company = new MassageCompany();
            Client c1 = company.Clients.CreateClient( "XRQ32" );
            Assert.That( c1, Is.Not.Null );
            Assert.That( c1.Code, Is.EqualTo( "XRQ32" ) );

            Client c2 = company.Clients.CreateClient( "C86769" );
            Assert.That( c2, Is.Not.Null );
            Assert.That( c2.Code, Is.EqualTo( "C86769" ) );

            Assert.Throws<ArgumentException>( () => company.Clients.CreateClient( "XRQ32" ) );
            Assert.Throws<ArgumentException>( () => company.Clients.CreateClient( "C86769" ) );
        }

        [Test]
        public void t2_client_code_must_not_be_null_empty_or_whitespace()
        {
            MassageCompany c = new MassageCompany();
            Assert.Throws<ArgumentException>( () => c.Clients.CreateClient( null ) );
            Assert.Throws<ArgumentException>( () => c.Clients.CreateClient( "" ) );
            Assert.Throws<ArgumentException>( () => c.Clients.CreateClient( " " ) );
            Assert.Throws<ArgumentException>( () => c.Clients.CreateClient( " \t\r\n" ) );
        }

        [Test]
        public void t3_clients_can_be_found_by_their_code()
        {
            MassageCompany c = new MassageCompany();
            var codes = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in codes )
            {
                c.Clients.CreateClient( n );
            }
            Assert.That( c.Clients.Count, Is.EqualTo( codes.Length ), "Since codes are different." );
            foreach( var n in codes )
            {
                Client i = c.Clients.FindByCode( n );
                Assert.That( i.Code, Is.EqualTo( n ) );
            }
        }

        [Test]
        public void t4_clients_first_and_last_names_are_never_null()
        {
            MassageCompany company = new MassageCompany();
            var c = company.Clients.CreateClient( "XRQ" );
            Assert.That( c.FirstName, Is.EqualTo( string.Empty ) );
            Assert.That( c.LastName, Is.EqualTo( string.Empty ) );
            c.FirstName = "Max";
            Assert.That( c.FirstName, Is.EqualTo( "Max" ) );
            c.LastName = "Planck";
            Assert.That( c.LastName, Is.EqualTo( "Planck" ) );
            c.FirstName = null;
            Assert.That( c.FirstName, Is.EqualTo( string.Empty ) );
            c.LastName = null;
            Assert.That( c.LastName, Is.EqualTo( string.Empty ) );
        }

        [Test]
        public void t5_client_phone_number_must_be_null_or_betwween_6_and_12_characters_length()
        {
            MassageCompany company = new MassageCompany();
            var c = company.Clients.CreateClient( "C" );
            Assert.That( c.PhoneNumber, Is.Null );
            c.PhoneNumber = "123456";
            Assert.That( c.PhoneNumber, Is.EqualTo( "123456" ) );
            c.PhoneNumber = "1234567";
            Assert.That( c.PhoneNumber, Is.EqualTo( "1234567" ) );
            c.PhoneNumber = "ABCDEFGHIJK";
            Assert.That( c.PhoneNumber, Is.EqualTo( "ABCDEFGHIJK" ) );
            c.PhoneNumber = "ABCDEFGHIJKL";
            Assert.That( c.PhoneNumber, Is.EqualTo( "ABCDEFGHIJKL" ) );

            Assert.Throws<ArgumentException>( () => c.PhoneNumber = Guid.NewGuid().ToString() );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "ABCDEFGHIJKLX" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "12345" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "1234" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "123" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "12" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "1" );
            Assert.Throws<ArgumentException>( () => c.PhoneNumber = "" );
        }


    }
}
