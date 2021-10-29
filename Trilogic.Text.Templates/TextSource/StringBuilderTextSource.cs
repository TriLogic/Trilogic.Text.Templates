using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates
{
    public class StringBuilderTextSource : ITextSource
    {
        private StringBuilder _pattern;

        public StringBuilderTextSource(StringBuilder pattern)
        {
            _pattern = pattern ?? new StringBuilder();
        }

        public int Length { get { return _pattern.Length; } }

        public char this[int index]
        {
            get { return _pattern[index]; }
        }

        public string Substring(int offset, int length)
        {
            return _pattern.ToString(offset, length);
        }
    }
}
