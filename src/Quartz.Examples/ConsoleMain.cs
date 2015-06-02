#region License
/* 
 * Copyright 2009- Marko Lahma
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using Quartz.Util;

namespace Quartz.Examples
{
    /// <summary>
    /// Console main runner.
    /// </summary>
    /// <author>Marko Lahma</author>
    public class ConsoleMain
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Type[] types = asm.GetTypes();

                IDictionary<int, Type> typeMap = new Dictionary<int, Type>();
                int counter = 1;

                List<Type> typeList = new List<Type>();
                foreach (Type t in types)
                {
                    if (new List<Type>(t.GetInterfaces()).Contains(typeof(IExample)))
                    {
                        typeList.Add(t);
                    }
                }

                // sort for easier readability
                typeList.Sort(new TypeNameComparer());

                int num = 0;

                do
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Select example to run: ");
                    foreach (Type t in typeList)
                    {
                        string counterString = string.Format("[{0}]", counter).PadRight(4);
                        Console.WriteLine("{0} {1} {2}", counterString, t.Namespace.Substring(t.Namespace.LastIndexOf(".") + 1), t.Name);
                        typeMap.Add(counter++, t);
                    }

                    Console.WriteLine();
                    Console.Write("> ");
                    string read = Console.ReadLine();
                    try
                    {
                        num = Convert.ToInt32(read);
                    }
                    catch
                    {
                        num = 0;
                    }

                    if (num != 0)
                    {
                        Type eType = typeMap[num];
                        IExample example = ObjectUtils.InstantiateType<IExample>(eType);
                        example.Run();
                        Console.WriteLine("Example run successfully.");
                    }
                } while (num != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running example: " + ex.Message);
                Console.WriteLine(ex.ToString());

            }

            Console.WriteLine("press any key exit...");
            Console.Read();
        }

        /// <summary>
        /// edit by willieni
        /// </summary>
        public class TypeNameComparer : IComparer<Type>
        {
            public int Compare(Type t1, Type t2)
            {
                if (t1.Namespace.Length > t2.Namespace.Length)
                {
                    return 1;
                }

                if (t1.Namespace.Length < t2.Namespace.Length)
                {
                    return -1;
                }

                return t1.Namespace.CompareTo(t2.Namespace); 
            }
        }
    }
}
