//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#region

using log4net;
using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace IProxy.common
{
    public class SimpleSettings : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (SimpleSettings));

        private readonly string cfgFile;
        private readonly string id;
        private readonly Dictionary<string, string> values;

        public SimpleSettings(string id)
        {
            log.InfoFormat("Loading settings for '{0}'...", id);

            values = new Dictionary<string, string>();
            this.id = id;
            cfgFile = Path.Combine(Environment.CurrentDirectory, id + ".cfg");
            if (!File.Exists(cfgFile))
            {
                log.Warn("Settings not found.");
                log.Info("Creating settings file.");
                File.Create(cfgFile).Dispose();
            }

            using (StreamReader rdr = new StreamReader(File.OpenRead(cfgFile)))
            {
                string line;
                int lineNum = 1;
                while ((line = rdr.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || String.IsNullOrWhiteSpace(line)) continue;
                    int i = line.IndexOf(":");
                    if (i == -1)
                    {
                        log.InfoFormat("Invalid settings at line {0}.", lineNum);
                        throw new ArgumentException("Invalid settings.");
                    }
                    string val = line.Substring(i + 1);

                    values.Add(line.Substring(0, i),
                        val.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? null : val);
                    lineNum++;
                }
                log.InfoFormat("Settings loaded.");
            }
        }

        public void Save()
        {
            try
            {
                log.InfoFormat("Saving settings for '{0}'...", id);
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(cfgFile)))
                    foreach (KeyValuePair<string, string> i in values)
                        writer.WriteLine("{0}:{1}", i.Key, i.Value == null ? "null" : i.Value);
            }
            catch (Exception e)
            {
                log.Error("Error when saving settings.", e);
            }
        }

        public void Dispose()
        {
            Save();
        }

        public string GetValue(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    log.ErrorFormat("Attempt to access nonexistant settings '{0}'.", key);
                    throw new ArgumentException(string.Format("'{0}' does not exist in settings.", key));
                }
                ret = values[key] = def;
            }
            return ret;
        }

        public T GetValue<T>(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    log.ErrorFormat("Attempt to access nonexistant settings '{0}'.", key);
                    throw new ArgumentException(string.Format("'{0}' does not exist in settings.", key));
                }
                ret = values[key] = def;
            }
            return (T) Convert.ChangeType(ret, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void SetValue(string key, string val)
        {
            values[key] = val;
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            foreach (var setting in values)
                yield return setting;
        }
    }
}