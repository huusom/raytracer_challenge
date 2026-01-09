module Raytracer.Graphics.Color

open Raytracer.Library

[<Struct; CustomEquality; NoComparison>]
type T =
    { r: float
      g: float
      b: float }

    override this.Equals obj =
        match obj with
        | :? T as other -> eq this.r other.r && eq this.g other.g && eq this.b other.b
        | _ -> false

    override this.GetHashCode() = hash (this.r, this.g, this.b)

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

let create r g b = { r = r; g = g; b = b }

let black = { r = 0.0; g = 0.0; b = 0.0 }

let white = { r = 1.0; g = 1.0; b = 1.0 }

let toInt (t: T) =
    let clamp x = x * 256.0 |> int |> min 255 |> max 0

    [ clamp t.r; clamp t.g; clamp t.b ]
