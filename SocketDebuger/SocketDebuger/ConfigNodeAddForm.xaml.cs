using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SocketDebuger
{
    /// <summary>
    /// ConfigNodeAddForm.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigNodeAddForm : Window
    {
        public delegate void AddConfigNodeHandle(ConfigTreeItem.NodeType type, string ip, int port);
        public event AddConfigNodeHandle AddConfigNodeEvent;
        public ConfigNodeAddForm()
        {
            InitializeComponent();
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ipstr = TextBox_Ip.Text;
                int port = Convert.ToInt32(TextBox_Port.Text);
                ConfigTreeItem.NodeType nType;
                switch (ComboBox_Type.SelectedIndex)
                {
                    case 0:nType = ConfigTreeItem.NodeType.TcpServer;break;
                    case 1: nType = ConfigTreeItem.NodeType.TcpClient; break;
                    case 2: nType = ConfigTreeItem.NodeType.UdpServer; break;
                    case 3: nType = ConfigTreeItem.NodeType.UdpClient; break;
                    default:
                        nType = ConfigTreeItem.NodeType.TcpServer;break;
                }
                AddConfigNodeEvent?.Invoke(nType, ipstr, port);
                MessageBox.Show("node add sucess");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
