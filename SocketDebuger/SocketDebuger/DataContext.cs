using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SocketDebuger
{
    [AddINotifyPropertyChangedInterface]
    public class DataContext:INotifyPropertyChanged
    {
        public bool Button_Connect_Enable { get; set; }
        public bool Button_Close_Enable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
