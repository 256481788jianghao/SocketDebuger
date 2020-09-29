using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketDebuger
{
    public class ConfigTreeItem
    {
        public string Tag { get; set; }
        public NodeType NType { get; set; }
        public List<ConfigTreeItem> Children { get; set; }

        public ConfigTreeItem(string tag)
        {
            Tag = tag;
            Children = new List<ConfigTreeItem>();
        }

        public ConfigTreeItem()
        {
            Children = new List<ConfigTreeItem>();
        }

        public enum NodeType
        {
            TcpServer,
            TcpClient,
            UdpServer,
            UdpClient
        }
    }
}
