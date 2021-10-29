using System.Text;
using System.IO;

namespace Trilogic.Text.Templates.Extensions
{
    public static class StreamTemplateExt
    {
        public static StringBuilder ReplaceTags(this Stream me, TagValueProvider provider)
        {
            return new StreamReader(me).ReplaceTags(provider);
        }
    }
}
