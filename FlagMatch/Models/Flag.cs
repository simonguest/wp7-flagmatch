using System;
using System.Collections.Generic;
using System.Linq;

namespace FlagMatch.Models
{
    public class Flag
    {
        private String _name;

        private static List<String> flags
        {
            get
            {
                List<String> returnedFlags = new List<String>();
                returnedFlags.AddRange(new List<String>() { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan" });
                returnedFlags.AddRange(new List<String>() { "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi" });
                returnedFlags.AddRange(new List<String>() { "Cambodia", "Cameroon", "Canada", "Cape Verde", "Central African Republic", "Chad", "Chile", "China (PRC)", "Colombia", "Comoros", "Congo", "Costa Rica", "Cote d'Ivoire", "Croatia", "Cuba", "Cyprus", "Czech Republic" });
                returnedFlags.AddRange(new List<String>() { "Denmark", "Djibouti", "Dominica", "Dominican Republic", "DR Congo" });
                returnedFlags.AddRange(new List<String>() { "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia" });
                returnedFlags.AddRange(new List<String>() { "Fed. States of Micronesia", "Fiji", "Finland", "France" });
                returnedFlags.AddRange(new List<String>() { "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea-Bissau", "Guinea", "Guyana" });
                returnedFlags.AddRange(new List<String>() { "Haiti", "Honduras", "Hungary" });
                returnedFlags.AddRange(new List<String>() { "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy" });
                returnedFlags.AddRange(new List<String>() { "Jamaica", "Japan", "Jordan" });
                returnedFlags.AddRange(new List<String>() { "Kazakhstan", "Kenya", "Kuwait", "Kyrgyzstan" });
                returnedFlags.AddRange(new List<String>() { "Laos", "Latvia", "Lebanon", "Leichtenstein", "Lesotho", "Liberia", "Libya", "Lithuania", "Luxembourg" });
                returnedFlags.AddRange(new List<String>() { "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Morocco", "Mozambique", "Myanmar" });
                returnedFlags.AddRange(new List<String>() { "Namibia", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "North Korea", "Norway" });
                returnedFlags.AddRange(new List<String>() { "Oman" });
                returnedFlags.AddRange(new List<String>() { "Pakistan", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico" });
                returnedFlags.AddRange(new List<String>() { "Qatar" });
                returnedFlags.AddRange(new List<String>() { "Romania", "Russia", "Rwanda" });
                returnedFlags.AddRange(new List<String>() { "Saint Kitts and Nevis", "Saint Lucia", "Samoa", "San Marino", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria" });
                returnedFlags.AddRange(new List<String>() { "Tajikistan", "Tanzania", "Thailand", "Togo", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan" });
                returnedFlags.AddRange(new List<String>() { "Uganda", "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan" });
                returnedFlags.AddRange(new List<String>() { "Vanuatu", "Venezuela", "Vietnam" });
                returnedFlags.AddRange(new List<String>() { "Wales" });
                returnedFlags.AddRange(new List<String>() { "Yemen" });
                returnedFlags.AddRange(new List<String>() { "Zambia", "Zimbabwe" });

                return returnedFlags;
            }
        }


        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public static List<Flag> All
        {
            get
            {
                List<Flag> returnedFlags = new List<Flag>();

                foreach (String flag in flags)
                {
                    returnedFlags.Add(new Flag() { Name = flag });
                }

                return returnedFlags;
            }
        }

        public static List<Flag> FourRandom
        {
            get
            {
                List<Flag> returnedFlags = new List<Flag>();

                while (returnedFlags.Count < 4)
                {
                    int r = new Random(DateTime.Now.Millisecond).Next(All.OrderBy(f => f.Name.Count()).Count());
                    if (!(returnedFlags.Select(f => f.Name).Contains(All.ElementAt(r).Name)))
                    {
                        returnedFlags.Add(All.ElementAt(r));
                    }
                }

                return returnedFlags;
            }
        }
    }

}
