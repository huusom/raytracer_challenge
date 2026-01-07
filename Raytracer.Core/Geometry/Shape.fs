module Raytracer.Geometry.Shape

open Raytracer.Math
open Raytracer.Math.Matrix
open Raytracer.Graphics

module Sphere =
    type T =
        { id: int
          mutable transform: M4.T
          mutable material: Material.T }

    let create id transform material =
        { id = id
          transform = transform
          material = material }

    let intersect (ray: Ray.T) =
        let str = ray.origin - Tuple.point 0. 0. 0.
        let a = Tuple.dot ray.direction ray.direction
        let b = 2. * Tuple.dot ray.direction str
        let c = Tuple.dot str str - 1.0

        let discrimant = b * b - 4. * a * c

        if discrimant < 0. then
            Seq.empty
        else
            seq {
                (-b - sqrt discrimant) / (2. * a)
                (-b + sqrt discrimant) / (2. * a)
            }

type T = Sphere of Sphere.T

let sphere id transform material =
    Sphere.create id transform material |> Sphere

let getTransform shape =
    match shape with
    | Sphere s -> s.transform

let setTransfrom shape m =
    match shape with
    | Sphere s -> s.transform <- m

let normalAt shape point =
    let transform = getTransform shape
    let inv = transform |> M4.inverse
    let object_point = inv * point
    let object_normal = object_point - Tuple.ORIGIN
    let world_point = M4.transpose inv * object_normal
    Tuple.vector world_point.x world_point.y world_point.z |> Tuple.normalize
