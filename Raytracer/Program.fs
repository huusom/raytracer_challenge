open Raytracer.Graphics

let ppm = Raytracer.Chapter7.run() |> Canvas.portablePixmapOf 

System.IO.File.WriteAllLines("out.ppm", ppm)
