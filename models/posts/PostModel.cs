using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_example_1.models.posts
{
    public class PostModel
    {
        public bool isChecked { get; set; }
        public long id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public long userId { get; set; }
    }
}
