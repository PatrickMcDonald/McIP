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
            bool headingPrinted = false;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith(" ", StringComparison.Ordinal))
                {
                    heading = line;
                    headingPrinted = false;
                }

                if (line.ContainsAny("IP Address", "IPv4 Address", "IPv6 Address"))
                {
                    if (!headingPrinted)
                    {
                        Console.WriteLine();
                        Console.WriteLine(heading);
                        headingPrinted = true;
                    }

                    Console.WriteLine(line);
                }
            }
        }

        private static bool ContainsAny(this string target, params string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            foreach (var value in values)
            {
                if (target.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
