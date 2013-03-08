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

namespace main
{
    sealed class BahnEvent
    {
        public readonly bool IsStart;
        public readonly string Platform;
        public DateTime DateTime;
        public string Station;
        public string Product;
        public BahnEvent(bool isStart, DateTime dateTime, string station, string platform)
        {
            IsStart = isStart;
            DateTime = dateTime;
            Station = station;
            Platform = platform;
        }


        public string Title
        {
            get
            {
                var result = Platform;
                if(!IsStart)
                    result += "->{0}";
                if(Product != null)
                    result += " " + Product;
                return result;
            }
        }
        public override string ToString()
        {
            return string.Format
                (
                    "IsStart: {0}, Platform: {1}, DateTime: {2}, Station: {3}, Product: {4}, Title: {5}",
                    IsStart,
                    Platform,
                    DateTime,
                    Station,
                    Product,
                    Title);
        }
    }
}