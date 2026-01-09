module Raytracer.Geometry.Shape

open Raytracer.Math
open Raytracer.Math.Matrix
open Raytracer.Graphics

module Sphere =
    [<CustomEquality; NoComparison>]
    type T =
        { mutable transform: M4.T
          mutable material: Material.T }

        interface System.IEquatable<T> with
            member this.Equals(other: T) : bool =
                let a = this.material.Equals other.material
                let b = this.transform.Equals other.transform
                a && b

        override this.Equals(obj) =
            match obj with
            | :? T as other -> (this :> System.IEquatable<T>).Equals other
            | _ -> false

        override this.GetHashCode() : int = hash (this.transform, this.material)

    let create transform material =
        { transform = transform
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

[<CustomEquality; NoComparison>]
type T =
    | Sphere of Sphere.T

    interface System.IEquatable<T> with
        member this.Equals(other: T) : bool =
            match this, other with
            | Sphere a, Sphere b -> a.Equals(b)

    override this.Equals obj =
        match obj with
        | :? T as other -> (this :> System.IEquatable<T>).Equals other
        | _ -> false

    override this.GetHashCode() : int =
        match this with
        | Sphere s -> hash s

let sphere transform material =
    Sphere.create transform material |> Sphere

let getTransform shape =
    match shape with
    | Sphere s -> s.transform

let setTransfrom shape m =
    match shape with
    | Sphere s -> s.transform <- m

let getMaterial shape =
    match shape with
    | Sphere s -> s.material

let normalAt shape point =
    let transform = getTransform shape
    let inv = transform |> M4.inverse
    let object_point = inv * point
    let object_normal = object_point - Tuple.ORIGIN
    let world_point = M4.transpose inv * object_normal
    Tuple.vector world_point.x world_point.y world_point.z |> Tuple.normalize
