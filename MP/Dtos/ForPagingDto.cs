using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Dtos
{
    public class ForPagingDto<T>
    {
        public int NowPage{get;set;}
        public int Itemcount{get;set;}
        public IEnumerable<T> Items { get; set; } 
        public int Pageitem{
            get
            {
                return 15;
            }
        }
        public int MaxPage {get;set;}
        public ForPagingDto()
        {
            this.NowPage = 1;
        }
        public ForPagingDto(int Page)
        {
            this.NowPage = Page;
        }
        public void SetRightPage()
        {
            if(this.NowPage <= 0)
            {
                this.NowPage =1 ;
            }
            else if(this.NowPage > this.MaxPage)
            {
                this.NowPage = this.MaxPage ;
            }
            if(this.MaxPage == 0)
            {
                this.NowPage =1 ;
            }
        }
    }
}