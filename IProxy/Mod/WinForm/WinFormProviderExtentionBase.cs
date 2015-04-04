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
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IProxy.Mod.WinForm
{
    public abstract class WinFormProviderExtentionBase
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(WinFormProviderExtentionBase));

        private readonly List<Type> registeredForms;

        private ManualResetEvent threadPause = new ManualResetEvent(true); 

        //Using thread instead of task here, because forms should always run on a thread.
        public Thread FormThread { get; private set; }
        public Form CurrentForm { get; private set; }
        private Type toOpenForm { get; set; }

        public WinFormProviderExtentionBase()
        {
            registeredForms = new List<Type>();
            FormThread = new Thread(new ThreadStart(FormThreadMethod));
            Application.SetCompatibleTextRenderingDefault(true);
            Application.EnableVisualStyles();

            registeredForms.AddRange(GetForms());
        }

        public void SetNextForm(Type form)
        {
            if (!registeredForms.Contains(form)) throw new InvalidOperationException("Form is not registered. Maybe a subform?");
            if(CurrentForm != null && (form == CurrentForm.GetType())) return;
            toOpenForm = form;
            ResumeThread();
        }

        public void ResumeThread()
        {
            this.threadPause.Set();
        }

        public void PauseThread()
        {
            this.threadPause.Reset();
        }

        private void FormThreadMethod()
        {
            while (true)
            {
                try
                {
                    if (toOpenForm != null)
                    {
                        var form = (Form)Activator.CreateInstance(toOpenForm);
                        toOpenForm = null;
                        Application.Run(CurrentForm = form);
                        CurrentForm = null;
                        if (toOpenForm == null)
                            PauseThread();
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Error in FormThread: {0}", ex);
                }
                threadPause.WaitOne();
            }
        }

        public abstract IEnumerable<Type> GetForms();
        public abstract void CreateSingletonForProvider();
    }
}
