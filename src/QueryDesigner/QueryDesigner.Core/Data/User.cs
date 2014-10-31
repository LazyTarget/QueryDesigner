using System.Collections.Generic;

namespace QueryDesigner.Core
{
    public class User
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public virtual List<Connection> Connections { get; set; } 
    }
}
