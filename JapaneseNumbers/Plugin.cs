using System;
using System.Collections.Generic;
using System.Linq;
using ChatSharp.Events;
using IRCPlugin;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseNumbers
{
    public class Plugin : BotPlugin
    {
        Dictionary<string, JapaneseNumber> _answers;
        List<string> _seen;

        public Plugin(IIrcClient client)
            : base(client)
        {
            Name = "Japanase Number Module";
            _answers = new Dictionary<string, JapaneseNumber>();
            _seen = new List<string>();
            Status = "Loaded & Running";
        }

        protected override void ChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            string message = e.PrivateMessage.Message.ToLower();

            if (message.StartsWith("!nt"))
            {
                GiveQuestion(e, message.Contains("number"));
                return;
            }
            else if (message.StartsWith("=") && message.Length > 1 && _answers.ContainsKey(e.PrivateMessage.User.Nick.ToLower()))
            {
                CheckAnswer(message.Remove(0, 1), e);
                return;
            }
        }

        public void GiveQuestion(PrivateMessageEventArgs e, bool numberTest)
        {
            string nick = e.PrivateMessage.User.Nick.ToLower();

            if (!_seen.Contains(e.PrivateMessage.User.Nick.ToLower()))
            {
                this._client.Client.SendMessage("Welcome to Japanese (Romaji) Number training. Answer with: \"=answer\" without quotation marks.", e.PrivateMessage.Source);
                _seen.Add(e.PrivateMessage.User.Nick.ToLower());
            }

            var number = new JapaneseNumber(numberTest);
            if (_answers.ContainsKey(nick))
                _answers.Remove(nick);
            _answers.Add(nick, number);

            this._client.Client.SendMessage(string.Format("{0}: {1}", e.PrivateMessage.User.Nick, numberTest ? number.NumberText : number.Number.ToString()), e.PrivateMessage.Source);
        }

        public void CheckAnswer(string answer, PrivateMessageEventArgs e)
        {
            var number = _answers[e.PrivateMessage.User.Nick.ToLower()];
            _answers.Remove(e.PrivateMessage.User.Nick.ToLower());
            if (answer == "answer")
            {
                this._client.Client.SendMessage(string.Format("{0}: Answer was {1}", e.PrivateMessage.User.Nick, number.IsNumberTest ? number.Number.ToString() : number.NumberText), e.PrivateMessage.Source);
            }
            else if (number.Match(answer))
            {
                this._client.Client.SendMessage(string.Format("{0}: Correct! {1} = {2}", e.PrivateMessage.User.Nick, number.Number, number.NumberText), e.PrivateMessage.Source);
            }
            else
            {
                this._client.Client.SendMessage(string.Format("{0}: Wrong, correct answer is {1}", e.PrivateMessage.User.Nick, number.IsNumberTest ? number.Number.ToString() : number.NumberText), e.PrivateMessage.Source);
            }
        }
    }
}
