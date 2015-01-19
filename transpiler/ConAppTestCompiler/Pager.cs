using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Wrapper.JS;
namespace Utils
{
    public class Pager<T> : jsClass
    {
        List<T> list = null;
        int size = 0;
        int pageNo = 1;
        public Pager(int size)
        {
            this.size = size;
        }
        public void SetSize(int size)
        {
            this.size = size;
            this.pageNo = 1;
        }
        public void SetList(List<T> list)
        {
            this.list = list;
            this.pageNo = 1;
        }
        public int GetTotalPage()
        {
            int count = this.list.Count;
            return parseInt(count / this.size) + ((count % this.size) > 0 ? 1 : 0);
        }
        public List<T> Next()
        {
            this.pageNo++;
            int total = this.GetTotalPage();
            if (this.pageNo > total) {
                this.pageNo = total;
            }
            return list.paging(this.pageNo, this.size);
        }
        public List<T> prev()
        {
            this.pageNo--;
            
            if (this.pageNo <= 0)
            {
                this.pageNo = 1;
            }
            return this.list.paging(this.pageNo, this.size);
        }

        public List<T> first() {
            this.pageNo = 1;
            return this.list.paging(this.pageNo, this.size);
        }
        public List<T> last()
        {
            this.pageNo = this.GetTotalPage();
            return this.list.paging(this.pageNo, this.size);
        }
        public List<T> GetPage(int pageNo)
        {
            this.pageNo = pageNo;
            return this.list.paging(this.pageNo, this.size);
        }
    }
}
