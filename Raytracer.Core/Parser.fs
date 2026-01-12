module Raytracer.Parser

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
    | Rx @"^π / (\d)$" [ Float v ] -> System.Math.PI / v
    | Rx @"^π/(\d)$" [ Float v ] -> System.Math.PI / v
    | Rx @"^√(\d)/(\d)$" [ Float d; Float n ] -> sqrt d / n
    | Rx @"^-√(\d)/(\d)$" [ Float d; Float n ] -> -(sqrt d) / n
    | Rx @"^√(\d+)$" [ Float f ] -> sqrt f
    | _ -> failwithf "'%s' does not match any pattern." txt

let parseUpdate (shape: Geometry.Shape.T) txt =
    match txt with
    | Rx @"^material.specular (.*)$" [ Float s ] -> shape.material.specular <- s
    | Rx @"^material.diffuse (.*)$" [ Float s ] -> shape.material.diffuse <- s
    | Rx @"^material.color \((.*), (.*), (.*)\)$" [ Float r; Float g; Float b ] ->
        shape.material.color <- Graphics.Color.create r g b
    | Rx @"^transform scaling\((.*), (.*), (.*)\)$" [ Float x; Float y; Float z ] ->
        shape.transform <- Geometry.Transformation.scaling x y z
    | Rx @"^transform translation\((.*), (.*), (.*)\)$" [ Float x; Float y; Float z ] ->
        shape.transform <- (Geometry.Transformation.translation x y z)
    | _ -> failwithf "'%s' does not match any pattern." txt

    shape
