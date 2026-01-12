module Raytracer.Geometry.Transformation

open Raytracer.Math.Matrix
open Raytracer.Math

let identity = M4.identity

let inverse t = M4.inverse t 

let transpose t = M4.transpose t

let translation x y z =
    M4.create 1. 0. 0. x 0. 1. 0. y 0. 0. 1. z 0. 0. 0. 1.

let scaling x y z =
    M4.create x 0. 0. 0. 0. y 0. 0. 0. 0. z 0. 0. 0. 0. 1.

let rotation_x r =
    let c = cos r
    let s = sin r
    M4.create 1. 0. 0. 0. 0. c -s 0. 0. s c 0. 0. 0. 0. 1.

let rotation_y r =
    let c = cos r
    let s = sin r
    M4.create c 0. s 0. 0. 1. 0. 0. -s 0. c 0. 0. 0. 0. 1.

let rotation_z r =
    let c = cos r
    let s = sin r
    M4.create c -s 0. 0. s c 0. 0. 0. 0. 1. 0. 0. 0. 0. 1.

let shearing xy xz yx yz zx zy =
    M4.create 1. xy xz 0. yx 1. yz 0. zx zy 1. 0. 0. 0. 0. 1.

let combine (transformations: seq<T>) =
    transformations |> Seq.rev |> Seq.reduce (*)

let view (from: Tuple.T) to' up =
    let forward = Tuple.normalize (to' - from)
    let upn = Tuple.normalize up
    let left = Tuple.cross forward upn
    let true_up = Tuple.cross left forward

    let orientation =
        M4.create left.x left.y left.z 0 true_up.x true_up.y true_up.z 0 -forward.x -forward.y -forward.z 0 0 0 0 1

    orientation * translation -from.x -from.y -from.z
