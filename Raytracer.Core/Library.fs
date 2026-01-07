[<AutoOpen>]
module Raytracer.Library

let epsilon = 0.00001
let inline eq a b = abs (a - b) < epsilon

let (|Rx|_|) pattern input =
    match System.Text.RegularExpressions.Regex.Match(input, pattern) with
    | m when m.Success -> [ for x in m.Groups -> x.Value ] |> List.tail |> Some
    | _ -> None

let (|Float|_|) input =
    match System.Double.TryParse(input: string) with
    | (true, f) -> Some f
    | _ -> None

let parseFloat txt =
    match txt with
    | Rx @"^π / (\d)$" [ Float v ] -> v
    | Rx @"^√(\d)/(\d)$" [ Float d; Float n ] -> (sqrt d) / n
    | Rx @"^-√(\d)/(\d)$" [ Float d; Float n ] -> -(sqrt d) / n
    | Rx @"^√(\d+)$" [ Float f ] -> sqrt f
    | _ -> failwithf "'%s' does not match any pattern." txt
