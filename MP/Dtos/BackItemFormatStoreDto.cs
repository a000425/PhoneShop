using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class BackItemFormatStoreDto
    {
        public string Space {get;set;}
        public string Color {get;set;}
        public int Store {get;set;}
        public int ItemPrice {get;set;}
        public int FormatId {get;set;}
        public List<BackItemFormatStoreDto> info{get;set;}
        
    }
}