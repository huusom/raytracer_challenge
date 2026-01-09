module Raytracer.Geometry.Transformation

open Raytracer.Math.Matrix.M4

let identity = identity

let translation x y z =
    create 1. 0. 0. x 0. 1. 0. y 0. 0. 1. z 0. 0. 0. 1.

let scaling x y z =
    create x 0. 0. 0. 0. y 0. 0. 0. 0. z 0. 0. 0. 0. 1.

let rotation_x r =
    let c = cos r
    let s = sin r
    create 1. 0. 0. 0. 0. c -s 0. 0. s c 0. 0. 0. 0. 1.

let rotation_y r =
    let c = cos r
    let s = sin r
    create c 0. s 0. 0. 1. 0. 0. -s 0. c 0. 0. 0. 0. 1.

let rotation_z r =
    let c = cos r
    let s = sin r
    create c -s 0. 0. s c 0. 0. 0. 0. 1. 0. 0. 0. 0. 1.

let shearing xy xz yx yz zx zy =
    create 1. xy xz 0. yx 1. yz 0. zx zy 1. 0. 0. 0. 0. 1.

let combine (transformations: seq<T>) =
    transformations |> Seq.rev |> Seq.reduce (*)
