using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.Templates
{
    internal class TagTokenizer
    {
        #region Class Members
        ITextSource mBuf;

        int mIdx = 0;
        #endregion

        #region Constructors and Destructors
        public TagTokenizer(ITextSource buf)
        {
            Reset(buf);
        }
        #endregion

        #region Reset
        public void Reset(ITextSource buf)
        {
            mBuf = buf;
            mIdx = 0;
        }
        #endregion

        #region Token Retreival
        public TemplateToken GetToken()
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
                        return new TemplateToken(TemplateTokenType.TkTXT, offst, chars);

                    mIdx++;
                    return new TemplateToken(TemplateTokenType.TkRHT, mIdx, 1);
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
                                return new TemplateToken(TemplateTokenType.TkTXT, offst, chars);

                            mIdx += 2;
                            return new TemplateToken(TemplateTokenType.TkTXT, offst + 1, 1);
                        }

                        // Is this a LHT - ${
                        if (mBuf[mIdx + 1] == '{')
                        {
                            if (chars > 0)
                                return new TemplateToken(TemplateTokenType.TkTXT, offst, chars);

                            mIdx += 2;
                            return new TemplateToken(TemplateTokenType.TkLHT, offst, 2);
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
                return new TemplateToken(TemplateTokenType.TkTXT, offst, chars);

            // No more tokens
            return null;
        }

        public List<TemplateToken> GetTokens()
        {
            List<TemplateToken> list = new List<TemplateToken>();
            TemplateToken token;
            while ((token = GetToken()) != null)
                list.Add(token);
            return list;
        }
        public List<TemplateToken> GetTokens(ITextSource pattern)
        {
            Reset(pattern);
            return GetTokens();
        }
        public List<TemplateToken> GetTokens(string pattern)
        {
            Reset(new StringTextSource(pattern));
            return GetTokens();
        }
        public List<TemplateToken> GetTokens(StringBuilder pattern)
        {
            Reset(new StringBuilderTextSource(pattern));
            return GetTokens();
        }
        public List<TemplateToken> GetTokens(char[] pattern)
        {
            Reset(new CharArrayTextSource(pattern));
            return GetTokens();
        }

        public static List<TemplateToken> Tokenize(ITextSource pattern)
        {
            return new TagTokenizer(pattern).GetTokens();
        }
        public static List<TemplateToken> Tokenize(string pattern)
        {
            return new TagTokenizer(new StringTextSource(pattern)).GetTokens();
        }
        public static List<TemplateToken> Tokenize(StringBuilder pattern)
        {
            return new TagTokenizer(new StringBuilderTextSource(pattern)).GetTokens();
        }
        public static List<TemplateToken> Tokenize(char[] pattern)
        {
            return new TagTokenizer(new CharArrayTextSource(pattern)).GetTokens();
        }

        #endregion
    }
}
