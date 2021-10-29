using System.Text;
using System.IO;

namespace Trilogic.Text.Templates.Extensions
{
    public static class StreamReaderTemplateExt
    {
        public static StringBuilder ReplaceTags(this StreamReader me, TagValueProvider provider)
        {
            StringBuilder target = new StringBuilder();
            TextTemplate.Compile(new StringTextSource(me.ReadToEnd()))
                .ReplaceTags(target, provider);
            return target;
        }
    }
}
