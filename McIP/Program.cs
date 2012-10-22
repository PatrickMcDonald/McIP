namespace McIP
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class Program
    {
        public static void Main()
        {
            var si = new ProcessStartInfo("ipconfig", "/all")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var p = Process.Start(si);

            StreamReader reader = p.StandardOutput;

            string heading = string.Empty;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith(" ", StringComparison.Ordinal))
                {
                    heading = line;
                }

                if (line.Contains("IP Address"))
                {
                    Console.WriteLine(heading);
                    Console.WriteLine(line);
                    Console.WriteLine();
                }
            }
        }
    }
}
