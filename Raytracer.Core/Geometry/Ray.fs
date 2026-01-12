module Raytracer.Geometry.Ray

open Raytracer.Math

[<Struct>]
type T =
    { origin: Tuple.T
      direction: Tuple.T }


let create origin direction =
    { origin = origin
      direction = direction }

let position ray t = ray.origin + ray.direction * t

let transform ray (m: Matrix.M4.T) =
    create (m * ray.origin) (m * ray.direction)
