using System.Diagnostics;
using Windows.Foundation.Metadata;

namespace AnnotatorRuntimeComponent
{
    [AllowForWeb]
    public sealed class HtmlCommunicator
    {
        public void alert(string text, string annotation)
        {
            string english = annotation;
            string chinese = text.Split(' ')[0];
            string pinyin = text.Replace(chinese + " ", "");

            Debug.WriteLine("CLICKED => " + chinese + " : " + english + " : " + pinyin);
        }

        public string annotate(string text)
        {
            string result = new Annotator(text).Result();
            result = result.Replace(@"<ruby title=""", @"<ruby onclick=""annotPopAll(this)"" title=""");

            return result;
        }

        public void getHtml(string html)
        {
            Debug.WriteLine(html);
        }

        public void test(string s)
        {
            Debug.WriteLine("FROM TEST!!!! => " + s);
        }
    }
}