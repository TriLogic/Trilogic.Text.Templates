using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates
{
    public interface ITextSource
    {
        int Length { get; }
        char this[int index] { get; }
        string Substring(int offset, int length);
    }
}
