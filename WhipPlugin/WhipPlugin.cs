using System;
using System.Collections.Generic;
using System.Linq;
using IRCPlugin;
using IRCPlugin.DB;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WhipPlugin
{
    public class WhipPlugin : BotPlugin
    {
        private Database _database;
        private Table<Whip> _whips;

        public WhipPlugin(IIrcClient client)
            : base(client)
        {
            Name = "Whipping Module";

            _database = new Database("whips.db");
            _whips = _database.Table<Whip>().EnsureExists();

            this.Status = "Loaded";
        }

        protected override void ChannelMessageReceived(object sender, ChatSharp.Events.PrivateMessageEventArgs e)
        {
            string message = e.PrivateMessage.Message.ToLower();

            if (message.StartsWith(".whips"))
            {
                var whips = _whips.Select(new { Owner = e.PrivateMessage.User.Nick.ToLower() });

                if (message == ".whips")
                {
                    var total = whips.Count();
                    this._client.Client.SendMessageRaw("NOTICE {0} :« You have {1} whip{2} »", e.PrivateMessage.User.Nick, total.ToString(), total != 1 ? "s" : "");
                    return;
                }
                if (!message.StartsWith(".whips "))
                    return;

                var command = message.Split(' ')[1];
                if (command == "")
                    throw new Exception();

                switch (command)
                {
                    case "list":
                        if (message.Split(' ').Length == 3 && e.PrivateMessage.User.Nick.ToLower() == "thething|24-7")
                            whips = _whips.Select(new { Owner = message.Split(' ')[2].ToLower() });
                        var list = whips.ToArray();
                        for (int i = 0; i < list.Length; i++)
                        {
                            this._client.Client.SendMessageRaw("NOTICE {0} :{1} = {2}", e.PrivateMessage.User.Nick, i.ToString(), list[i].Link);
                        }
                        if (list.Length == 0)
                            this._client.Client.SendMessageRaw("NOTICE {0} :You have no whips", e.PrivateMessage.User.Nick);
                        return;
                    case "help":
                        this._client.Client.SendMessageRaw("NOTICE {0} :.whips      - Get total whips you have", e.PrivateMessage.User.Nick);
                        this._client.Client.SendMessageRaw("NOTICE {0} :.whips list - Get list of all the whips", e.PrivateMessage.User.Nick);
                        this._client.Client.SendMessageRaw("NOTICE {0} :.whips help - Display this help", e.PrivateMessage.User.Nick);
                        this._client.Client.SendMessageRaw("NOTICE {0} :.whip nick  - Whip that nigga", e.PrivateMessage.User.Nick);
                        return;
                    case "available":
                        whips = _whips.Select(new { Owner = "" });
                        var total = whips.Count();
                        this._client.Client.SendMessage(string.Format("« There are {0} whip{1} available »", total, total != 1 ? "s" : ""), e.PrivateMessage.Source);
                        return;
                    default:
                        this._client.Client.SendMessageRaw("NOTICE {0} :.whip help  - Display help", e.PrivateMessage.User.Nick);
                        return;
                }
                return;
            }
            if (message.StartsWith(".give ") && e.PrivateMessage.User.Nick == "TheThing|24-7")
            {
                try
                {
                    var nick = message.Split(' ')[1];
                    var first = _whips.Select(new { Owner = "" }).OrderBy(k => k.Order).First();
                    first.Owner = nick;
                    _whips.Set(first);
                    this._client.Client.SendMessageRaw("NOTICE {0} :You just got a new whip {1}", nick, first.Link);
                    this._client.Client.SendMessageRaw("NOTICE {0} :You gave {1} {2}", e.PrivateMessage.User.Nick, nick, first.Link);
                }
                catch (Exception error)
                {
                    this.Status = error.Message;
                }
            }
            if (message.StartsWith(".whip "))
            {
                var whips = _whips.Select(new { Owner = e.PrivateMessage.User.Nick.ToLower() });

                if (whips.Count() == 0)
                {
                    this._client.Client.SendMessageRaw("NOTICE {0} :Nobody by that name exists on this channel", e.PrivateMessage.User.Nick);
                    return;
                }

                var target = message.Split(' ')[1];

                var randomIndex = new Random().Next(0, whips.Count() - 1);
                this._client.Client.SendMessage(string.Format("You whip {0} with {1}", target, whips.ElementAt(randomIndex).Link), e.PrivateMessage.Source);
            }
        }

        public override IList<string> Buttons
        {
            get { return new string[] { "Whips" }; }
        }

        public override void Open(string name)
        {
            if (_window != null)
                return;
            WhipWindow w = new WhipWindow(this);
            OpenWindow(w);
        }

        public Table<Whip> Whips
        {
            get { return _whips; }
        }
    }
}
