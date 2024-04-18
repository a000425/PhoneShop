using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class BackItemStoreDto
    {
        public string ItemName {get;set;}
        public List<BackItemFormatStoreDto> Format {get;set;}
        public DateTime CreateTime {get;set; }
    }
}