﻿using System;
using System.Linq;
using hw.DebugFormatter;

namespace Taabus.External
{
    [Serializer.Enable]
    public sealed class ExpansionDescription
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
    }
}