#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SalesOrderManagerWPF.Properties;
using SalesOrderManagerWPF.ViewModels;
using SalesOrderManagerWPF.Views;

#endregion

namespace SalesOrderManagerWPF.Presenters
{
    public class FindDrawingPresenter
    {
        private readonly FindDrawingView _view;

        public FindDrawingPresenter(FindDrawingView view)
        {
            _view = view;
        }

        public async void SearchForFiles(string drawingNumber)
        {
            await Task.Factory.StartNew(() =>
            {
                var searchValue = drawingNumber.ToLower();

                var dirStack = new Stack<string>();
                dirStack.Push(Settings.Default.DrawingFileLocation);

                while (dirStack.Count > 0)
                {
                    var currentDir = dirStack.Pop();

                    var directories = Directory.GetDirectories(currentDir);

                    Array.ForEach(directories, d => dirStack.Push(d));

                    foreach (var file in Directory.GetFiles(currentDir).Where(f => f.ToLower().Contains(searchValue)))
                    {
                        var fi = new FileInfo(file);

                        var model = new FindDrawingViewModel
                        {
                            ShortFileName = fi.Name,
                            FileName = file,
                            CreatedAt = fi.CreationTime
                        };

                        _view.AddResult(model);
                    }
                }

                _view.SearchComplete();
            });
        }
    }
}