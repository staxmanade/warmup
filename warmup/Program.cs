﻿// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace warmup
{
    using System;
    using settings;

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                ShowHelp();
                Environment.Exit(-1);
            }
            //parse out command line
            // warmup web newprojName
            string templateName = args[0];
            string name = args[1];
            string target = null;
            if (args.Length > 2) target = args[2];

            var td = new TargetDir(name);
            IExporter exporter = GetExporter();
            exporter.Export(WarmupConfiguration.settings.SourceControlWarmupLocation, templateName, td);
            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
            td.MoveToDestination(target);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("==========");
            Console.WriteLine("WarmuP");
            Console.WriteLine("==========");
            Console.WriteLine("current settings");
            Console.WriteLine("----------");
            Console.WriteLine("Your current configuration is set to {0} ({1}).", WarmupConfiguration.settings.SourceControlType,
                              WarmupConfiguration.settings.SourceControlWarmupLocation);
            Console.WriteLine("----------");
            Console.WriteLine("usage");
            Console.WriteLine("----------");
            Console.WriteLine("warmup templateFolderName replacementName [targetDirectoryIfDifferentThanReplacementName]");
            Console.WriteLine("Example: warmup base Bob");
            Console.WriteLine("Example: 'base' is a subfolder in your warmup template that has a warmup template in it. 'Bob' is what you want to use instead of the token '__NAME__'.");
        }

        static IExporter GetExporter()
        {
            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    return new Svn();
                case SourceControlType.Git:
                    return new Git();
                case SourceControlType.FileSystem:
                    return new Folder();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}