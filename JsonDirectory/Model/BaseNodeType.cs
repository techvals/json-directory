using JsonDirectoryNetCore.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDirectoryNetCore.Model
{
    public class BaseNodeType<T> : AbstractNodeType
    {
        [Name]
        public virtual string Name { get; set; }
        [Children]
        public virtual ICollection<T> Children { get; set; } = new List<T>();
    }
}
