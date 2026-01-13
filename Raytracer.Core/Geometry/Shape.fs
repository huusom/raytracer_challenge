module Raytracer.Geometry.Shape

open Raytracer.Math
open Raytracer.Math.Matrix
open Raytracer.Graphics

[<CustomEquality; NoComparison>]
type T =
    { mutable transform: M4.T
      mutable material: Material.T
      intersect: Ray.T -> seq<float>
      normal: Tuple.T -> Tuple.T }

    override this.Equals(obj) =
        match obj with
        | :? T as other -> this.material.Equals other.material && this.transform.Equals other.transform
        | _ -> false

    override this.GetHashCode() : int = hash (this.transform, this.material)

let create transform material intersect normal =
    { transform = transform
      material = material
      intersect = intersect
      normal = normal }

let sphereOf transform material =
    let intersect (ray: Ray.T) =
        let str = ray.origin - Tuple.origin
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

    let normal point = point - Tuple.origin

    create transform material intersect normal

let testOf transform material (save_ray: System.Action<Ray.T>) =
    let intersect ray =
        save_ray.Invoke ray
        Seq.empty

    create transform material intersect Tuple.vectorFrom

let planeOf transform material =
    let n = Tuple.vectorOf 0 1 0

    let intersect (ray: Ray.T) =
        if abs ray.direction.y < epsilon then
            Seq.empty
        else
            Seq.singleton (-ray.origin.y / ray.direction.y)

    create transform material intersect (fun _ -> n)

let normalFrom shape point =
    let inverse = Transformation.inverse shape.transform
    let local_point = inverse * point
    let local_normal = shape.normal local_point
    let world_normal = Transformation.transpose inverse * local_normal

    world_normal |> Tuple.vectorFrom |> Tuple.normalize
