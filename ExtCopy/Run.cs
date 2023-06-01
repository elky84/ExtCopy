using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtCopy
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
            var save = LoadSaveFile() ?? new Save();

            do
            {
                foreach (var file in Directory.GetFiles(opts.Input))
                {
                    var fileName = Path.GetFileName(file);
                    if (!string.IsNullOrEmpty(opts.Pattern) && !Regex.IsMatch(fileName, opts.Pattern))
                        continue;

                    // 첫 파일이거나 오늘꺼거나
                    if (today != File.GetLastWriteTime(file).Date && save.ProcessFiles.Contains(file)) continue;
                    var dest = opts.Output + "\\" + Path.GetFileName(file);
                    Console.WriteLine($"<Src:{file}> <Dest:{dest}> <Size:{BytesToString(new FileInfo(file).Length)}>");
                    File.Copy(file, dest, true);

                    if (save.ProcessFiles.Contains(file)) continue;
                    save.ProcessFiles.Add(file);
                    SaveFile(save);
                }

                Console.WriteLine($"Process Complete.");
                Thread.Sleep(opts.TimeInterval * 1000);
            }
            while (opts.Repeat);
        }

        private static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + suf[place];
        }

        private static Save? LoadSaveFile()
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

        private static void SaveFile(Save save)
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

        private static void DeleteDirectory(string targetDir)
        {
            var files = Directory.GetFiles(targetDir);
            var dirs = Directory.GetDirectories(targetDir);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }
    }
}