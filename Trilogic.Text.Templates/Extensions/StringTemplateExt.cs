using System.Text;

namespace Trilogic.Text.Templates.Extensions
{
    public static class StringTemplateExt
    {
        public static StringBuilder ReplaceTags(this string me, TagValueProvider provider)
        {
            StringBuilder target = new StringBuilder();
            TextTemplate.Compile(new StringTextSource(me))
                .ReplaceTags(target, provider);
            return target;
        }
    }
}
