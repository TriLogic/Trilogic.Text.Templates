using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates
{
    public class CharArrayTextSource : ITextSource
    {
        private char[] _pattern;

        public CharArrayTextSource(char[] pattern)
        {
            _pattern = pattern ?? new char[0];
        }

        public int Length { get { return _pattern.Length; } }

        public char this[int index]
        {
            get { return _pattern[index]; }
        }

        public string Substring(int offset, int length)
        {
            return new string(_pattern, offset, length);
        }
    }
}
