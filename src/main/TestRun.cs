#region Copyright (C) 2013

//     Project main
//     Copyright (C) 2013 - 2013 Harald Hoyer
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

#endregion

using System.Linq;
using System.Collections.Generic;
using System;
using hw.Debug;
using hw.Helper;

namespace main
{
    public abstract class TestRun
    {
        public interface ILogManager
        {
            void Paragraph();
            void Write(string message);
            void Indent();
            void UnIndent();
        }

        public bool ShowData;
        public ILogManager _logManager;
        public Func<ILogManager> GetLogManager = () => new TraceLogManager();

        sealed class TraceLogManager : ILogManager
        {
            int _indent;
            void ILogManager.UnIndent() { _indent--; }
            void ILogManager.Indent() { _indent++; }


            void ILogManager.Paragraph() { Write(("\n" + DateTime.Now.Format() + " ")); }
            void ILogManager.Write(string message) { Write(message); }
            
            void Write(string message) { Tracer.LinePart(message.Indent(_indent)); }
        }

    }

    public abstract class TestRun<TItem> : TestRun
    {
        public interface IData
        {
            TItem[] Data { get; }
            string Title { get; }
        }
    }

    public sealed class TestRun<TEnvironment, TItem> : TestRun<TItem>
        where TEnvironment : class
    {
        readonly ITest _test;

        public TestRun(ITest test) { _test = test; }
        public void Execute()
        {
            Tracer.Assert(_logManager == null);
            _logManager = GetLogManager();
            _logManager.Paragraph();
            _logManager.Write("Start of test " + _test.Title.Quote());
            _logManager.Indent();
            _logManager.Write("\n");

            if(ShowData)
            {
                var data = _test.Data;
                _logManager.Write("\nData groups (count = " + data.Length + ")");
                _logManager.Indent();
                for(var i = 0; i < data.Length; i++)
                {
                    var dataGroup = data[i];
                    _logManager.Write("\nGroup " + i + " Title: " + dataGroup.Title);
                    var length = dataGroup.Data.Length;
                    var size = length.ToString().Length;
                    for(var j = 0; j < length; j++)
                        _logManager.Write("\n" + j.ToString().PadRight(size, '0') + " " + dataGroup.Data[j]);
                    _logManager.Write("\n");
                }

                _logManager.UnIndent();
                _logManager.Write("\n");
            }

            Execute(new int[0], new TItem[0]);

            _logManager.UnIndent();
            _logManager.Paragraph();
            _logManager.Write("End of test\n\n");
            _logManager = null;
        }

        void Execute(int[] index, TItem[] data)
        {
            Tracer.Assert(_logManager != null);
            var position = index.Length;
            Tracer.Assert(position == data.Length);
            if(position == _test.Data.Length)
                Execute(index, _test.Environment(index, data));
            else
            {
                var testDataItems = _test.Data[position].Data;
                for(var subIndex = 0; subIndex < testDataItems.Length; subIndex++)
                    Execute(index.Union(new[] {subIndex}).ToArray(), data.Union(new[] {testDataItems[subIndex]}).ToArray());
            }
        }

        void Execute(int[] index, TEnvironment environment)
        {
            Tracer.Assert(_logManager != null);
            _logManager.Paragraph();
            if(environment == null)
            {
                _logManager.Write("Ignoring data combination " + index.Stringify(","));
                return;
            }
            try
            {
                _logManager.Write("Start data combination " + index.Stringify(","));
                Exception exception = null;
                try
                {
                    _logManager.Indent();
                    _test.Execute(environment);
                    _logManager.UnIndent();
                    _logManager.Paragraph();
                    _logManager.Write("successful\n");
                }
                catch(Exception e)
                {
                    exception = e;
                    _logManager.Paragraph();
                    _logManager.Write("exception: " + e + "\n");
                }
                _logManager.Indent();
                _test.Assess(index, environment, exception, _logManager);
                _logManager.UnIndent();
            }
            finally
            {
                _logManager.Indent();
                _test.TearDown(index, environment);
                _logManager.UnIndent();
            }
        }

        public interface ITest
        {
            IData[] Data { get; }
            string Title { get; }
            TEnvironment Environment(int[] index, TItem[] testDataItems);
            void Execute(TEnvironment environment);
            void Assess(int[] index, TEnvironment environment, Exception exception, ILogManager logManager);
            void TearDown(int[] index, TEnvironment environment);
        }
    }
}