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
    sealed class Travel
    {
        readonly string _title;
        public BahnEvent[] Steps;
        public Travel(string title)
        {
            _title = title;
            Date = DateTime.Today;
        }
        public DateTime Date;
        public Calendar ToCalendar { get { return new Calendar {Events = Steps.Select(ToEvent).ToArray()}; } }

        Event ToEvent(BahnEvent step, int i)
        {
            var startTime = step.DateTime + TimeSpan;
            var result = new Event
            {
                StartTime = startTime,
                EndTime = startTime,
                Title = step.Title,
                UIDHead = i + _title,
                Location = step.Station,
                TimeCreated = DateTime.Now 
            };

            if(i + 1 < Steps.Length)
            {
                result.EndTime = Steps[i + 1].DateTime;
                result.Title = result.Title.ReplaceArgs(Steps[i + 1].Platform);
            }

            return result;
        }

        TimeSpan TimeSpan { get { return Date - DateTime.Today; } }
        internal static DateTime ToDateTime(int start)
        {
            var minutes = start % 100;
            start /= 100;
            var hours = start % 100;
            start /= 100;
            return DateTime.Today + new TimeSpan(start, hours, minutes, 0);
        }
        public void ReplendishAndCheck()
        {
            for(var i = 0; i < Steps.Length; i++)
            {
                var step = Steps[i];
                var iOther = step.IsStart ? i - 1 : i + 1;
                if (iOther >= 0 && iOther < Steps.Length)
                {
                    var other = Steps[iOther];
                    if(step.Station == null)
                        step.Station = other.Station;
                    if(other.Station == null)
                        other.Station = step.Station;
                    Tracer.Assert(other.Station == step.Station);
                }
                if(i > 0)
                    Tracer.Assert(Steps[i - 1].DateTime < step.DateTime);
            }
        }
    }
}