using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{

    /// <summary>
    /// Holds a collection of <see cref="Client"/>.
    /// One can only add new clients to this collection thanks to <see cref="CreateClient"/> and 
    /// a client can not be removed.
    /// </summary>
    public class ClientCollection
    {
        readonly MassageCompany _company;
        readonly Dictionary<string, Client> _clients;

        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal ClientCollection( MassageCompany context )
        {
            _company = context;
            _clients = new Dictionary<string, Client>();
        }

        /// <summary>
        /// Gets the <see cref="MassageCompany"/> to which this collection of clients belongs.
        /// </summary>
        public MassageCompany Company { get { return _company; } }

        /// <summary>
        /// Gets the number of clients in this collection.
        /// </summary>
        public int Count { get { return _clients.Count; } }

        /// <summary>
        /// Creates a new client in this collection.
        /// </summary>
        /// <param name="code">Code of the new client. This code must be unique otherwise an <see cref="ArgumentException"/> is thrown.</param>
        /// <returns>The newly created client.</returns>
        public Client CreateClient( string code )
        {
            if ( string.IsNullOrWhiteSpace( code ) ) throw new ArgumentException( "The lient code must not be null or empty.", nameof(code) );
            if ( _clients.Keys.Contains( code ) ) throw new ArgumentException( "The client code already exists.", nameof(code) );

            Client client = new Client( code );
            _clients.Add( code, client );

            return client;
        }

        /// <summary>
        /// Finds an client by its code. Returns null if it does not exist.
        /// </summary>
        /// <param name="code">Code of the client to find.</param>
        /// <returns>The client or null if not found.</returns>
        public Client FindByCode( string code )
        {
            Client client;
            _clients.TryGetValue( code, out client );

            return client;
        }

    }
}
