using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketDebuger
{
    public class ConfigTreeItem
    {
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public NodeType NType { get; set; }
        public List<ConfigTreeItem> Children { get; set; }

        public bool IsRootNode { get; set; }

        public ConfigTreeItem(string tag):this()
        {
            Tag = tag;
        }

        public ConfigTreeItem(string tag, bool isRootNode) : this(tag)
        {
            IsRootNode = isRootNode;
        }

        public ConfigTreeItem()
        {
            IsRootNode = false;
            Children = new List<ConfigTreeItem>();
            Id = new Guid();
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
