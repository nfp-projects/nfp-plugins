using System;
using System.Collections.Generic;
using System.Linq;
using IRCPlugin.DB;
using System.Text;
using System.Threading.Tasks;

namespace WhipPlugin
{
    public class Whip
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Order { get; set; }
        public string Owner { get; set; }
        public string Link { get; set; }
    }
}
