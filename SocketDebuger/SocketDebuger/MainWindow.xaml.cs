using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace SocketDebuger
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigTreeItem node_TcpServer = new ConfigTreeItem("TcpServer", true);
        ConfigTreeItem node_TcpClient = new ConfigTreeItem("TcpClient", true);
        ConfigTreeItem node_UdpServer = new ConfigTreeItem("UdpServer", true);
        ConfigTreeItem node_UdpClient = new ConfigTreeItem("UdpClient", true);
        ConfigTreeItem node_cur_select = null;

        List<TcpClient> m_TcpClientList = new List<TcpClient>();
        public MainWindow()
        {
            InitializeComponent();
            UpdateConfigNodes();

            this.DataContext = GVL.Context;
        }

        private void UpdateConfigNodes()
        {
            node_cur_select = null;
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
            if(node_cur_select != null && !node_cur_select.IsRootNode)
            {
                switch (node_cur_select.NType)
                {
                    case ConfigTreeItem.NodeType.TcpServer:
                        {
                            node_TcpServer.Children.RemoveAll(it => it == node_cur_select);
                            break;
                        }
                    case ConfigTreeItem.NodeType.TcpClient:
                        {
                            TcpClient client = m_TcpClientList.Find(it => it.Id == node_cur_select.Id);
                            if(client != null && client.IsConnect())
                            {
                                client.DisConnect();
                            }
                            m_TcpClientList.RemoveAll(it => it.Id == node_cur_select.Id);
                            node_TcpClient.Children.RemoveAll(it => it == node_cur_select);
                            break;
                        }
                    case ConfigTreeItem.NodeType.UdpServer:
                        {
                            node_UdpServer.Children.RemoveAll(it => it == node_cur_select);
                            break;
                        }
                    case ConfigTreeItem.NodeType.UdpClient:
                        {
                            node_UdpClient.Children.RemoveAll(it => it == node_cur_select);
                            break;
                        }
                }
                UpdateConfigNodes();
            }
        }

        private void TreeView_Config_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            node_cur_select = TreeView_Config.SelectedItem as ConfigTreeItem;
            if (node_cur_select != null && !node_cur_select.IsRootNode)
            {
                switch (node_cur_select.NType)
                {
                    case ConfigTreeItem.NodeType.TcpClient:
                        {
                            Label_TYPE.Content = node_cur_select.NType.ToString();
                            TcpClient client = m_TcpClientList.Find(it => it.Id == node_cur_select.Id);
                            if (client != null)
                            {
                                Label_IP.Content = client.GetIPEndPoint().Address.ToString();
                                Label_PORT.Content = client.GetIPEndPoint().Port.ToString();
                                GVL.Context.IsTcpClientSendingData = client.IsSending();
                                if (client.IsConnect())
                                {
                                    GVL.Context.Button_Connect_Enable = false;
                                    GVL.Context.Button_Close_Enable = true;
                                }
                                else
                                {
                                    GVL.Context.Button_Connect_Enable = true;
                                    GVL.Context.Button_Close_Enable = false;
                                }
                            }
                            break;
                        }
                }
            }
            
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
                        TcpClient client = new TcpClient(newItem.Id, ip, port);
                        m_TcpClientList.Add(client);
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

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach(TcpClient client in m_TcpClientList)
            {
                if (client.IsConnect())
                {
                    client.DisConnect();
                }
            }
        }
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (node_cur_select != null && !node_cur_select.IsRootNode)
            {
                switch (node_cur_select.NType)
                {
                    case ConfigTreeItem.NodeType.TcpClient:
                        {
                            TcpClient client = m_TcpClientList.Find(it => it.Id == node_cur_select.Id);
                            if(client != null)
                            {
                                if (!client.IsConnect())
                                {
                                    try
                                    {
                                        client.Connect();
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    } 
                                }
                            }
                            break;
                        }
                }
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            if (node_cur_select != null && !node_cur_select.IsRootNode)
            {
                switch (node_cur_select.NType)
                {
                    case ConfigTreeItem.NodeType.TcpClient:
                        {
                            TcpClient client = m_TcpClientList.Find(it => it.Id == node_cur_select.Id);
                            if (client != null)
                            {
                                if (client.IsConnect())
                                {
                                    try
                                    {
                                        client.DisConnect();
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    } 
                                }
                            }
                            break;
                        }
                }
            }
        }

        DispatcherTimer m_SendDatTimer = new DispatcherTimer();
        UInt32 m_SendStopCountNum = 0;
        private void Button_StartSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GVL.Context.Label_SendData_SendCount = 0;
                m_SendDatTimer.Tick += new EventHandler(SendTimerCallBack);
                int interval = Convert.ToInt32(GVL.Context.TextBox_SendData_Interval_Text);
                m_SendStopCountNum = Convert.ToUInt32(GVL.Context.TextBox_SendData_Timers_Text);
                m_SendDatTimer.Interval = TimeSpan.FromMilliseconds(interval*50);
                m_SendDatTimer.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_StopSend_Click(object sender, RoutedEventArgs e)
        {
            m_SendDatTimer.Stop();
        }

        private void SendTimerCallBack(object sender,EventArgs e)
        {
            if(GVL.Context.Label_SendData_SendCount < m_SendStopCountNum)
            {
                if (node_cur_select != null && !node_cur_select.IsRootNode)
                {
                    switch (node_cur_select.NType)
                    {
                        case ConfigTreeItem.NodeType.TcpClient:
                            {
                                TcpClient client = m_TcpClientList.Find(it => it.Id == node_cur_select.Id);
                                if (client != null)
                                {
                                    if (client.IsConnect())
                                    {
                                        try
                                        {
                                            string SendDataStr = GVL.Context.TextBox_SendData_Text;
                                            client.SetSendBuffer(SendDataStr, GVL.Context.CheckBox_SendData_IsHex);
                                            client.SendData();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                GVL.Context.Label_SendData_SendCount++;
            }
        }

        object m_objReceiveFilter = null;
        object m_objReceiveFilterFunction = null;
        private void Button_AddFilter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                GVL.Context.Label_Receive_Filter = dialog.FileName;
                string codestr = File.ReadAllText(GVL.Context.Label_Receive_Filter);
                SKCSharpCompilter compliter = new SKCSharpCompilter();
                compliter.SetCodeStr(codestr);
                try
                {
                    Assembly assemblycode = compliter.Compile();
                    m_objReceiveFilter = assemblycode.CreateInstance("ReceiveFilter");
                    m_objReceiveFilterFunction = m_objReceiveFilter.GetType().GetMethod("Filter");
                    //string ans = (string)objMI.Invoke(objReceiveFilter, new object[] { new byte[] { 2,2},2});
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
