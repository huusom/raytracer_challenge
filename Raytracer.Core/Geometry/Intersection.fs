module Raytracer.Geometry.Intersection

open Raytracer.Geometry.Shape
open Raytracer.Math.Matrix.M4

module Computation =
    type T =
        { t: float
          object: Shape.T
          point: Raytracer.Math.Tuple.T
          eye: Raytracer.Math.Tuple.T
          normal: Raytracer.Math.Tuple.T }

    let create t object point eye normal =
        { t = t
          object = object
          point = point
          eye = eye
          normal = normal }

type T = { t: float; object: Shape.T }

let create object t = { t = t; object = object }

let sort xs = xs |> Seq.sortBy (fun i -> i.t)

let intersect shape ray =
    let r = shape |> Shape.getTransform |> inverse |> Ray.transform ray

    match shape with
    | Shape.Sphere _ -> Sphere.intersect r
    |> Seq.map (create shape)
    |> Array.ofSeq

let hit xs =
    xs |> sort |> Seq.skipWhile (fun i -> i.t < 0.) |> Seq.tryHead

let prepare i ray =
    let point = Ray.position ray i.t

    Computation.create i.t i.object point -ray.direction (Shape.normalAt i.object point)
