using System;
using System.Collections.Generic;
using System.Linq;
using IRCPlugin.DB;
using System.Text;
using System.Threading.Tasks;

namespace AsunaPlugin.Asuna
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }
        public int Mood { get; set; }
        public DateTime Timer { get; set; }
    }
}