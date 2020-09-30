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

        Task m_SendTask;
        Task m_ReceiveTask;

        public TcpClient(Guid id, string ip,int port)
        {
            Id = id;
            m_IPAddress = IPAddress.Parse(ip);
            m_IPEndPoint = new IPEndPoint(m_IPAddress, port);
        }

        public IPEndPoint GetIPEndPoint()
        {
            return m_IPEndPoint;
        }

        public bool IsConnect()
        {
            return m_Connected;
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
            }
            catch(Exception ex)
            {
                MessageBox.Show("Can not connect");
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
            }
            
        }
    }
}
