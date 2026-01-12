module Raytracer.Scene.World

open Raytracer

type T =
    { objects: Geometry.Shape.T[]
      lights: Graphics.Light.T[] }

let create objects lights = { objects = objects; lights = lights }

let add_shape world shape =
    { world with
        objects =
            seq {
                yield! world.objects
                yield shape
            }
            |> Array.ofSeq }

let intersect world ray =
    world.objects
    |> Seq.collect (fun shape -> Geometry.Intersection.intersect shape ray)
    |> Geometry.Intersection.sort
    |> Array.ofSeq

let in_shadow world point =
    let v = world.lights[0].position - point
    let distance = Math.Tuple.magnitude v
    let direction = Math.Tuple.normalize v
    let r = Geometry.Ray.create point direction
    let xs = intersect world r

    match Geometry.Intersection.hit xs with
    | Some h -> h.t < distance
    | _ -> false

let shade world (comps: Geometry.Intersection.Comps.T) =
    let s = in_shadow world comps.over
    let m = comps.object.material
    Graphics.Material.lightning m world.lights[0] comps.point comps.eye comps.normal s

let color world ray =
    let xs = intersect world ray

    match Geometry.Intersection.hit xs with
    | None -> Graphics.Color.black
    | Some i -> Geometry.Intersection.prepare i ray |> shade world
