using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.TagReplacer
{
    public interface ITagPattern
    {
        int Length { get; }
        char this[int index] { get; }
        string Substring(int offset, int length);
    }

    public class StringTagPattern : ITagPattern
    {
        private string _pattern;

        public StringTagPattern()
        {
            _pattern = string.Empty;
        }
        public StringTagPattern(string pattern)
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

    public class StringBuilderTagPattern : ITagPattern
    {
        private StringBuilder _pattern;

        public StringBuilderTagPattern(StringBuilder pattern)
        {
            _pattern = (pattern == null) ? new StringBuilder(): pattern;
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

    public class CharArrayTagPattern : ITagPattern
    {
        private char[] _pattern;

        public CharArrayTagPattern(char[] pattern)
        {
            _pattern = (pattern == null) ? new char[0] : pattern;
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
