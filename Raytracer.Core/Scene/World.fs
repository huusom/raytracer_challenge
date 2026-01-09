module Raytracer.Scene.World

type T =
    { objects: Raytracer.Geometry.Shape.T[]
      lights: Raytracer.Graphics.Light.T[] }

let create objects lights = { objects = objects; lights = lights }

let intersect world ray =
    world.objects
    |> Seq.collect (fun shape -> Raytracer.Geometry.Intersection.intersect shape ray) 
    |> Raytracer.Geometry.Intersection.sort 
    
  

