module Raytracer.Graphics.Pattern

open Raytracer.Graphics
open Raytracer.Math

type Kind =
    | Stripe
    | Solid
    | Test
    | Gradient
    | Ring
    | Checker

[<CustomEquality; NoComparison>]
type T =
    { kind: Kind
      a: Color.T
      b: Color.T
      mutable transform: Transformation.T
      colorFrom: Tuple.T -> Color.T }

    override this.Equals obj =
        match obj with
        | :? T as other -> this.kind = other.kind && this.a.Equals other.a && this.b.Equals other.b
        | _ -> false

    override this.GetHashCode() = hash (this.kind, this.a, this.b)

let create kind a b colorFrom transform =
    { kind = kind
      a = a
      b = b
      colorFrom = colorFrom
      transform = transform }

let stripeOf a b =
    Transformation.identity
    |> create Stripe a b (fun p -> if floor p.x % 2.0 = 0 then a else b)

let solidOf a =
    Transformation.identity |> create Solid a a (fun _ -> a)

let testOf () =
    Transformation.identity
    |> create Test Color.black Color.black (fun p -> Color.create p.x p.y p.z)

let gradientOf a b =
    let d = b - a

    Transformation.identity
    |> create Gradient a b (fun p -> a + d * (p.x - floor p.x))

let ringOf a b =
    Transformation.identity
    |> create Ring a b (fun p ->
        if floor (sqrt (p.x * p.x + p.z * p.z)) % 2. = 0. then
            a
        else
            b)

let checkerOf a b =
    Transformation.identity
    |> create Checker a b (fun p ->
        if int (floor p.x + floor p.y + floor p.z) % 2 = 0 then
            a
        else
            b)

let colorFrom pattern point =
    pattern.transform.inverse.Value * point |> pattern.colorFrom
