using JsonDirectoryNetCore.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDirectoryNetCore.Model
{
    public class NodeType : BaseNodeType<NodeType>
    {
        [Name]
        public override string Name { get; set; }
        [Children]
        public override ICollection<NodeType> Children { get; set; } = new List<NodeType>();
    }
}
