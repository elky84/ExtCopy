﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RepeatedCopy
{
    public static class Run
    {
        public static void Execute(Options opts)
        {
            if (opts.CleanUp && Directory.Exists(opts.Output))
            {
                DeleteDirectory(opts.Output);
            }

            Directory.CreateDirectory(opts.Output);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var today = DateTime.Now.Date;
            var save = LoadSaveFile();
            if (save == null)
            {
                save = new Save { };
            }

            do
            {
                foreach (var file in Directory.GetFiles(opts.Input))
                {
                    var fileName = Path.GetFileName(file);
                    if (!string.IsNullOrEmpty(opts.Pattern) && !Regex.IsMatch(fileName, opts.Pattern))
                        continue;

                    // 첫 파일이거나 오늘꺼거나
                    if (today == File.GetLastWriteTime(file).Date || !save.ProcessFiles.Contains(file))
                    {
                        var dest = opts.Output + "\\" + Path.GetFileName(file);
                        Console.WriteLine($"<Src:{file}> <Dest:{dest}> <Size:{new System.IO.FileInfo(file).Length}>");
                        File.Copy(file, dest, true);

                        if (!save.ProcessFiles.Contains(file))
                        {
                            save.ProcessFiles.Add(file);
                            SaveFile(save);
                        }
                    }

                }

                Console.WriteLine($"Process Complete.");
                Thread.Sleep(opts.TimeInterval * 1000);
            }
            while (opts.Repeat);
        }

        public static Save? LoadSaveFile()
        {
            try
            {
                var json = File.ReadAllText("Save.json");
                return JsonConvert.DeserializeObject<Save>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void SaveFile(Save save)
        {
            try
            {
                File.WriteAllText("Save.json", JsonConvert.SerializeObject(save, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }
    }
}