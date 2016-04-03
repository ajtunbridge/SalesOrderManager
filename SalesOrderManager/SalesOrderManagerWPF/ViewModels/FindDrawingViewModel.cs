using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SalesOrderManagerWPF.ViewModels
{
    public class FindDrawingViewModel
    {
        public bool FoundInCpeCentral { get; set; }
        public string ShortFileName { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public BitmapSource Icon { get; set; }
    }
}
