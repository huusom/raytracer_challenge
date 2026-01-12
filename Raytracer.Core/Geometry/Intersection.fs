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

let intersect shape ray =
    let r = shape.transform |> Transformation.inverse |> Ray.transform ray

    shape.intersect r
    |> Seq.map (create shape)
    |> Array.ofSeq

let hit xs =
    xs |> sort |> Seq.skipWhile (fun i -> i.t < 0.) |> Seq.tryHead

let prepare i ray =
    let point = Ray.position ray i.t
    let eye = -ray.direction

    let normal, inside =
        match normalAt i.object point with
        | n when Tuple.dot n eye < 0.0 -> -n, true
        | n -> n, false

    let over = point + normal * Raytracer.Library.epsilon

    Comps.create i.t i.object point over -ray.direction normal inside
