using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketDebuger
{
    class ReceiveFilter
    {
        public string Filter(byte[] data,int datalen)
        {
            return datalen.ToString();
        }
    }
}
