using hw.DebugFormatter;
using hw.Forms;

namespace Taabus.Data
{
    public abstract class NamedObject : DumpableObject, INodeNameProvider, IAdditionalNodeInfoProvider
    {
        [DisableDump]
        public abstract string Name { get; }

        string INodeNameProvider.Value(string name) { return Name; }
        string IAdditionalNodeInfoProvider.AdditionalNodeInfo { get { return Name; } }
    }
}