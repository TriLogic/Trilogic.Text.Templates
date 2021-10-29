using System;

namespace Trilogic.Text.Templates
{
    public interface ITextSource
    {
        int Length { get; }
        char this[int index] { get; }
        string Substring(int offset, int length);
    }
}
