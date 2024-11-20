using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using wpf_get_fonts.Constants;
using wpf_get_fonts.Messages;

namespace wpf_get_fonts.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<string> fonts;

        public MainWindowViewModel()
        {
            CopyFontsCommand = new RelayCommand<CopyFontMode>(copyFonts);

            WeakReferenceMessenger.Default.Register<SetFontsMessage>(this, (recipient, message) =>
            {
                Fonts = message.Fonts;
            });
            WeakReferenceMessenger.Default.Register<CopyFontsToClipboardMessage>(this, (recipient, message) =>
            {
                // 使用LINQ对集合进行排序，包含中文的名称优先
                var sortedFonts = fonts.OrderBy(f => f.Any(c => c >= '\u4e00' && c <= '\u9fa5') ? 0 : 1) // 检查是否包含中文字符
                                       .ThenBy(f => f) // 然后按名称排序
                                       .ToList();
                Clipboard.SetText(string.Join("\n", sortedFonts));
            });
        }
        private string previewText = "眼里有光，心中有梦   Follow Your Heart ";

        public string PreviewText
        {
            get { return previewText; }
            set => SetProperty(ref previewText, value);
        }

        public ObservableCollection<string> Fonts
        {
            get => fonts;
            set => SetProperty(ref fonts, value);
        }

        public RelayCommand<CopyFontMode> CopyFontsCommand { get; private set; }
        private void copyFonts(CopyFontMode type)
        {
            var sortedFonts = new List<string>();
            var hint = "全部字体";
            switch (type)
            {
                case CopyFontMode.Default:
                    sortedFonts = fonts.ToList();
                    break;
                case CopyFontMode.ChineseFirst:
                    hint = "全部字体(中文优先)";
                    // 使用LINQ对集合进行排序，包含中文的名称优先
                    sortedFonts = fonts.OrderBy(f => f.Any(c => c >= '\u4e00' && c <= '\u9fa5') ? 0 : 1) // 检查是否包含中文字符
                                       .ThenBy(f => f) // 然后按名称排序
                                       .ToList();
                    break;
                case CopyFontMode.EnglishFirst:
                    hint = "全部字体(英文优先)";
                    // 使用LINQ对集合进行排序，不包含中文的名称优先
                    sortedFonts = fonts.OrderBy(f => f.Any(c => c >= '\u4e00' && c <= '\u9fa5') ? 1 : 0) // 检查是否包含中文字符
                                       .ThenBy(f => f) // 然后按名称排序
                                       .ToList();
                    break;
                case CopyFontMode.ChineseOnly:
                    hint = "中文字体";
                    // 仅包含中文的名称
                    sortedFonts = fonts.Where(f => f.Any(c => c >= '\u4e00' && c <= '\u9fa5')).ToList();
                    break;
                case CopyFontMode.EnglishOnly:
                    hint = "英文字体";
                    // 仅包含英文的名称
                    sortedFonts = fonts.Where(f => !f.Any(c => c >= '\u4e00' && c <= '\u9fa5')).ToList();
                    break;
                default:
                    Clipboard.SetText(string.Join("\n", sortedFonts));
                    return;
            }

            // 将结果复制到剪贴板
            Clipboard.SetText(string.Join("\n", sortedFonts));
            HandyControl.Controls.Growl.Success(hint + " 已复制");
        }
    }
}
