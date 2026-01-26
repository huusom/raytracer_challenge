module Raytracer.Scene.World

open Raytracer

type T =
    { objects: Geometry.Shape.T[]
      lights: Light.T[] }

let create objects lights = { objects = Array.ofSeq objects; lights = Array.ofSeq lights }

let appendShape world shape =
    { world with
        objects =
            seq {
                yield! world.objects
                yield shape
            }
            |> Array.ofSeq }

let intersectionsOf world ray =
    world.objects
    |> Seq.collect (fun shape -> Geometry.Intersection.intersectionsOf shape ray)
    |> Seq.sortBy(fun i -> i.t)

let inShadow world point =
    let v = world.lights[0].position - point
    let distance = Math.Tuple.magnitude v
    let direction = Math.Tuple.normalize v
    let r = Geometry.Ray.create point direction
    let xs = intersectionsOf world r

    match Geometry.Intersection.hitFrom xs with
    | Some h -> h.t < distance
    | _ -> false

let colorFrom world (comps: Geometry.Intersection.Comps.T) =
    let s = inShadow world comps.over
    Light.colorFrom world.lights[0] comps s 

let trace world ray =
    let xs = intersectionsOf world ray
    match Geometry.Intersection.hitFrom xs with
    | None -> Graphics.Color.black
    | Some i -> 
        Geometry.Intersection.compsFrom i ray 
        |> colorFrom world 
