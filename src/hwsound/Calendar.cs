#region Copyright (C) 2013

//     Project hwsound
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
using HWClassLibrary.Debug;
using HWClassLibrary.Helper;

namespace main
{
    public sealed class Calendar
    {
        public Event[] Events;
    }

    public sealed class Event
    {
        static long _nextUid;
        static readonly string _uidBase = DateTime.Now.Ticks.ToString() ;
        string _uid;

        static string GetUID() { return _nextUid++ + _uidBase; }

        public DateTime StartTime;
        public DateTime EndTime;
        public DateTime TimeCreated;
        public string Location = string.Empty;
        public string Title = string.Empty;
        public string Description = string.Empty;
        public string UIDHead { get { return _uid??(_uid=GetUID()); } set { _uid = value; } }
        public string UID { get { return UIDHead + "@hahoyer.de"; } }
    }
}