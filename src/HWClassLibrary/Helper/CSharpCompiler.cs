// 
//     Project HWClassLibrary
//     Copyright (C) 2011 - 2012 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using HWClassLibrary.Debug;
using HWClassLibrary.IO;
using HWClassLibrary.UnitTest;
using Microsoft.CSharp;

namespace HWClassLibrary.Helper
{
    static class CSharpCompiler
    {
        public static object Exec(string fileName, string namespaceName, string typeName, string methodName, params object[] args)
        {
            // Build the parameters for source compilation.
            var cp = new CompilerParameters {GenerateInMemory = true, IncludeDebugInformation = true, CompilerOptions = "/TargetFrameworkVersion=v3.5"};
            cp.ReferencedAssemblies.AddRange(new[] {"System.dll", "NUnit.Framework.dll", "HWClassLibrary.dll"});
            var cr = new CSharpCodeProvider().CompileAssemblyFromFile(cp, fileName);
            if(cr.Errors.Count > 0)

            {
                foreach(var error in cr.Errors)
                    Tracer.Line(error.ToString());

                throw new CSharpCompilerErrors(cr.Errors);
            }
            var methodInfo = FindMethod(cr.CompiledAssembly, namespaceName, typeName, methodName);
            return methodInfo.Invoke(null, args);
        }

        static MethodInfo FindMethod(_Assembly assembly, string namespaceName, string typeName, string methodName)
        {
            var type = FindType(assembly, namespaceName, typeName);
            return FindMethod(type, methodName);
        }

        static Type FindType(_Assembly assembly, string namespaceName, string typeName)
        {
            if(namespaceName != "?" && typeName != "?")
            {
                var typeFullName = typeName;
                if(namespaceName != "")
                    typeFullName = namespaceName + "." + typeName;
                return assembly.GetType(typeFullName);
            }
            var types = assembly.GetTypes();
            return types.FirstOrDefault(t => IsMatch(t, namespaceName, typeName));
        }

        static bool IsMatch(Type type, string namespaceName, string typeName)
        {
            if(namespaceName != "?")
                return type.Namespace == namespaceName;
            return type.Name == typeName;
        }

        static MethodInfo FindMethod(Type type, string methodName)
        {
            if(methodName == "?")
                return type.GetMethods()[0];
            return type.GetMethod(methodName);
        }
    }

    sealed class CSharpCompilerErrors : Exception
    {
        public CompilerErrorCollection CompilerErrorCollection { get { return _compilerErrorCollection; } }
        readonly CompilerErrorCollection _compilerErrorCollection;

        public CSharpCompilerErrors(CompilerErrorCollection compilerErrorCollection) { _compilerErrorCollection = compilerErrorCollection; }
    }

    [TestFixture]
    public sealed class Test
    {
        /// <summary>
        ///     Special test, will not work automatically.
        /// </summary>
        /// created 08.10.2006 16:33
        //[Test]
        public void TestMethod()
        {
            var x1 = CSharpCompiler.Exec(File.SourceFileName(0), "?", "?", "?", null);
            var x2 = CSharpCompiler.Exec(File.SourceFileName(0), "?", "?", "?", null);
            var x3 = CSharpCompiler.Exec(File.SourceFileName(0), "?", "?", "?", null);
            var x4 = CSharpCompiler.Exec(File.SourceFileName(0), "?", "?", "?", null);
            var x5 = CSharpCompiler.Exec(File.SourceFileName(0), "?", "?", "?", null);
        }
    }
}