using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates
{
    public enum TemplateTokenType
    {
        TkNIL = 0,
        TkLHT = 1,
        TkTXT = 2,
        TkRHT = 3
    }

    public class TemplateToken
    {
        #region Class Members
        public TemplateTokenType TokenType;
        public int TokenOffset;
        public int TokenLength;
        #endregion

        #region Constructors and Destructors
        public TemplateToken()
        {
            TokenType = TemplateTokenType.TkNIL;
        }
        public TemplateToken(TemplateTokenType type, int offset)
        {
            TokenType = type;
            TokenOffset = offset;
        }
        public TemplateToken(TemplateTokenType type, int offset, int length)
        {
            TokenType = type;
            TokenOffset = offset;
            TokenLength = length;
        }
        #endregion

        #region Helper Methods
        public void SetTXT(int offset, int length)
        {
            TokenType = TemplateTokenType.TkTXT;
            TokenOffset = offset;
            TokenLength = length;
        }

        public void SetLHT(int offset)
        {
            TokenType = TemplateTokenType.TkLHT;
            TokenOffset = offset;
            TokenLength = 2;
        }

        public void SetRHT(int offset)
        {
            TokenType = TemplateTokenType.TkLHT;
            TokenOffset = offset;
            TokenLength = 1;
        }

        public void SetNIL()
        {
            TokenType = TemplateTokenType.TkNIL;
            TokenOffset = 0;
            TokenLength = 0;
        }

        public TemplateToken Clone()
        {
            return new TemplateToken(TokenType, TokenOffset, TokenLength);
        }
        public TemplateToken Clone(int offset)
        {
            return new TemplateToken(TokenType, offset, TokenLength);
        }
        public TemplateToken Clone(int offset, int length)
        {
            return new TemplateToken(TokenType, offset, length);
        }
        #endregion

        #region Properties
        public bool IsNIL
        {
            get { return TokenType == TemplateTokenType.TkNIL; }
        }

        public bool IsLHT
        {
            get { return TokenType == TemplateTokenType.TkLHT; }
        }

        public bool IsTXT
        {
            get { return TokenType == TemplateTokenType.TkTXT; }
        }

        public bool IsRHT
        {
            get { return TokenType == TemplateTokenType.TkRHT; }
        }
        #endregion
    }
}
