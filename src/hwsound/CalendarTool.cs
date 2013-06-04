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
    class CalendarTool
    {
        public static void Run()
        {

            /*
            Köln Hbf 08.03. ab 17:55 6
            F-Flughafen Fernbf. 08.03. an 18:50 Fern 5
            ICE 613 1 Sitzplatz, Wg. 39, Pl. 106, 1 Fenster,
            Panorama / Lounge, Nichtraucher, Ruhebereich
            F-Flughafen Fernbf. 08.03. ab 19:02 Fern 4
            Leipzig Hbf 08.03. an 22:55 10
            ICE 1657 1 Sitzplatz, Wg. 38, Pl. 12, 1 Fenster,
            Panorama / Lounge, Nichtraucher, Ruhebereich
            Ihre Reiseverbindung und Reservierung Rückfahrt am 12.03.2013
            Halt Datum Zeit Gleis Fahrt Reservierung
            Leipzig Hbf 12.03. ab 13:11 10
            F-Flughafen Fernbf. 12.03. an 16:55 Fern 7
            ICE 1558 1 Sitzplatz, Wg. 28, Pl. 16, 1 Fenster,
            Panorama / Lounge, Nichtraucher, Ruhebereich
            F-Flughafen Fernbf. 12.03. ab 17:09 Fern 6
            Köln Hbf 12.03. an 18:05 4
            ICE 516 1 Sitzplatz, Wg. 39, Pl. 106, 1 Fenster,
            Panorama / Lounge, Nichtraucher, Ruhebereich */

            var travel = new Travel("Leipzig Juni 2013 Hin")
            {
                Steps = new[]
                {
                    Start(1155, 6, "Koln", product: "ICE517 P29/31"),
                    End(1250, 5),
                    Start(1302, 4, "FFM F", product: "ICE1651 P28/36"),
                    End(1645,10,"Leipzig"),
                },
                Date = new DateTime(2013, 6, 4)
            };

            var travelBack = new Travel("Leipzig Juni 2013 Zurück")
            {
                Steps = new[]
                {
                    Start(1511, 10, "Leipzig", product: "ICE1556 P38/72"),
                    End(1855,7),
                    Start(1909,6, "FFM F", product: "ICE514 P39/31"),
                    End(2005, 5, "Köln"),
                },
                Date = new DateTime(2013, 6, 6)
            };

            travel.ReplendishAndCheck();
            travelBack.ReplendishAndCheck();
            
            var t = new CalendarGenerator(travel.ToCalendar).String;
            Tracer.Line(t);
            var tb = new CalendarGenerator(travelBack.ToCalendar).String;
            Tracer.Line(tb);
        }
        static BahnEvent End(int date, int platForm, string station = null) { return End(date, "G" + platForm, station); }
        static BahnEvent Start(int start, string station, int? dummy = null, string product = null) { return Start(start, "", station, product: product); }
        static BahnEvent Start(int start, int platForm, string station, int? dummy = null, string product = null) { return Start(start, "G" + platForm, station, product: product); }
        
        static BahnEvent End(int date, string platForm = null, string station = null) { return new BahnEvent(false, Travel.ToDateTime(date), station, platForm); }

        static BahnEvent Start(int start, string platForm, string station, int? dummy = null, string product = null)
        {
            return new BahnEvent(true, Travel.ToDateTime(start), station, platForm)
            {
                Product = product
            };
        }
    }

    partial class CalendarGenerator
    {
        readonly Calendar _data;
        internal CalendarGenerator(Calendar data) { _data = data; }
        internal string String { get { return Reformat(TransformText()); } }
        
        static string Reformat(string text)
        {
            var result = ReplaceAll("\n" + text, R("\n ", "\n"), R("\n\t", "\n"));
            while(result[0] == '\n')
                result = result.Substring(1);
            return result;
        }

        static Tuple<string, string> R(string oldValue, string newValue) { return new Tuple<string, string>(oldValue, newValue); }

        static string ReplaceAll(string text, params Tuple<string, string>[] data)
        {
            while(true)
            {
                var newText = data.Aggregate(text, (c, n) => c.Replace(n.Item1, n.Item2));
                if(text == newText)
                    return newText;
                text = newText;
            }
        }
    }

    static class DateTimeExtender
    {
        public static string Format(this DateTime dateTime) { return dateTime.ToString("yyyyMMddTHHmmss"); }
    }
}