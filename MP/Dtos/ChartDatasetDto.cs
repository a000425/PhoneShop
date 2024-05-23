using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class ChartDatasetDto
    {
        public string Label { get; set; }
        public int[] Data { get; set; }
        public int BorderWidth { get; set; }
    }
}