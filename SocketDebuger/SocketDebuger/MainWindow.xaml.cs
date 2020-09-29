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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketDebuger
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigTreeItem node_TcpServer = new ConfigTreeItem("TcpServer");
        ConfigTreeItem node_TcpClient = new ConfigTreeItem("TcpClient");
        ConfigTreeItem node_UdpServer = new ConfigTreeItem("UdpServer");
        ConfigTreeItem node_UdpClient = new ConfigTreeItem("UdpClient");
        public MainWindow()
        {
            InitializeComponent();
            UpdateConfigNodes();


        }

        private void UpdateConfigNodes()
        {
            TreeView_Config.Items.Clear();
            TreeView_Config.Items.Add(node_TcpServer);
            TreeView_Config.Items.Add(node_TcpClient);
            TreeView_Config.Items.Add(node_UdpServer);
            TreeView_Config.Items.Add(node_UdpClient);
        }

        private void Button_ConfigCreate_Click(object sender, RoutedEventArgs e)
        {
            ConfigNodeAddForm form = new ConfigNodeAddForm();
            form.AddConfigNodeEvent += new ConfigNodeAddForm.AddConfigNodeHandle(AddConfigNodeHandle);
            form.Show();
        }

        private void Button_ConfigDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TreeView_Config_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }


        private void AddConfigNodeHandle(ConfigTreeItem.NodeType type, string ip, int port)
        {
            ConfigTreeItem newItem = new ConfigTreeItem();
            switch (type)
            {
                case ConfigTreeItem.NodeType.TcpServer:
                    {
                        newItem.Tag = ip + "[" + port.ToString() + "]";
                        newItem.NType = type;
                        node_TcpServer.Children.Add(newItem);
                        break;
                    }
                case ConfigTreeItem.NodeType.TcpClient:
                    {
                        newItem.Tag = ip + "[" + port.ToString() + "]";
                        newItem.NType = type;
                        node_TcpClient.Children.Add(newItem);
                        break;
                    }
                case ConfigTreeItem.NodeType.UdpServer:
                    {
                        newItem.Tag = ip + "[" + port.ToString() + "]";
                        newItem.NType = type;
                        node_UdpServer.Children.Add(newItem);
                        break;
                    }
                case ConfigTreeItem.NodeType.UdpClient:
                    {
                        newItem.Tag = ip + "[" + port.ToString() + "]";
                        newItem.NType = type;
                        node_UdpClient.Children.Add(newItem);
                        break;
                    }
            }
            UpdateConfigNodes();
        }
    }
}
