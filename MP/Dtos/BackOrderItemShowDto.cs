using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class BackOrderItemShowDto
    {
        public string ItemName {get;set;}
        public string ItemFormat {get;set;}
        public int ItemPrice {get;set;}
        public int ItemNum {get;set;}
    }
}