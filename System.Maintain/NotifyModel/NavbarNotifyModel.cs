using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Maintain.NotifyModel
{
    public class NavbarNotifyModel : ObservableObject
    {
        private int _Key;
        public int Key
        {
            get => _Key;
            set => SetProperty(ref _Key, value);
        }
        private string _Value;
        public string Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }
    }
}
