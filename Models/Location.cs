namespace Flep.Models
{
    using System.Diagnostics.CodeAnalysis;

    public class Location
    {
        public Location(string type, params double[] coordinates)
        {
            this.type = type;
            this.coordinates = coordinates;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Mongo serializable type")]
        public string type { get; set; }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Mongo serializable type")]
        public double[] coordinates { get; set; }
    }
}
