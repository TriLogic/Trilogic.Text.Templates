using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates
{
    public class StringTextSource : ITextSource
    {
        private string _pattern;

        public StringTextSource()
        {
            _pattern = string.Empty;
        }
        public StringTextSource(string pattern)
        {
            _pattern = string.IsNullOrEmpty(pattern) ? string.Empty : pattern;
        }

        public int Length { get { return _pattern.Length; } }

        public char this[int index]
        {
            get { return _pattern[index]; }
        }

        public string Substring(int offset, int length)
        {
            return _pattern.Substring(offset, length);
        }
    }
}
