using System.Linq;
using hw.Debug;

namespace Taabus.UserInterface
{
    [Serializer.Class]
    public sealed class ExpansionDescription
    {
        string _id;
        [Serializer.Member]
        public bool IsExpanded;
        [Serializer.Member]
        public ExpansionDescription[] Nodes;

        [Serializer.Member]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Tracer.Assert(_id == null || _id.Any());
            }
        }
    }
}