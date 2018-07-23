using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppX.Models
{
    class MenuItem
    {
        public int id { get; set; }
        public String name { get; set; }
        public String path { get; set; }

        public MenuItem(int id, string name,string path)
        {
            this.id = id;
            this.name = name;
            this.path = path;
        }


        public MenuItem()
        {

        }

        public List<MenuItem> getMenu()
        {
            List<MenuItem> list = new List<MenuItem>();
            list.Add(new MenuItem(1,"TRANG CHỦ" ,"/all/"));
            list.Add(new MenuItem(2,"TRUYỆN TIÊN HIỆP" ,"/tien-hiep/"));
            list.Add(new MenuItem(3,"TRUYỆN KIẾM HIỆP" ,"/kiem-hiep/"));
            list.Add(new MenuItem(4,"TRUYỆN NGÔN TÌNH" ,"/ngon-tinh/"));
            list.Add(new MenuItem(5,"TRUYỆN ĐÔ THỊ" ,"/do-thi/"));
            list.Add(new MenuItem(6,"TRUYỆN XUYÊN KHÔNG" ,"/xuyen-khong/"));
            list.Add(new MenuItem(7, "TRUYỆN ƯA THÍCH", ""));
            return list;
        }
    }
}
