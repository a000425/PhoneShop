using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class ChartDataDto
    {
        public string[] Labels{get; set;}
        public ChartDatasetDto[] Datasets{get;set;}
    }
}