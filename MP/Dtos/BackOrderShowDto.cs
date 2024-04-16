using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class BackOrderShowDto
    {
        public int OrderId {get;set;}
        public DateTime OrderTime {get;set;}
        public string Account{get;set;}
        public List<BackOrderItemShowDto> Items {get;set;}
        public int TotalPrice {get;set;}
        public string Address {get;set;}
        public string OrderStatus {get;set;}
        
    }
}