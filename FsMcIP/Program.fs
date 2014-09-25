namespace FsMcIP

open System.IO
open System.Diagnostics

module Program =
    [<EntryPoint>]
    let main argv =
        let containsAny parms (line:string) =
            Seq.exists (fun v -> line.Contains(v)) parms

        let startsWith value (line:string) =
            line.StartsWith(value)

        let readLines (reader:StreamReader) =
            seq { while not reader.EndOfStream do
                    yield reader.ReadLine() }

        let startIPConfig() =
            let si = new ProcessStartInfo("ipconfig", "/all", RedirectStandardOutput = true, UseShellExecute = false)
            let p = Process.Start(si)
            p.StandardOutput

        let isHeading = startsWith " " >> not

        let isIPAddress = containsAny ["IP Address"; "IPv4 Address"; "IPv6 Address"]

        let printIPAddress (heading, printed) line =
            if not printed then
                printfn ""
                printfn "%s" heading
            printfn "%s" line

        let (|Heading|IPAddress|End|Other|) lines =
            match lines with
            | ""::tail -> Other tail
            | line::tail when isHeading line -> Heading (line, tail)
            | line::tail when isIPAddress line -> IPAddress (line, tail)
            | _::tail -> Other tail
            | [] -> End

        let analyze =
            let rec analyze' (heading, printed) = function
                | Heading(line, tail) -> analyze' (line, false) tail

                | IPAddress(line, tail) ->
                    printIPAddress (heading, printed) line
                    analyze' (heading, true) tail

                | Other tail -> analyze' (heading, printed) tail
                | End -> ()
            analyze' ("", false)

        startIPConfig()
            |> readLines
            |> List.ofSeq
            |> analyze

        0 // return an integer exit code
