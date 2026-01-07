module Raytracer.Math.Tuple

open Raytracer.Library

[<CustomEquality; NoComparison>]
type T =
    { x: float
      y: float
      z: float
      w: float }


    interface System.IEquatable<T> with
        member this.Equals(other: T) =
            (eq this.x other.x)
            && (eq this.y other.y)
            && (eq this.z other.z)
            && (eq this.w other.w)

    static member (+)(l, r) =
        { x = l.x + r.x
          y = l.y + r.y
          z = l.z + r.z
          w = l.w + r.w }

    static member (*)(l, r) =
        { x = l.x * r
          y = l.y * r
          z = l.z * r
          w = l.w * r }

    static member (/)(l, r) =
        { x = l.x / r
          y = l.y / r
          z = l.z / r
          w = l.w / r }

    static member (-)(l, r) =
        { x = l.x - r.x
          y = l.y - r.y
          z = l.z - r.z
          w = l.w - r.w }

    static member (~-)(r) =
        { x = -r.x
          y = -r.y
          z = -r.z
          w = -r.w }


let tuple x y z w = { x = x; y = y; w = w; z = z }
let vector x y z = tuple x y z 0.0
let point x y z = tuple x y z 1.0

let magnitude this =
    sqrt (this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w)

let normalize this =
    let magnitude = magnitude this

    { x = this.x / magnitude
      y = this.y / magnitude
      z = this.z / magnitude
      w = this.w / magnitude }


let dot this other =
    this.x * other.x + this.y * other.y + this.z * other.z + this.w * other.w

let cross this other =
    { x = this.y * other.z - this.z * other.y
      y = this.z * other.x - this.x * other.z
      z = this.x * other.y - this.y * other.x
      w = 0.0 }

let reflect v n = v - n * 2. * dot v n

let ORIGIN = point 0. 0. 0.
