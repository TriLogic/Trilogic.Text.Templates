using System;
using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.Templates
{
    #region Provider Delegate
    public delegate string TagValueProvider(string Key);
    #endregion

    public class TextTemplate
    {
        #region Class members
        private ITextSource mSource = new StringTextSource();
        private List<TemplateToken> mTokens = new List<TemplateToken>();
        private Stack<TemplateToken> mStack = new Stack<TemplateToken>();
        #endregion

        #region Constructors & Destructors
        public TextTemplate()
        {
        }
        #endregion

        #region Internal Properties
        internal List<TemplateToken> Tokens
        {
            get { return mTokens; }
            set { mTokens = value ?? new List<TemplateToken>(); }
        }

        internal ITextSource Source
        {
            get { return mSource; }
            set { mSource = value; }
        }

        internal Stack<TemplateToken> Stack
        {
            get { return mStack; }
        }
        #endregion

        #region Replace Tags
        public void ReplaceTags(StringBuilder target, TagValueProvider provider)
        {
            StringBuilder keyBuilder = new StringBuilder();

            // Clear the stack
            mStack.Clear();

            // assign an default provider
            if (provider == null)
            {
                provider = delegate (string key) {
                    return $"[{key}]";
                };
            }

            // Process the tokens
            foreach (TemplateToken tkn in mTokens)
            {
                // Text Token
                if (tkn.IsTXT)
                {
                    // Nothing on the stack so copy this text directly to the target
                    if (mStack.Count == 0)
                    {
                        target.Append(mSource.Substring(tkn.TokenOffset, tkn.TokenLength));
                    }
                    else
                    {
                        // This text is intended to be part of a key or a complete key value.
                        mStack.Peek().TokenLength += tkn.TokenLength;
                        keyBuilder.Append(mSource.Substring(tkn.TokenOffset, tkn.TokenLength));
                    }
                    continue;
                }

                // Left hand tag token "${"
                if (tkn.IsLHT)
                {
                    // Push a clone of the token  to the top of the stack. The tokens
                    // offset is the current length of the keyValue StringBuilder.
                    mStack.Push(tkn.Clone(keyBuilder.Length, 0));

                    continue;
                }

                // Right hand tag token "}"
                if (tkn.IsRHT)
                {
                    // If TOS token is not a LHT then we have a mismatch error on the tag strcuture,
                    // furthermore if the length of the tag is zero then we have an empty tag.
                    if (mStack.Count < 1 || !mStack.Peek().IsLHT || mStack.Peek().TokenLength == 0)
                        throw new Exception($"Invalid template: mismatch at offset {tkn.TokenOffset}");

                    // Retrieve the key value for the current token
                    TemplateToken keyToken = mStack.Pop();
                    string keyValue = keyBuilder.ToString(keyToken.TokenOffset, keyToken.TokenLength);

                    // Remove the key value text from the keyBuilder
                    keyBuilder.Remove(keyToken.TokenOffset, keyToken.TokenLength);

                    // Ask the value provider for the value that corresponds to the current key.
                    // It is now the job of the provider to throw an error if that's approriate.
                    string txtValue = provider(keyValue) ?? string.Empty;

                    // If the value returned has length
                    // non-empty stack indicates we are working on a compound key
                    if (mStack.Count > 0)
                    {
                        keyBuilder.Append(txtValue);
                        mStack.Peek().TokenLength += txtValue.Length;
                    }
                    else
                    {
                        target.Append(txtValue);
                    }
                }
            }

            // If the stack is not empty we have a mismatch somewhere in the template
            if (mStack.Count != 0)
                throw new Exception($"Invalid template: mismatch");
        }
        #endregion

        #region Static Compile Method
        public static TextTemplate Compile(ITextSource source)
        {
            List<TemplateToken> tokens = TagTokenizer.Tokenize(source);
            Stack<TemplateToken> stack = new Stack<TemplateToken>();

            foreach (TemplateToken token in tokens)
            {
                if (token.IsLHT)
                {
                    stack.Push(token);
                }

                if (token.IsRHT)
                {
                    if (stack.Count < 1)
                        throw new Exception($"Invalid template: mismatch at offset {token.TokenOffset}");

                    stack.Pop();
                }
            }

            if (stack.Count > 0)
            {
                throw new Exception($"Invalid template: mismatch at offset {stack.Peek().TokenOffset}");
            }

            // Return new textTemplate
            return new TextTemplate
            {
                mSource = source,
                mTokens = tokens
            };
        }
        #endregion
    }
}
