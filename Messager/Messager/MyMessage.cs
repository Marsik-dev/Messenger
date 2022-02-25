using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
    public class MyMessage : IComparable
    {

        public bool isUser;
        public string text;
        public byte[] image;
        public DateTime time;
        public String SenderName;

        public int CompareTo(object obj) => time.CompareTo(((MyMessage)obj).time);
        
    }
}
