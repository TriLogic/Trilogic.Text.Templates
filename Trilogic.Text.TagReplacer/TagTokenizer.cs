using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.TagReplacer
{
    internal class TagTokenizer
    {
        #region Class Members
        ITagPattern mBuf;

        int mIdx = 0;
        #endregion

        #region Constructors and Destructors
        public TagTokenizer(ITagPattern buf)
        {
            Reset(buf);
        }
        #endregion

        #region Reset
        public void Reset(ITagPattern buf)
        {
            mBuf = buf;
            mIdx = 0;
        }
        #endregion

        #region Token Retreival
        public TagToken GetToken()
        {
            int offst = mIdx;
            int chars = 0;

            while (mIdx < mBuf.Length)
            {
                char tkc = mBuf[mIdx];

                // Closure
                if (tkc == '}')
                {
                    if (chars > 0)
                        return new TagToken(TokenType.TkTXT, offst, chars);

                    mIdx++;
                    return new TagToken(TokenType.TkRHT, mIdx, 1);
                }

                // LHT or Escape
                if (tkc == '$')
                {
                    // Is there room for a LHT or Escape?
                    if (mIdx + 1 < mBuf.Length)
                    {
                        // Is this an escape - $$ or $}?
                        if (mBuf[mIdx + 1] == '$' || mBuf[mIdx + 1] == '}')
                        {
                            if (chars > 0)
                                return new TagToken(TokenType.TkTXT, offst, chars);

                            mIdx += 2;
                            return new TagToken(TokenType.TkTXT, offst + 1, 1);
                        }

                        // Is this a LHT - ${
                        if (mBuf[mIdx + 1] == '{')
                        {
                            if (chars > 0)
                                return new TagToken(TokenType.TkTXT, offst, chars);

                            mIdx += 2;
                            return new TagToken(TokenType.TkLHT, offst, 2);
                        }

                        // Dangling '$' char fall though
                    }
                }

                // add to the growing list of characters
                chars++;
                mIdx++;
            }

            // if we have a token value
            if (chars > 0)
                return new TagToken(TokenType.TkTXT, offst, chars);

            // No more tokens
            return null;
        }

        public List<TagToken> GetTokens()
        {
            List<TagToken> list = new List<TagToken>();
            TagToken tkn = GetToken();
            while ((tkn = GetToken() ) != null)
                list.Add(tkn);
            return list;
        }
        public List<TagToken> GetTokens(ITagPattern pattern)
        {
            Reset(pattern);
            return GetTokens();
        }
        public List<TagToken> GetTokens(string pattern)
        {
            Reset(new StringTagPattern(pattern));
            return GetTokens();
        }
        public List<TagToken> GetTokens(StringBuilder pattern)
        {
            Reset(new StringBuilderTagPattern(pattern));
            return GetTokens();
        }
        public List<TagToken> GetTokens(char[] pattern)
        {
            Reset(new CharArrayTagPattern(pattern));
            return GetTokens();
        }

        public static List<TagToken> Tokenize(ITagPattern pattern)
        {
            return new TagTokenizer(pattern).GetTokens();
        }
        public static List<TagToken> Tokenize(string pattern)
        {
            return new TagTokenizer(new StringTagPattern(pattern)).GetTokens();
        }
        public static List<TagToken> Tokenize(StringBuilder pattern)
        {
            return new TagTokenizer(new StringBuilderTagPattern(pattern)).GetTokens();
        }
        public static List<TagToken> Tokenize(char[] pattern)
        {
            return new TagTokenizer(new CharArrayTagPattern(pattern)).GetTokens();
        }

        #endregion
    }
}
