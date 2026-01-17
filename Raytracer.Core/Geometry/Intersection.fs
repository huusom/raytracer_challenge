module Raytracer.Geometry.Intersection

open Raytracer.Geometry.Shape
open Raytracer.Math

module Comps =
    type T =
        { t: float
          object: Shape.T
          point: Tuple.T
          over: Tuple.T
          eye: Tuple.T
          normal: Tuple.T
          inside: bool }

    let create t object point over eye normal inside =
        { t = t
          object = object
          point = point
          over = over
          eye = eye
          normal = normal
          inside = inside }

type T = { t: float; object: Shape.T }

let create object t = { t = t; object = object }

let sort xs = xs |> Seq.sortBy (fun i -> i.t)

let intersectionsOf shape ray =
    let r = Ray.transformationOf ray shape.transform.inverse.Value
    shape.intersect r |> Seq.map (create shape) 
    

let hitFrom xs =
    xs |> Seq.tryFind (fun i -> i.t > 0)

let compsFrom i ray =
    let point = Ray.positionFrom ray i.t
    let eye = -ray.direction

    let normal, inside =
        match normalFrom i.object point with
        | n when Tuple.dot n eye < 0.0 -> -n, true
        | n -> n, false

    let over = point + normal * epsilon

    Comps.create i.t i.object point over -ray.direction normal inside
