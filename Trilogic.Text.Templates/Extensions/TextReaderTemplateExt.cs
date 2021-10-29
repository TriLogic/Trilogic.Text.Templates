using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Trilogic.Text.Templates.Extensions
{
    public static class TextReaderTemplateExt
    {
        public static StringBuilder ReplaceTags(this TextReader me, TagValueProvider provider)
        {
            StringBuilder target = new StringBuilder();
            TextTemplate.Compile(new StringTextSource(me.ReadToEnd()))
                .ReplaceTags(target, provider);
            return target;
        }
    }
}
