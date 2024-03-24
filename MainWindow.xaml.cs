using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace wpf_get_fonts
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_get_fonts_Click(object sender, RoutedEventArgs e)
        {
            GetFontsStr();
        }

        private void GetFontsStr()
        {
            var fontFamilies = Fonts.SystemFontFamilies;

            // 按字体名称进行排序
            var sortedFontFamilies = fontFamilies.OrderBy(family => family.Source);

            // 使用 StringBuilder 来构建字体字符串
            StringBuilder fontsStringBuilder = new StringBuilder();

            var p = new Paragraph();
            foreach (FontFamily fontfamily in sortedFontFamilies)
            {
                string fontName = GetFontName(fontfamily);
                if (fontName == null)
                    fontName = fontfamily.ToString();
                fontsStringBuilder.AppendLine(fontName);
            }

            // 更新 UI
            textBox_fonts.Text = fontsStringBuilder.ToString();
            textBlock_fonts_count.Text = "共" + fontFamilies.Count() + "个字体";
        }
        public string GetFontName(FontFamily fontfamily)
        {
            LanguageSpecificStringDictionary lsd = fontfamily.FamilyNames;
            string fontname;
            // 检查是否包含中文
            if (lsd.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
            {
                // 如果包含中文，则尝试获取中文的字体名称
                if (lsd.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out fontname))
                {
                    return fontname; // 返回中文字体名称
                }
            }
            else
            {
                // 否则，尝试获取英文的字体名称
                if (lsd.TryGetValue(XmlLanguage.GetLanguage("en-us"), out fontname))
                {
                    return fontname; // 返回英文字体名称
                }
            }

            // 如果没有找到对应的字体名称，则返回 null
            return null;
        }

        private void button_copy_fonts_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox_fonts.Text);
            HandyControl.Controls.MessageBox.Show("已复制到剪贴板！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetFontsStr();
        }
    }
}
