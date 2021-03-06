﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace ITI.MassageParlor.Tests
{
    [TestFixture]
    public class PublicModelChecker
    {
        [Test]
        public void write_current_public_API_to_console_with_double_quotes()
        {
            Console.WriteLine( GetPublicAPI( typeof( Masseur ).Assembly ).ToString().Replace( "\"", "\"\"" ) );
        }

        [Test]
        public void public_API_is_not_modified()
        {
            var model = XElement.Parse( @"<Assembly Name=""ITI.MassageParlor"">
  <Types>
    <Type Name=""ITI.MassageParlor.Appointment"">
      <Member Type=""Property"" Name=""Client"" />
      <Member Type=""Property"" Name=""Duration"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Client"" />
      <Member Type=""Method"" Name=""get_Duration"" />
      <Member Type=""Method"" Name=""get_Masseur"" />
      <Member Type=""Method"" Name=""get_Start"" />
      <Member Type=""Method"" Name=""get_Stop"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""Masseur"" />
      <Member Type=""Property"" Name=""Start"" />
      <Member Type=""Property"" Name=""Stop"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.Client"">
      <Member Type=""Property"" Name=""Code"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Property"" Name=""FirstName"" />
      <Member Type=""Method"" Name=""get_Code"" />
      <Member Type=""Method"" Name=""get_FirstName"" />
      <Member Type=""Method"" Name=""get_LastName"" />
      <Member Type=""Method"" Name=""get_PhoneNumber"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""LastName"" />
      <Member Type=""Property"" Name=""PhoneNumber"" />
      <Member Type=""Method"" Name=""set_FirstName"" />
      <Member Type=""Method"" Name=""set_LastName"" />
      <Member Type=""Method"" Name=""set_PhoneNumber"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.ClientCollection"">
      <Member Type=""Property"" Name=""Company"" />
      <Member Type=""Property"" Name=""Count"" />
      <Member Type=""Method"" Name=""CreateClient"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""FindByCode"" />
      <Member Type=""Method"" Name=""get_Company"" />
      <Member Type=""Method"" Name=""get_Count"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.MassageCompany"">
      <Member Type=""Constructor"" Name="".ctor"" />
      <Member Type=""Property"" Name=""Clients"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Clients"" />
      <Member Type=""Method"" Name=""get_Masseurs"" />
      <Member Type=""Method"" Name=""get_Planning"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""Masseurs"" />
      <Member Type=""Property"" Name=""Planning"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.Masseur"">
      <Member Type=""Property"" Name=""Company"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Company"" />
      <Member Type=""Method"" Name=""get_Name"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""Name"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.MasseurCollection"">
      <Member Type=""Property"" Name=""Company"" />
      <Member Type=""Property"" Name=""Count"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""FindByName"" />
      <Member Type=""Method"" Name=""FindOrCreateMasseur"" />
      <Member Type=""Method"" Name=""Fire"" />
      <Member Type=""Method"" Name=""get_Company"" />
      <Member Type=""Method"" Name=""get_Count"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.MassageParlor.Planning"">
      <Member Type=""Method"" Name=""AddAppointment"" />
      <Member Type=""Method"" Name=""CanAddAppointment"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""GetAllAppointments"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
  </Types>
</Assembly>
" );
            var current = GetPublicAPI( typeof( Masseur ).Assembly );
            if( !XElement.DeepEquals( model, current ) )
            {
                string m = model.ToString( SaveOptions.DisableFormatting );
                string c = current.ToString( SaveOptions.DisableFormatting );
                Assert.That( c, Is.EqualTo( m ) );
            }
        }

        XElement GetPublicAPI( Assembly a )
        {
            return new XElement( "Assembly",
                                  new XAttribute( "Name", a.GetName().Name ),
                                  new XElement( "Types",
                                                AllNestedTypes( a.GetExportedTypes() )
                                                 .OrderBy( t => t.FullName )
                                                 .Select( t => new XElement( "Type",
                                                                                new XAttribute( "Name", t.FullName ),
                                                                                t.GetMembers()
                                                                                 .OrderBy( m => m.Name )
                                                                                 .Select( m => new XElement( "Member",
                                                                                                              new XAttribute( "Type", m.MemberType ),
                                                                                                              new XAttribute( "Name", m.Name ) ) ) ) ) ) );
        }

        IEnumerable<Type> AllNestedTypes( IEnumerable<Type> types )
        {
            foreach( Type t in types )
            {
                yield return t;
                foreach( Type nestedType in AllNestedTypes( t.GetNestedTypes() ) )
                {
                    yield return nestedType;
                }
            }
        }
    }
}
