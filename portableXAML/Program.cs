using System;
using System.IO;
using System.Reflection;
using Portable.Xaml;
using System.Linq;
using CoreWf;
using CoreWf.XamlIntegration;

namespace XAMLConsoleApp
{
    class Program
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void Main(string[] args)
        {
            Portable.Xaml.XamlObjectReaderSettings rs = new XamlObjectReaderSettings() { };
            Portable.Xaml.XamlSchemaContext xsContext = new XamlSchemaContext() {  };
            XamlSchemaContextSettings xsContextSettings = new XamlSchemaContextSettings() { };

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ActivityXamlServicesSettings settings = new CoreWf.XamlIntegration.ActivityXamlServicesSettings { CompileExpressions = false };

            string ActivityAlone = @"<Activity x:Class=""WFTemplate"" xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">   </Activity>";
            var act = CoreWf.XamlIntegration.ActivityXamlServices.Load(GenerateStreamFromString(ActivityAlone), settings);
            WorkflowInvoker.Invoke(act);

            string ActivityWriteLine = @"<Activity x:Class=""WFTemplate"" xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">  <WriteLine Text=""HelloWorld"" /> </Activity>";
            act = CoreWf.XamlIntegration.ActivityXamlServices.Load(GenerateStreamFromString(ActivityWriteLine), settings);
            WorkflowInvoker.Invoke(act);

            string XamlText = @"<Activity x:Class=""WFTemplate"" xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">   <Sequence>     <WriteLine Text=""HelloWorld"" />   </Sequence> </Activity>";
            act = CoreWf.XamlIntegration.ActivityXamlServices.Load(GenerateStreamFromString(XamlText), settings);
            WorkflowInvoker.Invoke(act);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(sender.ToString() + " " + args.Name);
            return null; 
        }
    }
}