using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BinanceDex.Api.Models;
using BinanceDex.Utilities.Extensions;

namespace BinanceDex.Api.RequestOptions
{
    public class OptionsBase
    {

        public override string ToString()
        {
            IEnumerable<string> result = this.GetType()
                .GetProperties()
                .Select(x => new
                {
                    x.Name,
                    Value = x.GetValue(this),
                })
                .Where(x => x.Value != null)
                .Select(x => new
                {
                    x.Name,
                    Value = x.Value.GetType()
                                .GetMember(x.Value.ToString())
                                .FirstOrDefault()?
                                .GetCustomAttribute<Descriptor>()
                                .Identifier 
                            ?? 
                            x.Value
                })
                .Select(x => $"{x.Name.ToCamelCase()}={x.Value}");

            string joined = string.Join("&", result);

            return joined == "" ? "" : $"?{joined}";
        }
    }
}