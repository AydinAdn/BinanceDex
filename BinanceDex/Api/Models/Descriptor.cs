using System;

namespace BinanceDex.Api.Models
{
    public class Descriptor : Attribute
    {
        #region Constructors

        public Descriptor(string identifier)
        {
            this.Identifier = identifier;
        }

        #endregion

        #region Properties

        public string Identifier { get; set; }

        #endregion
    }
}