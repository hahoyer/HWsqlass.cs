﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HWClassLibrary.Debug;
using HWClassLibrary.Helper;
using HWClassLibrary.IO;
using HWClassLibrary.Relation;

namespace HWClassLibrary.UnitTest
{
    public sealed class TestRunner : Dumpable
    {
        private readonly TestType[] _testTypes;
        public static bool IsModeErrorFocus;
        private readonly File _configFile = File.m("Test.HWconfig");
        private string _status = "Start";
        private int _complete;
        private string _currentMethodName = "";

        private TestRunner(IEnumerable<TestType> testTypes)
        {
            _testTypes = testTypes.ToArray();
            Tracer.Assert(_testTypes.IsCircuidFree(Dependants), () => Tracer.Dump(_testTypes.Circuids(Dependants).ToArray()));
            if(IsModeErrorFocus)
                LoadConfiguration();
        }

        internal static void RunTests(Assembly rootAssembly) { new TestRunner(GetUnitTestTypes(rootAssembly)).Run(); }

        private TestType[] Dependants(TestType type)
        {
            if(IsModeErrorFocus)
                return new TestType[0];
            return type
                .Dependants
                .Select(attribute => attribute.AsTestType(_testTypes))
                .ToArray();
        }

        private void Run()
        {
            _status = "run";
            while(RunLevel())
                continue;
            _status = "ran";
            SaveConfiguration();
        }

        private bool RunLevel()
        {
            var openTests = _testTypes.Where(x => x.IsStartable).ToArray();
            if(openTests.Length == 0)
                return false;

            foreach(var openTest in openTests)
            {
                var dependants = Dependants(openTest);
                if(dependants.All(test => test.IsStarted))
                {
                    openTest.IsStarted = true;
                    if(dependants.All(test => test.IsSuccessfull))
                    {
                        if(!IsModeErrorFocus)
                        {
                            _currentMethodName = openTest.Type.FullName;
                            SaveConfiguration();
                            _currentMethodName = "";
                        }
                        openTest.Run();
                        _complete++;
                    }
                }
            }
            return true;
        }

        private string ConfigurationString
        {
            get
            {
                return HeaderText + "\n" +
                    _testTypes
                    .OrderBy(t => t.ConfigurationModePriority)
                    .Aggregate("", (current, testType) => current + testType.ConfigurationString);
            }
            set
            {
                if (value == null)
                    return;
                var pairs = value.Split('\n')
                    .Where((line,i) => i > 0 && line != "")
                    .Join(_testTypes, line => line.Split(' ')[1], type => type.Type.FullName, (line, type) => new {line, type});
                foreach(var pair in pairs)
                    pair.type.ConfigurationString = pair.line;
            }
        }

        private string HeaderText { get { return DateTime.Now.Format() + " " + _status + " " + _complete + " of " + _testTypes.Length + " " + _currentMethodName; } }

        private void SaveConfiguration()
        {
            _configFile.String = ConfigurationString;
            ConfigFileMessage("Configuration saved");
        }

        private void ConfigFileMessage(string flagText) { Tracer.Line(Tracer.FilePosn(_configFile.FullName, 1, 1, flagText)); }


        private void LoadConfiguration()
        {
            ConfigurationString = _configFile.String;
            ConfigFileMessage("Configuration loaded");
        }

        private static IEnumerable<TestType> GetUnitTestTypes(Assembly rootAssembly)
        {
            return rootAssembly
                .GetReferencedTypes()
                .Where(type => !(type.IsAbstract || type.GetAttribute<TestFixtureAttribute>(true) == null))
                .Select(methodInfo => new TestType(methodInfo));
        }
    }

    internal sealed class TestFailedException : Exception
    {}
}