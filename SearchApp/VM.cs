using SearchApp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SearchApp
{
    public class VM : INotifyPropertyChanged
    {
        BackgroundWorker _backgroundWorker;
        ObservableCollection<string> _items = new ObservableCollection<string>();
        internal Action SearchEnded;

        public VM()
        {
            _items = new ObservableCollection<string>();
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchEnded?.Invoke();
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Cancel = Search(SearchPath, ProcessFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        public ObservableCollection<string> Items
        {
            get
            {
                return _items;
            }
        }

        public string SearchPath { get; set; }

        public string FileName { get; set; }


        internal void CancelAsync()
        {
            _backgroundWorker.CancelAsync();
        }

        public void RunSearch()
        {
            _items.Clear();
            _backgroundWorker.RunWorkerAsync();
        }

        void ProcessFile(string path)
        {
            _items.AddOnUI(path);
        }

        public bool Search(string folder, Action<string> fileAction)
        {
            if (!Directory.Exists(folder))
            {
                return true;
            }

            foreach (string file in Directory.GetFiles(folder, FileName))
            {
                if (_backgroundWorker.CancellationPending == true)
                {
                    return true;
                }
                fileAction(file);
            }
            foreach (string subDir in Directory.GetDirectories(folder))
            {
                if (_backgroundWorker.CancellationPending == true)
                {
                    return true;
                }
                try
                {
                    var res = Search(subDir, fileAction);
                    if(res)
                    {
                        return true;
                    }
                    else
                    {
                        var t = 4;
                    }
                }
                catch
                {
                    // swallow, log, whatever
                }
            }

            return false;

        }

    }
}
