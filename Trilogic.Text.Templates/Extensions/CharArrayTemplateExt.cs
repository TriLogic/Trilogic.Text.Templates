using System.Text;

namespace Trilogic.Text.Templates.Extensions
{
    public static class CharArrayTemplateExt
    {
        public static StringBuilder ReplaceTags(this char[] me, TagValueProvider provider)
        {
            StringBuilder target = new StringBuilder();
            TextTemplate.Compile(new CharArrayTextSource(me))
                .ReplaceTags(target, provider);
            return target;
        }
    }
}
