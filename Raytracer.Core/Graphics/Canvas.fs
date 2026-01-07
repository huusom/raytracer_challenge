module Raytracer.Graphics.Canvas

type T =
    { w: int
      h: int
      p: Color.T array }

    member this.Item
        with get (x, y) = this.p[y * this.h + x]
        and set (x, y) v = this.p[y * this.h + x] <- v

let canvas w h =
    { w = w
      h = h
      p = Array.create (w * h) Color.black }

let toPortablePixmap canvas =
    seq {
        yield "P3"
        yield sprintf "%i %i" canvas.w canvas.h
        yield "255"

        for y in [ 0 .. canvas.h - 1 ] do
            let line =
                [ for x in 0 .. canvas.w - 1 -> Color.toInt canvas[x, y] ]
                |> List.concat
                |> List.map (sprintf "%i")
                |> String.concat " "

            if line.Length < 70 then
                yield line
            else
                let i = line.LastIndexOf(' ', 70)
                yield line.Substring(0, i)
                yield line.Substring(i + 1)

        yield System.Environment.NewLine
    }
