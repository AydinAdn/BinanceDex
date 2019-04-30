using System.Collections.Generic;

namespace BinanceDex.Api.Models
{
    public class MarketPairs : ApiError
    {
        #region Properties

        public IEnumerable<Market> Markets { get; set; }

        #endregion
    }
}