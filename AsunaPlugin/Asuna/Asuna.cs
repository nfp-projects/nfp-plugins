using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCPlugin.DB;

namespace AsunaPlugin.Asuna
{
    class Asuna
    {
        private List<User> _users;

        public Asuna()
        {
            _users = new List<User>();
        }

        public List<User> Users
        {
            get { return _users; }
        }
    }
}
