using System.Text;

namespace Trilogic.Text.Templates.Extensions
{
    public static class StringBuilderTemplateExt
    {
        public static StringBuilder ReplaceTags(this StringBuilder me, TagValueProvider provider)
        {
            StringBuilder target = new StringBuilder();
            TextTemplate.Compile(new StringBuilderTextSource(me))
                .ReplaceTags(target, provider);
            return target;
        }
    }
}
