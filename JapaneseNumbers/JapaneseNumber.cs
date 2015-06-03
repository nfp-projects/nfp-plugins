using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseNumbers
{
    public class JapaneseNumber
    {
        #region Static Stuff
        private static Dictionary<int, string[]> _dictDigits = new Dictionary<int, string[]>
        {
            {0, new string[] {"rei", "maru", "zero"}},
            {1, new string[] {"ichi"}},
            {2, new string[] {"ni"}},
            {3, new string[] {"san"}},
            {4, new string[] {"shi", "yon"}},
            {5, new string[] {"go"}},
            {6, new string[] {"roku"}},
            {7, new string[] {"shichi", "nana"}},
            {8, new string[] {"hachi"}},
            {9, new string[] {"ku", "kyuu"}}
        };
        private static string[] _dictPattern = new string[] {
            "",
            "juu",
            "hyaku",
            "sen"
        };
        private static Dictionary<int, Dictionary<int, string>> _dictExceptions = new Dictionary<int, Dictionary<int, string>>
        {
            {1, new Dictionary<int, string> {
                {4, "yon juu"},
                {7, "nana juu"},
                {9, "kyuu juu"}
            }},
            {2, new Dictionary<int, string> {
                {3, "san byaku"},
                {4, "yon hyaku"},
                {6, "roppyaku"},
                {7, "nana hyaku"},
                {8, "happyaku"},
                {9, "kyuu hyaku"}
            }},
            {3, new Dictionary<int, string> {
                {3, "san zen"},
                {4, "yon sen"},
                {7, "nana sen"},
                {8, "hyassen"},
                {9, "kyuu sen"}
            }}
        };
        #endregion

        private readonly int _number;
        private readonly bool _isNumberTest;
        private readonly string _numberText;
        private readonly string _numberTextSafe;
        private readonly DateTime _dateTimeStart;

        public JapaneseNumber(bool isNumberTest)
            : this(isNumberTest, -1)
        {
        }

        public JapaneseNumber(bool isNumberTest, int number)
        {
            if (number < 0)
            {
                var r = new Random();
                number = r.Next(0, 9999);
            }
            _isNumberTest = isNumberTest;
            _number = number;
            _numberText = JapaneseNumber.Format(_number);
            _numberTextSafe = JapaneseNumber.Format(_number, true).Replace(" ", "");
            _dateTimeStart = DateTime.Now;
        }

        public bool Match(string input)
        {
            if (_isNumberTest)
            {
                int test = 0;
                if (!int.TryParse(input, out test))
                    return false;
                return test == _number;
            }
            input = input.Replace(" ", "");
            if (input.IndexOf(_numberTextSafe) != 0)
                return false;
            if (!string.IsNullOrEmpty(_numberTextSafe))
                input = input.Remove(0, _numberTextSafe.Length);
            if (!_dictDigits[_number % 10].Contains(input))
                return false;
            return true;
            
        }

        public static string Format(int number, bool safe = false)
        {
            var output = "";
            var temp = number.ToString().Select(c => (int)Char.GetNumericValue(c)).Reverse().ToArray();
            if (temp.Length > 4)
                return "Error";

            for (int i = 0; i < temp.Length; i++)
            {
                if (_dictExceptions.ContainsKey(i) && _dictExceptions[i].ContainsKey(temp[i]))
                    output = String.Format("{0} {1}", _dictExceptions[i][temp[i]], output);
                else if (i > 0 && temp[i] > 1)
                    output = String.Format("{0} {1} {2}", _dictDigits[temp[i]][0], _dictPattern[i], output);
                else if (i > 0 && temp[i] == 1)
                    output = String.Format("{0} {1}", _dictPattern[i], output);
                else if (i == 0 && temp[i] > 0 && !safe)
                    output = String.Format("{0} {1}", _dictDigits[temp[i]][0], output);
                else if (i == 0 && temp[i] == 0 && temp.Length == 1 && !safe)
                    output = String.Format("{0} {1}", _dictDigits[temp[i]][0], output);
            }

            return output.Trim();
        }

        public DateTime DateTimeStart
        {
            get { return _dateTimeStart; }
        }

        public bool IsNumberTest
        {
            get { return _isNumberTest; }
        }

        public int Number
        {
            get { return _number; }
        }

        public string NumberText
        {
            get { return _numberText; }
        }
    }
}
