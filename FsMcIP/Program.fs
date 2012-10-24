namespace FsMcIP

open FsMcIP.ExtensionMethods

module Program =
    [<EntryPoint>]
    let main argv =
        let isHeading (line:string) = not (line.StartsWith(" "))

        let isIPAddress (line: string) = line.ContainsAny(["IP Address"; "IPv4 Address"; "IPv6 Address"])

        let printIPAddress (heading, printed) line =
            if not printed then 
                printfn "%s" ""
                printfn "%s" heading
            printfn "%s" line

        let rec analyze (heading, printed) lines =
            match lines with
            | [] -> ()
            | ""::tail -> analyze (heading, printed) tail
            | line::tail when isHeading line -> analyze (line, false) tail
            
            | line::tail when isIPAddress line -> 
                printIPAddress (heading, printed) line
                analyze (heading, true) tail

            | _::tail -> analyze (heading, printed) tail
        
        System.Diagnostics.Process.StartIPConfig().StandardOutput.readLines()
            |> Seq.toList 
            |> analyze ("", false)

        0 // return an integer exit code
