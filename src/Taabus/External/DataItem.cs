using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;
using Taabus.UserInterface;

namespace Taabus.External
{
    [Serializer.Enable]
    public abstract class DataItem : DumpableObject
    {
        internal abstract IControlledItem Internalize(Internalizer controller);
    }

    [Serializer.Enable]
    public sealed class CardItem : DataItem
    {
        public Link Data;
        internal override IControlledItem Internalize(Internalizer controller) { return new CardView(controller.Execute(Data)); }
    }

    [Serializer.Enable]
    public sealed class TableVieItem : DataItem
    {
        public Link Data;
        internal override IControlledItem Internalize(Internalizer controller) { throw new NotImplementedException(); }
    }

    [Serializer.Enable]
    public abstract class Link: DumpableObject
    {
        internal abstract IControlledItem Internalize(Internalizer internalizer);
    }

    [Serializer.Enable]
    public sealed class Id :Link 
    {
        public int Value;
        internal override IControlledItem Internalize(Internalizer internalizer) { return internalizer.Id(Value); }
    }
}