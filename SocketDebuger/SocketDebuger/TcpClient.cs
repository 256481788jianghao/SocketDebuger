using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketDebuger
{
    class TcpClient
    {
        public Guid Id;
        public delegate void TcpClientConnectHandle(object client);
        public event TcpClientConnectHandle TcpClientConnectEvent;

        Socket m_SocketClient;
        IPAddress m_IPAddress;
        IPEndPoint m_IPEndPoint;

        Task m_SendTask;
        Task m_ReceiveTask;

        public TcpClient(Guid id, string ip,int port)
        {
            Id = id;
            m_IPAddress = IPAddress.Parse(ip);
            m_IPEndPoint = new IPEndPoint(m_IPAddress, port);
            m_SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public IPEndPoint GetIPEndPoint()
        {
            return m_IPEndPoint;
        }

        public bool IsConnect()
        {
            return m_SocketClient.Connected;
        }

        public void Connect()
        {
            try
            {
                m_SocketClient.BeginConnect(m_IPEndPoint,new AsyncCallback(ConnectCallBack),null);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            TcpClientConnectEvent?.Invoke(this);
        }

        public void DisConnect()
        {
            try
            {
                m_SocketClient.Disconnect(false);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public void Close()
        {
            m_SocketClient.Close();
        }
    }
}
