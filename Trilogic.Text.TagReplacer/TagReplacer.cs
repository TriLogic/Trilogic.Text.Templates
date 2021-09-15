using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.TagReplacer
{
    #region Provider Delegate
    public delegate bool TagProvider(string Key, out string Value);
    #endregion

    public class TagReplacer
    {
        #region Class members
        private ITagPattern mPattern = new StringTagPattern();
        private List<TagToken> mTokens = new List<TagToken>();
        private Stack<TagToken> mStack = new Stack<TagToken>();
        #endregion

        #region Constructors & Destructors
        public TagReplacer()
        {
        }
        public TagReplacer(string pattern)
        {
            Compile(pattern);
        }
        public TagReplacer(StringBuilder pattern)
        {
            Compile(pattern);
        }
        public TagReplacer(char[] pattern)
        {
            Compile(pattern);
        }
        public TagReplacer(ITagPattern pattern)
        {
            Compile(pattern);
        }
        #endregion

        #region Compile Methods
        public void Compile(string pattern)
        {
            Compile(new StringTagPattern(pattern));
        }
        public void Compile(StringBuilder pattern)
        {
            Compile(new StringBuilderTagPattern(pattern));
        }
        public void Compile(char[] pattern)
        {
            Compile(new CharArrayTagPattern(pattern));
        }
        public void Compile(ITagPattern pattern)
        {
            // store the pattern
            mPattern = pattern == null ? new StringTagPattern() : pattern;

            // generate a list of tokens
            mTokens = TagTokenizer.Tokenize(mPattern);
        }
        #endregion

        #region Replace with New Pattern
        public void Replace(string pattern, TagProvider provider, StringBuilder target)
        {
            Compile(pattern);
            Replace(provider, target);
        }
        public void Replace(StringBuilder pattern, TagProvider provider, StringBuilder target)
        {
            Compile(pattern);
            Replace(provider, target);
        }
        public void Replace(char[] pattern, TagProvider provider, StringBuilder target)
        {
            Compile(pattern);
            Replace(provider, target);
        }
        public void Replace(ITagPattern pattern, TagProvider provider, StringBuilder target)
        {
            Compile(pattern);
            Replace(provider, target);
        }
        #endregion

        #region Replace with Existing Pattern
        public void Replace(TagProvider provider, StringBuilder target)
        {
            // assign an empty defaul provider
            if (provider == null)
            {
                provider = delegate (string key, out string value) {
                    value = $"[{key}]"; return true;
                };
            }

            // clear the target
            target.Clear();

            // clear the stack
            mStack.Clear();

            // Process the tokens
            foreach (TagToken tkn in mTokens)
            {
                // Text Token
                if (tkn.IsTXT)
                {
                    // Append the text into the target StringBuilder
                    target.Append(mPattern.Substring(tkn.TknOffset, tkn.TknLength));

                    if (mStack.Count == 0)
                    {
                        mStack.Push(tkn.Clone(0));
                    }
                    else
                    {
                        // Include the new text to the TOS tag
                        // and we don't care what type it is.
                        mStack.Peek().TknLength += tkn.TknLength;
                    }
                    continue;
                }

                // Left hand tag
                if (tkn.IsLHT)
                {
                    mStack.Push(tkn.Clone(target.Length, 0));
                    continue;
                }

                // Right hand token
                if (tkn.IsRHT)
                {
                    // If TOS is not a RHT then we have a format error
                    if (mStack.Count < 1 || !mStack.Peek().IsLHT)
                        throw new Exception($"Invalid pattern: mismtach at offset {tkn.TknOffset}");

                    // We must have non-zero text for the Tag lookup
                    if (mStack.Peek().TknLength == 0)
                        throw new Exception($"Invalid pattern: empty key at offset {tkn.TknOffset}");

                    TagToken keyToken = mStack.Peek();
                    string keyValue = target.ToString(keyToken.TknOffset, keyToken.TknLength);

                    // Are we trying to lookup an empty key?
                    if (string.IsNullOrEmpty(keyValue))
                        throw new Exception($"Invalid pattern: empty null of empty key at offset {tkn.TknOffset}");

                    // the is the resulting text value
                    string txtValue = null;

                    if (!provider(keyValue, out txtValue))
                        throw new Exception($"Invalid pattern: unresolved key [{keyValue}]");

                    // remove the key text from the target
                    target.Remove(keyToken.TknOffset, keyToken.TknLength);

                    // Append the replacement text target (don't allow null)
                    target.Append(string.IsNullOrEmpty(txtValue) ? string.Empty : txtValue);

                    // Discard the key token
                    if (mStack.Count == 1)
                    {
                        // First token was a LHT so it becomes a TXT an the only
                        // text in the target is the result of the replacement.
                        mStack.Peek().SetTXT(0, txtValue.Length);
                        continue;
                    }

                    // Discard the TOS LHT
                    mStack.Pop();

                    // add the replacement text to the TOS token
                    mStack.Peek().TknLength += txtValue.Length;
                }
            }

            // The stack could contain only single item
            if (mStack.Count != 1)
                throw new Exception($"Invalid pattern: mismatch");

        }
        #endregion
    }
}
