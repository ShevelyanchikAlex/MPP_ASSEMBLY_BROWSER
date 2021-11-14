using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AssemblyBrowserCore;
using AssemblyBrowserCore.Model;
using Microsoft.Win32;

namespace AssemblyBrowserView
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string FailedAssemblyLoad = "An error has occurred!\nAssembly was not loaded!";

        public event PropertyChangedEventHandler PropertyChanged;
        public string FilePath { get; private set; }
        public List<NamespaceInfo> Namespaces { get; set; }

        private BrowserCommand _openFileCommand;

        public BrowserCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??= new BrowserCommand(obj =>
                {
                    try
                    {
                        var openFileDialog = new OpenFileDialog();
                        if (openFileDialog.ShowDialog() != true) return;
                        FilePath = openFileDialog.FileName;
                        Namespaces = AssemblyBrowserCoreImpl.GetAssemblyData(FilePath);
                        OnPropertyChanged(nameof(Namespaces));
                        OnPropertyChanged(nameof(FilePath));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(FailedAssemblyLoad);
                    }
                });
            }
        }


        private void OnPropertyChanged(string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}