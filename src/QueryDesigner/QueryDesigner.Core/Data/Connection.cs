using System.Collections.Generic;

namespace QueryDesigner.Core
{
    public class Connection
    {
        public long ID { get; set; }
        public string ConnectionString { get; set; }
        public virtual List<User> Users { get; set; }

    }
}
