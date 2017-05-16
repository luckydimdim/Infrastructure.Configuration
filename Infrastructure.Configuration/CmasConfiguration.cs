using System;
using System.Collections.Generic;
using System.Text;

namespace Cmas.Infrastructure.Configuration
{ 
    public class CmasConfiguration
    {
        public CmasConfiguration()
        {
            Option1 = "value1_from_ctor";
        }

        public string Option1 { get; set; }
        public int Option2 { get; set; } = 5;
    }
}
