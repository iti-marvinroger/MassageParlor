using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{
    /// <summary>
    /// Holds a collection of recipes.
    /// </summary>
    public class MasseurCollection
    {
        readonly MassageCompany _company;
        readonly Dictionary<string, Masseur> _masseurs;

        internal MasseurCollection( MassageCompany context )
        {
            Debug.Assert( context != null );
            _company = context;
            _masseurs = new Dictionary<string, Masseur>();
        }

        /// <summary>
        /// Gets the <see cref="MassageCompany"/> to which this collection of masseurs belongs.
        /// </summary>
        public MassageCompany Company { get { return _company; } }

        /// <summary>
        /// Gets the number of masseurs in this collection.
        /// </summary>
        public int Count { get { return _masseurs.Count; } }
        
        /// <summary>
        /// Creates a new named <see cref="Masseur"/> or find an exisitng one.
        /// </summary>
        /// <param name="name">Name of the masseur.</param>
        /// <returns>A new or existing masseur.</returns>
        public Masseur FindOrCreateMasseur( string name )
        {
            if ( string.IsNullOrWhiteSpace( name ) ) throw new ArgumentException( "The masseur name must not be null or empty", nameof(name) );

            Masseur masseur;
            _masseurs.TryGetValue( name, out masseur );

            if ( masseur != null ) return masseur;

            masseur = new Masseur( _company, name );
            _masseurs.Add( name, masseur );

            return masseur;
        }

        /// <summary>
        /// Finds a masseur by its name or null if not found.
        /// </summary>
        /// <param name="name">Name to find.</param>
        /// <returns>Null or the masseur with the given name.</returns>
        public Masseur FindByName( string name )
        {
            Masseur masseur;
            _masseurs.TryGetValue( name, out masseur );

            return masseur;
        }

        /// <summary>
        /// Fires a masseur: it is removed from this collection and does not work any more for the Company.
        /// This throws an <see cref="ArgumentException"/> if the masseur does not currently belong to this collection.
        /// </summary>
        /// <param name="m">The masseur to fire.</param>
        public void Fire( Masseur m )
        {
            if ( !_masseurs.Values.Contains( m ) ) throw new ArgumentException( "The masseur does not belong to this collection.", nameof(m) );
            if ( _company.Planning.HasAppointments( m ) ) throw new InvalidOperationException( "The masseur has appointements." );

            m.BeFired();
            _masseurs.Remove( m.Name );
        }

    }
}
