using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.TagReplacer
{
    public enum TokenType
    {
        TkNIL = 0,
        TkLHT = 1,
        TkTXT = 2,
        TkRHT = 3
    }

    public class TagToken
    {
        #region Class Members
        public TokenType TknType;
        public int TknOffset;
        public int TknLength;
        #endregion

        #region Constructors and Destructors
        public TagToken()
        {
            TknType = TokenType.TkNIL;
        }
        public TagToken(TokenType type, int offset)
        {
            TknType = type;
            TknOffset = offset;
        }
        public TagToken(TokenType type, int offset, int length)
        {
            TknType = type;
            TknOffset = offset;
            TknLength = length;
        }
        #endregion

        #region Helper Methods
        public void SetTXT(int offset, int length)
        {
            TknType = TokenType.TkTXT;
            TknOffset = offset;
            TknLength = length;
        }

        public void SetLHT(int offset)
        {
            TknType = TokenType.TkLHT;
            TknOffset = offset;
            TknLength = 2;
        }

        public void SetRHT(int offset)
        {
            TknType = TokenType.TkLHT;
            TknOffset = offset;
            TknLength = 1;
        }

        public void SetNIL()
        {
            TknType = TokenType.TkNIL;
            TknOffset = 0;
            TknLength = 0;
        }

        public TagToken Clone()
        {
            return new TagToken(TknType, TknOffset, TknLength);
        }
        public TagToken Clone(int offset)
        {
            return new TagToken(TknType, offset, TknLength);
        }
        public TagToken Clone(int offset, int length)
        {
            return new TagToken(TknType, offset, length);
        }
        #endregion

        #region Properties
        public bool IsNIL
        {
            get { return TknType == TokenType.TkNIL; }
        }

        public bool IsLHT
        {
            get { return TknType == TokenType.TkLHT; }
        }

        public bool IsTXT
        {
            get { return TknType == TokenType.TkTXT; }
        }

        public bool IsRHT
        {
            get { return TknType == TokenType.TkRHT; }
        }
        #endregion
    }
}
