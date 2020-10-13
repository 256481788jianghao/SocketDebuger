using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SocketDebuger
{
    class TcpClient
    {
        public Guid Id;

        Socket m_SocketClient;
        IPAddress m_IPAddress;
        IPEndPoint m_IPEndPoint;
        bool m_Connected = false;
        bool m_IsSending = false;

        List<Byte> m_SendBuffer;

        Task m_ReceiveTask;
        bool m_ReceiveTaskRunLoop = true;

        byte[] m_ReceiveBufferArray = new byte[1024];

        object m_objReceiveFilter = null;
        object m_objReceiveFilterFunction = null;

        public string ReceiveFilterPath { get; set; }

        public bool IsUseReceiveFilter()
        {
            return ReceiveFilterPath != null && ReceiveFilterPath.Length > 0;
        }

        public void SetReceiveFilter(object filter,object filterFun)
        {
            m_objReceiveFilter = filter;
            m_objReceiveFilterFunction = filterFun;
        }

        public TcpClient(Guid id, string ip,int port)
        {
            Id = id;
            m_IPAddress = IPAddress.Parse(ip);
            m_IPEndPoint = new IPEndPoint(m_IPAddress, port);
        }

        private void ReceiveCallBack()
        {
            while (m_ReceiveTaskRunLoop)
            {
                if (IsConnect())
                {
                    int receivelen = m_SocketClient.Receive(m_ReceiveBufferArray);
                    GVL.Context.TextBox_ReceiceData_Text = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ff")+": [";
                    for (int index = 0; index < receivelen; index++)
                    {
                        GVL.Context.TextBox_ReceiceData_Text += m_ReceiveBufferArray[index].ToString();
                    }
                    GVL.Context.TextBox_ReceiceData_Text += "]\n";
                }
            }
        }

        public IPEndPoint GetIPEndPoint()
        {
            return m_IPEndPoint;
        }

        public bool IsConnect()
        {
            return m_Connected;
        }

        public bool IsSending()
        {
            return m_IsSending;
        }

        public void SetSendBuffer(string datastr,bool isHex)
        {
            try
            {
                if (!isHex)
                {
                    byte[] ans = System.Text.Encoding.UTF8.GetBytes(datastr);
                    m_SendBuffer = new List<byte>(ans);
                }
                else
                {
                    string[] ansArray = datastr.Split(',');
                    m_SendBuffer = new List<byte>();
                    foreach(string item in ansArray)
                    {
                        Byte num = Byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
                        m_SendBuffer.Add(num);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SendData()
        {
            try
            {
                m_SocketClient.BeginSend(m_SendBuffer.ToArray(), 0, m_SendBuffer.Count, SocketFlags.None, new AsyncCallback(SendCallBack), this);
                m_IsSending = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            TcpClient item = ar.AsyncState as TcpClient;
            try
            {
                m_SocketClient.EndSend(ar);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                m_IsSending = false;
            }
        }

        public void Connect()
        {
            try
            {
                m_SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_SocketClient.BeginConnect(m_IPEndPoint,new AsyncCallback(ConnectCallBack),this);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            TcpClient item = ar.AsyncState as TcpClient;

            try
            {
                item.m_SocketClient.EndConnect(ar);
                m_Connected = true;
                GVL.Context.Button_Connect_Enable = false;
                GVL.Context.Button_Close_Enable = true;
                m_ReceiveTaskRunLoop = true;
                m_ReceiveTask = new Task(ReceiveCallBack);
                m_ReceiveTask.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Can not connect ex:"+ex.ToString());
            }
        }

        public void DisConnect()
        {
            try
            {
                m_SocketClient.BeginDisconnect(false, new AsyncCallback(DisConnectCallBack), this);
            }
            catch(Exception ex)
            {
                throw ex;
            }  
        }

        private void DisConnectCallBack(IAsyncResult ar)
        {
            TcpClient item = ar.AsyncState as TcpClient;

            try
            {
                m_SocketClient.EndDisconnect(ar);
                m_Connected = false;
                GVL.Context.Button_Connect_Enable = true;
                GVL.Context.Button_Close_Enable = false;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Can not disconnect ex:"+ex.ToString());
            }
        }

        private void Close()
        {
            try
            {
                m_SocketClient.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                m_SocketClient.Close();
                m_ReceiveTaskRunLoop = false;
            }
            
        }
    }
}
