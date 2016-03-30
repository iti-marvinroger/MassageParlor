using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.MassageParlor
{

    /// <summary>
    /// A masseur works for a <see cref="Company"/>.
    /// </summary>
    public class Masseur
    {
        MassageCompany _company;
        readonly string _name;
        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal Masseur( MassageCompany context, string name )
        {
            _company = context;
            _name = name;
        }

        internal void BeFired()
        {
            _company = null;
        }

        /// <summary>
        /// Gets the name of the masseur.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the company to which this collection of masseurs belongs.
        /// This is null if this masseur does not work anymore for a <see cref="MassageCompany"/>.
        /// </summary>
        public MassageCompany Company { get { return _company; } }

    }
}
