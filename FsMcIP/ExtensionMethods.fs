namespace FsMcIP

open System.Diagnostics

module ExtensionMethods =

    type System.String with
        member this.ContainsAny (parms: string seq) =
            Seq.exists (fun v -> this.Contains(v)) parms
    
    type System.IO.StreamReader with
        member this.readLines() =
            seq { while not this.EndOfStream do
                    yield this.ReadLine() }

    type System.Diagnostics.Process with
        static member StartIPConfig() =
            let si = new ProcessStartInfo("ipconfig", "/all", RedirectStandardOutput = true, UseShellExecute = false)
            Process.Start(si)
        