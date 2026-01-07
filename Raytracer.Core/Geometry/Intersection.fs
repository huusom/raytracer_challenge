module Raytracer.Geometry.Intersection

open Raytracer.Geometry.Shape
open Raytracer.Math.Matrix.M4

type T = { t: float; object: Shape.T }

let create object t = { t = t; object = object }

let intersect shape ray =
    let r = shape |> getTransform |> inverse |> Ray.transform ray

    match shape with
    | Sphere _ -> Sphere.intersect r
    |> Seq.map (create shape)
    |> Array.ofSeq

let hit xs =
    xs
    |> Seq.sortBy (fun i -> i.t)
    |> Seq.skipWhile (fun i -> i.t < 0.)
    |> Seq.tryHead
