using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using hw.Helper;
using Taabus.External;

namespace Taabus
{
    sealed class PersistentConfiguration : PersistenceController<Configuration>
    {
        PersistentConfiguration(string fileName, Configuration configuration)
            : base(fileName, configuration) { }

        protected override string Serialize(Configuration data) { return InternalSerialize(data); }

        internal static PersistentConfiguration Load(string fileName)
        {
            if(fileName == null)
                return null;

            return new PersistentConfiguration(fileName, InternalLoad(fileName));
        }

        internal static string CreateEmpty() { return InternalSerialize(new Configuration()); }

        static Configuration InternalLoad(string fileName)
        {
            var assembly = fileName.CreateAssemblyFromFile();
            var type = assembly.GetType("V13");
            if(type != null)
                return type.Invoke<Configuration>("Data");

            // older version
            var type1 = assembly.GetType("TaabusProject");
            if(type1 != null)
                return Convert(type1.Invoke<TaabusProject>("Project"));

            Tracer.FlaggedLine("Error: Unexpected configuration\nTypes expected: \"CSharp\", \"TaabusProject\"\nType(s) found: " + assembly.GetTypes().Select(t => t.PrettyName().Quote()).Stringify(", "));
            Tracer.TraceBreak();
            return null;
        }

        static Configuration Convert(TaabusProject target)
        {
            return new Configuration
            {
                ExpansionDescriptions = target.ExpansionDescriptions,
                Selection = target.Selection,
                Servers = target.Project.Servers.ToArray(),
                Items = new Item[0]
            };
        }

        static string InternalSerialize(Configuration data) { return new Generator(data).TransformText(); }
    }

    [Serializer.Enable]
    public sealed class TaabusProject
    {
        public Project Project;
        public ExpansionDescription[] ExpansionDescriptions;
        public string[] Selection;
    }
}