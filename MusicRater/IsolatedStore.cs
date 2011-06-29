﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;

namespace MusicRater
{
    public class IsolatedStore : IIsolatedStore
    {
        private readonly IsolatedStorageFile isoStore;

        public IsolatedStore()
        {
            this.isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public bool FileExists(string fileName)
        {
            return isoStore.FileExists(fileName);
        }

        public Stream CreateFile(string fileName)
        {
            return isoStore.OpenFile(fileName, FileMode.Create);
        }

        public Stream OpenFile(string fileName)
        {
            return isoStore.OpenFile(fileName, FileMode.Open);
        }

        public void Dispose()
        {
            this.isoStore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
