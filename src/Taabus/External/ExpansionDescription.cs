using System;
using System.Linq;
using hw.Debug;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class ExpansionDescription : IEquatable<ExpansionDescription>
    {
        string _id;
        public bool IsExpanded;
        public ExpansionDescription[] Nodes;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Tracer.Assert(_id == null || _id.Any());
            }
        }

        bool IEquatable<ExpansionDescription>.Equals(ExpansionDescription other)
        {
            return Id == other.Id
                && IsExpanded == other.IsExpanded
                && Extension.Equals(Nodes, other.Nodes);
        }
    }

    public class Item
    {
    }

}