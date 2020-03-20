using System;
using System.Collections.Generic;
using System.Text;

namespace litewsTest
{
    public class testmodel
    {
        public int? length { get => data?.Length; }
        public int[] data { get; set; }
        public string status { get; set; } = "ok";
    }
}
