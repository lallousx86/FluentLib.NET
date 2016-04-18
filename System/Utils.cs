/* ----------------------------------------------------------------------------- 
* .NET FluentLib - Copyright (c) Elias Bachaalany <lallousz-x86@yahoo.com>
* All rights reserved.
* 
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
* 
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
* ----------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace lallouslab.FluentLib.Sys
{
    public class BoxedType<T>
    {
        public T Val;
    }

    public class BoxedInt : BoxedType<int> { }

    public static class Utils
    {
        /// <summary>
        /// From: http://stackoverflow.com/a/2709523
        /// http://en.wikipedia.org/wiki/Hamming_weight
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static long CountSetBits(long i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
        }

        public static string GetCurrentAsmDirectory()
        {
            return Path.GetDirectoryName((new FileInfo(global::System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName);
        }

        /// <summary>
        /// http://stackoverflow.com/a/129395
        /// </summary>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }

    public class ParseArgsClass
    {
        private string Arguments;
        Dictionary<string, string> args;

        public ParseArgsClass(string Args)
        {
            Arguments = Args;
        }

        public int Parse()
        {
            // http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
            var regex = new Regex(@"(\s(-{1,2}|/)(?<key>\w+)([:=]?((['""](?<value>.*?)['""])|(?<value>\S+))?))");
            var matches = regex.Matches(Arguments);

            args = matches.Cast<Match>().ToDictionary(
                match => match.Groups["key"].Value,
                match => match.Groups["value"].Success ? match.Groups["value"].Value : null);

            return args.Count;
        }

        public string GetStringArg(
            string OptionName,
            string DefVal = "")
        {
            string Val;
            return args.TryGetValue(OptionName, out Val) ? Val : DefVal;
        }

        public bool GetBoolArg(
            string OptionName,
            bool bDefault = true)
        {
            int val;
            return (args.ContainsKey(OptionName) && int.TryParse(args[OptionName], out val)) ? val != 0 : bDefault;
        }

        public int GetIntArg(
            string OptionName,
            int DefVal = 0)
        {
            int val;
            return (args.ContainsKey(OptionName) && int.TryParse(args[OptionName], out val)) ? val : DefVal;
        }
    }
}
