module Raytracer.Geometry.Transformation

open Raytracer.Math.Matrix
open Raytracer.Math

[<CustomEquality; NoComparison>]
type T =
    { source: M4.T
      inverse: Lazy<M4.T> }

    override this.Equals obj =
        match obj with
        | :? T as other -> this.source.Equals(other.source)
        | _ -> false

    override this.GetHashCode() = this.source.GetHashCode()

    static member (*)(l, r) =
        let s = l.source * r.source

        { source = s
          inverse = lazy (M4.inverse s) }

let create source inverse =
    { source = source
      inverse = lazy inverse }

let init source =
    { source = source
      inverse = lazy (M4.inverse source) }

let identity = M4.identity |> init

let translationOf x y z =
    M4.create 1. 0. 0. x 0. 1. 0. y 0. 0. 1. z 0. 0. 0. 1. |> init

let scalingOf x y z =
    M4.create x 0. 0. 0. 0. y 0. 0. 0. 0. z 0. 0. 0. 0. 1. |> init

let rotationXOf r =
    let c = cos r
    let s = sin r
    M4.create 1. 0. 0. 0. 0. c -s 0. 0. s c 0. 0. 0. 0. 1. |> init

let rotationYOf r =
    let c = cos r
    let s = sin r
    M4.create c 0. s 0. 0. 1. 0. 0. -s 0. c 0. 0. 0. 0. 1. |> init

let rotationZOf r =
    let c = cos r
    let s = sin r
    M4.create c -s 0. 0. s c 0. 0. 0. 0. 1. 0. 0. 0. 0. 1. |> init

let shearingOf xy xz yx yz zx zy =
    M4.create 1. xy xz 0. yx 1. yz 0. zx zy 1. 0. 0. 0. 0. 1. |> init

let combine (transformations: seq<T>) = transformations |> Seq.reduce (*)

let viewOf (from: Tuple.T) to' up =
    let forward = Tuple.normalize (to' - from)
    let upn = Tuple.normalize up
    let left = Tuple.cross forward upn
    let true_up = Tuple.cross left forward

    let orientation =
        M4.create left.x left.y left.z 0 true_up.x true_up.y true_up.z 0 -forward.x -forward.y -forward.z 0 0 0 0 1
        |> init

    combine [ orientation; translationOf -from.x -from.y -from.z ]
