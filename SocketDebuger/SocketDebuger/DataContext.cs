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

        public bool IsTcpClientSendingData { get; set; }

        public string TextBox_SendData_Text { get; set; }
        public bool CheckBox_SendData_IsHex { get; set; }
        public string TextBox_SendData_Timers_Text { get; set; } = "1";
        public string TextBox_SendData_Interval_Text { get; set; } = "20";

        public UInt32 Label_SendData_SendCount { get; set; } = 0;

        public string TextBox_ReceiceData_Text { get; set; }

        public string Label_Receive_Filter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
