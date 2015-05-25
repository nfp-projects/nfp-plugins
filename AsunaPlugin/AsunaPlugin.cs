using System;
using System.Collections.Generic;
using System.Linq;
using IRCPlugin;
using System.Text;
using System.Threading;
using IRCPlugin.DB;
using AsunaPlugin.Asuna;

namespace AsunaPlugin
{
    public class AsunaPlugin : BotPlugin
    {
        private Database _database;
        private Asuna.Asuna _asuna;
        private Dictionary<string, int> _mood;
        private Dictionary<string, DateTime> _timers;
        private Timer _t;
        private List<Action> _actions;
        private Database _db;

        public AsunaPlugin(IIrcClient client)
            : base(client)
        {
            Name = "Asuna Plugin";
            Status = "Asuna plugin loaded";

            Database.RegisterTypeFromAssembly();
            var ser = new IRCPlugin.DB.Serializer.Serializer();

            _database = new Database("asuna.db", true);

            _asuna = new Asuna.Asuna();
            foreach (var user in _database.Table<User>().EnsureExists().Select())
            {
                _asuna.Users.Add(user);
            }
            _asuna.Users.Add(new User { Username = "test", Mood = 1 });
            _database.Table<User>().Save(_asuna.Users);

            //_actions = new List<Action>();
            //_mood = new Dictionary<string, int>();
            //_timers = new Dictionary<string, DateTime>();

            //_actions.Add(new Action(this, "hello", new int[] { -5, 0, 5 },
            //    new string[] { "Please stop talking to me", "Can you leave me alone?", "Sigh", "Urusai!", "No!", "I hate you!" },
            //    new string[] { "Uhh, hi?", "H-Hi?", "Yeah?", "Do you need something?"},
            //    new string[] { "Hi o/", "Hello!", "Hi!", "o/"},
            //    new string[] { "Hi %s ^_^", "Hi <3", "That's me <3", "%s <3"}));

            //a.Actions.Add(_actions[0]);

            //_database.SetOption("test", a);

            //_t = new Timer(Timer_Ticked, null, 60000, 60 * 1000);
        }

        protected void Timer_Ticked(object state)
        {
            Status = string.Format("Timer ticked. Parsing {0} users.", _timers.Keys.Count);

            lock (_timers)
            {
                var keys = new List<string>(_timers.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    var delta = (DateTime.Now - _timers[keys[i]]);
                    if (delta.TotalMinutes > 5 && _mood[keys[i]] < 0)
                    {
                        _mood[keys[i]]++;
                        _timers[keys[i]] = DateTime.Now;
                    }
                    else if (delta.TotalDays > 1 && _mood[keys[i]] > 0)
                    {
                        _mood[keys[i]]--;
                        _timers[keys[i]] = DateTime.Now;
                    }
                }
            }
        }

        public override IList<string> Buttons
        {
            get { return new string[] { "About" }; }
        }

        protected override void ChannelMessageReceived(object sender, ChatSharp.Events.PrivateMessageEventArgs e)
        {
            string message = e.PrivateMessage.Message.ToLower();

            User user = null;
            try
            {
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            if (message == "$affection")
            {
                _client.Client.SendMessage(String.Format("« your affection is at {1}{0} »", user.Mood, user.Mood >= 0 ? "+" : ""), e.PrivateMessage.Source);
                return;
            }

            if (!message.Contains("asuna-chan"))
            {
                Status = string.Format("No Asuna in '{0}', ignoring", message);
                return;
            }
        }

        public override void Open(string name)
        {
            if (_window != null)
                return;
            About w = new About();
            OpenWindow(w);
        }

        public override void Dispose()
        {
            if (_disposed)
                return;

            base.Dispose();
            _database.Dispose();
        }
    }
}
