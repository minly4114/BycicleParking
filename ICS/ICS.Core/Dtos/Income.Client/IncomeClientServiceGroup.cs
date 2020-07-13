using System;
using System.Collections.Generic;
using System.Text;

namespace ICS.Core.Dtos.Income.Client
{
    public class IncomeClientServiceGroup
    {
        public string UuidCreator { get; set; }
        public string Name { get; set; }
        public List<string> ClientCardNumbers { get; set; }
    }
}
