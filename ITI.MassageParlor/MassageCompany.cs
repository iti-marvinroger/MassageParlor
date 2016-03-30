using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{

    /// <summary>
    /// Root object that holds <see cref="Clients"/> and <see cref="Masseurs"/>.
    /// </summary>
    public class MassageCompany
    {
        readonly ClientCollection _clients;
        readonly MasseurCollection _masseurs;
        readonly Planning _planning;

        /// <summary>
        /// Initializes a new empty <see cref="MassageCompany"/> instance.
        /// </summary>
        public MassageCompany()
        {
            _clients = new ClientCollection( this );
            _masseurs = new MasseurCollection( this );
            _planning = new Planning( this );
        }

        /// <summary>
        /// Gets the collection of clients.
        /// </summary>
        public ClientCollection Clients
        {
            get { return _clients; }
        }

        /// <summary>
        /// Gets the collection of masseurs.
        /// </summary>
        public MasseurCollection Masseurs
        {
            get { return _masseurs; }
        }

        /// <summary>
        /// Gets the planning for this company.
        /// </summary>
        public Planning Planning
        {
            get { return _planning; }
        }
    }
}
