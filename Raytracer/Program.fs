open Raytracer.Graphics
open Raytracer.IO.Parser
open Raytracer.Scene
open Raytracer.Presets
open Microsoft.Extensions.Configuration

[<CLIMutable>]
type Options =
    { width: int
      height: int
      fieldOfView: string
      out: string }

let render (options: Options) (f: Camera.T -> Canvas.T) =
    let fov = floatFrom options.fieldOfView
    printfn "Render with:" 
    printfn " - size : %i x %i" options.width options.height
    printfn " - fov  : %f (%s)" fov options.fieldOfView
    printfn " - out  : %s" options.out
    let camera = Camera.cameraOf options.width options.height fov
    let sw = System.Diagnostics.Stopwatch.StartNew ()
    let ppm = f camera |> Canvas.portablePixmapOf
    sw.Stop() 
    printfn " - time : %A" sw.Elapsed
    System.IO.File.WriteAllLines(options.out, ppm)

[<EntryPoint>]
let main args =
    let arg0 = if Array.isEmpty args then "" else args[0]

    let root =
        ConfigurationBuilder().AddJsonFile("appsettings.json").AddCommandLine(args).Build()

    let options = root.Get<Options>()

    let scenes =
        [ Chapter5.name, Chapter5.render; Chapter6.name, Chapter6.render ; Chapter7.name, Chapter7.render; Chapter9.name, Chapter9.render ] |> Map.ofList

    match arg0 with
    | _ when Map.containsKey arg0 scenes -> render options scenes[arg0]
    | _ ->
        for name in Map.keys scenes do
            printfn "- %s" name

    0
