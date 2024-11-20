using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_get_fonts.Messages
{
    internal class SetFontsMessage
    {
        public ObservableCollection<string> Fonts { get; set; }
        public SetFontsMessage(ObservableCollection<string> fonts)
        {
            Fonts = fonts;
        }

    }
}
