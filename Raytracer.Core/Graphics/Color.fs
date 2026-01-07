module Raytracer.Graphics.Color

open Raytracer.Library

[<CustomEquality; NoComparison>]
type T =
    { r: float
      g: float
      b: float }

    interface System.IEquatable<T> with
        member this.Equals(other: T) : bool =
            eq this.r other.r && eq this.g other.g && eq this.b other.b

    static member (+)(l, r) =
        { r = l.r + r.r
          g = l.g + r.g
          b = l.b + r.b }

    static member (-)(l, r) =
        { r = l.r - r.r
          g = l.g - r.g
          b = l.b - r.b }

    static member (*)(l, r) =
        { r = l.r * r.r
          g = l.g * r.g
          b = l.b * r.b }

    static member (*)(l, r) =
        { r = l.r * r
          g = l.g * r
          b = l.b * r }

let color r g b = { r = r; g = g; b = b }
let black = color 0 0 0
let white = color 1 1 1
let equals (l: T) (r: T) = (l :> System.IEquatable<T>).Equals(r)

let toInt (t: T) =
    let clamp x = x * 256.0 |> int |> min 255 |> max 0

    [ clamp t.r; clamp t.g; clamp t.b ]
