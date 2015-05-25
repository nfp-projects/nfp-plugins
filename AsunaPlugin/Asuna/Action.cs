using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsunaPlugin.Asuna
{
    public class Action
    {
        public int Id { get; set; }
        public DateTime Timer { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
