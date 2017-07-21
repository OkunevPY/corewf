using System;
using System.IO;
using System.Reflection;
using Portable.Xaml;
using System.Linq;
using CoreWf;

namespace portableXAML
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

        public static string MethodSignature(MethodInfo mi)
        {
            String[] param = mi.GetParameters()
                            .Select(p => String.Format("{0} {1}", p.ParameterType.Name, p.Name))
                            .ToArray();


            string signature = String.Format("{0} {1}({2})", mi.ReturnType.Name, mi.Name, String.Join(",", param));

            return signature;
        }


        static void Main(string[] args)
        {
            string XamlText = @"<Activity x:Class=""WFTemplate"" xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">   <Sequence>     <WriteLine Text=""HelloWorld"" />   </Sequence> </Activity>";
            var act = CoreWf.XamlIntegration.ActivityXamlServices.Load(GenerateStreamFromString(XamlText));
            WorkflowInvoker.Invoke(act);

            /*Assembly[] AssList = AppDomain.CurrentDomain.GetAssemblies();
            var assemblySystemXaml = AssList.First(x => x.GetName().Name.Equals("Portable.Xaml"));

            Type[] xamlTypes = assemblySystemXaml.GetTypes();
            foreach (var t in xamlTypes)
            {
                Console.WriteLine(t.FullName);
                foreach (var method in t.GetMethods())
                {
                    Console.WriteLine("\t" + MethodSignature(method));
                }
            }*/
        }
    }
}