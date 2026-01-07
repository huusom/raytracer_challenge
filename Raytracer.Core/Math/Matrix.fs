module Raytracer.Math.Matrix

module M2 =
    type T =
        { m11: float
          m12: float
          m21: float
          m22: float }

        member this.Item
            with get (r, c) =
                match (r, c) with
                | 0, 0 -> this.m11
                | 0, 1 -> this.m12
                | 1, 0 -> this.m21
                | 1, 1 -> this.m22

                | _ -> failwithf "(%i, %i) cannot index into M2" r c

    let create m11 m12 m21 m22 =
        { m11 = m11
          m12 = m12
          m21 = m21
          m22 = m22 }

    let ofArray (a: array<float>) = create a[0] a[1] a[2] a[3]

    let determinant m = m.m11 * m.m22 - m.m12 * m.m21

module M3 =
    type T =
        { m11: float
          m12: float
          m13: float
          m21: float
          m22: float
          m23: float
          m31: float
          m32: float
          m33: float }

        member this.Item
            with get (r, c) =
                match (r, c) with
                | 0, 0 -> this.m11
                | 0, 1 -> this.m12
                | 0, 2 -> this.m13

                | 1, 0 -> this.m21
                | 1, 1 -> this.m22
                | 1, 2 -> this.m23

                | 2, 0 -> this.m31
                | 2, 1 -> this.m32
                | 2, 2 -> this.m33
                | _ -> failwithf "(%i, %i) cannot index into M3" r c

    let create m11 m12 m13 m21 m22 m23 m31 m32 m33 =
        { m11 = m11
          m12 = m12
          m13 = m13
          m21 = m21
          m22 = m22
          m23 = m23
          m31 = m31
          m32 = m32
          m33 = m33 }

    let ofArray (a: array<float>) =
        create a[0] a[1] a[2] a[3] a[4] a[5] a[6] a[7] a[8]

    let submatrix (m: T) row col =
        M2.ofArray
            [| for r in 0..2 do
                   for c in 0..2 do
                       if r <> row && c <> col then
                           m[r, c] |]

    let minor m row col = submatrix m row col |> M2.determinant

    let cofactor m row col =
        if (row + col) % 2 = 1 then
            -(minor m row col)
        else
            minor m row col

    let determinant m =
        m.m11 * (cofactor m 0 0) + m.m12 * (cofactor m 0 1) + m.m13 * (cofactor m 0 2)

module M4 =
    open Raytracer.Library 
    [<CustomEquality; NoComparison>]
    type T =
        { m11: float
          m12: float
          m13: float
          m14: float
          m21: float
          m22: float
          m23: float
          m24: float
          m31: float
          m32: float
          m33: float
          m34: float
          m41: float
          m42: float
          m43: float
          m44: float }

        interface System.IEquatable<T> with
            member this.Equals(other: T) : bool =
                eq this.m11 other.m11
                && eq this.m12 other.m12
                && eq this.m13 other.m13
                && eq this.m14 other.m14
                && eq this.m21 other.m21
                && eq this.m22 other.m22
                && eq this.m23 other.m23
                && eq this.m24 other.m24
                && eq this.m31 other.m31
                && eq this.m32 other.m32
                && eq this.m33 other.m33
                && eq this.m34 other.m34
                && eq this.m41 other.m41
                && eq this.m42 other.m42
                && eq this.m43 other.m43
                && eq this.m44 other.m44

        member this.Item
            with get (r, c) =
                match (r, c) with
                | 0, 0 -> this.m11
                | 0, 1 -> this.m12
                | 0, 2 -> this.m13
                | 0, 3 -> this.m14

                | 1, 0 -> this.m21
                | 1, 1 -> this.m22
                | 1, 2 -> this.m23
                | 1, 3 -> this.m24

                | 2, 0 -> this.m31
                | 2, 1 -> this.m32
                | 2, 2 -> this.m33
                | 2, 3 -> this.m34

                | 3, 0 -> this.m41
                | 3, 1 -> this.m42
                | 3, 2 -> this.m43
                | 3, 3 -> this.m44
                | _ -> failwithf "(%i, %i) cannot index into M4" r c

        static member (*)(l, r) =
            { m11 = l.m11 * r.m11 + l.m12 * r.m21 + l.m13 * r.m31 + l.m14 * r.m41
              m12 = l.m11 * r.m12 + l.m12 * r.m22 + l.m13 * r.m32 + l.m14 * r.m42
              m13 = l.m11 * r.m13 + l.m12 * r.m23 + l.m13 * r.m33 + l.m14 * r.m43
              m14 = l.m11 * r.m14 + l.m12 * r.m24 + l.m13 * r.m34 + l.m14 * r.m44

              m21 = l.m21 * r.m11 + l.m22 * r.m21 + l.m23 * r.m31 + l.m24 * r.m41
              m22 = l.m21 * r.m12 + l.m22 * r.m22 + l.m23 * r.m32 + l.m24 * r.m42
              m23 = l.m21 * r.m13 + l.m22 * r.m23 + l.m23 * r.m33 + l.m24 * r.m43
              m24 = l.m21 * r.m14 + l.m22 * r.m24 + l.m23 * r.m34 + l.m24 * r.m44

              m31 = l.m31 * r.m11 + l.m32 * r.m21 + l.m33 * r.m31 + l.m34 * r.m41
              m32 = l.m31 * r.m12 + l.m32 * r.m22 + l.m33 * r.m32 + l.m34 * r.m42
              m33 = l.m31 * r.m13 + l.m32 * r.m23 + l.m33 * r.m33 + l.m34 * r.m43
              m34 = l.m31 * r.m14 + l.m32 * r.m24 + l.m33 * r.m34 + l.m34 * r.m44

              m41 = l.m41 * r.m11 + l.m42 * r.m21 + l.m43 * r.m31 + l.m44 * r.m41
              m42 = l.m41 * r.m12 + l.m42 * r.m22 + l.m43 * r.m32 + l.m44 * r.m42
              m43 = l.m41 * r.m13 + l.m42 * r.m23 + l.m43 * r.m33 + l.m44 * r.m43
              m44 = l.m41 * r.m14 + l.m42 * r.m24 + l.m43 * r.m34 + l.m44 * r.m44 }

        static member (*)(l, r: Tuple.T) =
            Tuple.tuple
                (l.m11 * r.x + l.m12 * r.y + l.m13 * r.z + l.m14 * r.w)
                (l.m21 * r.x + l.m22 * r.y + l.m23 * r.z + l.m24 * r.w)
                (l.m31 * r.x + l.m32 * r.y + l.m33 * r.z + l.m34 * r.w)
                (l.m41 * r.x + l.m42 * r.y + l.m43 * r.z + l.m44 * r.w)

    let create m11 m12 m13 m14 m21 m22 m23 m24 m31 m32 m33 m34 m41 m42 m43 m44 =
        { m11 = m11
          m12 = m12
          m13 = m13
          m14 = m14
          m21 = m21
          m22 = m22
          m23 = m23
          m24 = m24
          m31 = m31
          m32 = m32
          m33 = m33
          m34 = m34
          m41 = m41
          m42 = m42
          m43 = m43
          m44 = m44 }

    let ofArray (a: array<float>) =
        create a[0] a[1] a[2] a[3] a[4] a[5] a[6] a[7] a[8] a[9] a[10] a[11] a[12] a[13] a[14] a[15]

    let identity = create 1. 0. 0. 0. 0. 1. 0. 0. 0. 0. 1. 0. 0. 0. 0. 1.

    let submatrix (m: T) row col =
        M3.ofArray
            [| for r in 0..3 do
                   for c in 0..3 do
                       if r <> row && c <> col then
                           m[r, c] |]


    let transpose m =
        create m.m11 m.m21 m.m31 m.m41 m.m12 m.m22 m.m32 m.m42 m.m13 m.m23 m.m33 m.m43 m.m14 m.m24 m.m34 m.m44

    let minor m row col = submatrix m row col |> M3.determinant

    let cofactor m row col =
        if (row + col) % 2 = 1 then
            -(minor m row col)
        else
            minor m row col

    let determinant m =
        m.m11 * (cofactor m 0 0)
        + m.m12 * (cofactor m 0 1)
        + m.m13 * (cofactor m 0 2)
        + m.m14 * (cofactor m 0 3)

    let inverse m =
        let d = determinant m

        [| for r in 0..3 do
               for c in 0..3 do
                   (cofactor m c r) / d |]
        |> ofArray


type T =
    | M4 of M4.T
    | M3 of M3.T
    | M2 of M2.T

    member this.Item
        with get (r, c) =
            match this with
            | M4 m -> m[r, c]
            | M3 m -> m[r, c]
            | M2 m -> m[r, c]

    static member (*)(l, r) =
        match (l, r) with
        | (M4 l'), (M4 r') -> l' * r' |> M4
        | _ -> failwith "multiplication not implemented"

    static member (*)(l, r) =
        match r with
        | (M4 r') -> l * r' |> M4
        | _ -> failwith "multiplication not implemented"

    static member (*)(l, (r: Tuple.T)) =
        match l with
        | (M4 l') -> l' * r
        | _ -> failwith "multiplication not implemented"

let ofSeq s =
    let a = Array.ofSeq s

    match a.Length with
    | 16 -> M4.ofArray a |> M4
    | 9 -> M3.ofArray a |> M3
    | _ -> M2.ofArray a |> M2

let identity = M4.identity |> M4

let inverse m =
    match m with
    | M4 m' -> M4.inverse m' |> M4
    | M3 _ -> failwith "cannot inverse M3"
    | M2 _ -> failwith "cannot inverse M2"

let transpose m =
    match m with
    | M4 m' -> M4.transpose m' |> M4
    | M3 _ -> failwith "cannot transpose M3"
    | M2 _ -> failwith "cannot transpose M2"

let determinant m =
    match m with
    | M2 m' -> M2.determinant m'
    | M3 m' -> M3.determinant m'
    | M4 m' -> M4.determinant m'

let submatrix m r c =
    match m with
    | M3 m' -> M3.submatrix m' r c |> M2
    | M4 m' -> M4.submatrix m' r c |> M3
    | _ -> failwith "cannot get submatrix from M2"

let minor m r c =
    match m with
    | M3 m' -> M3.minor m' r c
    | M4 m' -> M4.minor m' r c
    | _ -> failwith "cannot get minor from M2"

let cofactor m r c =
    match m with
    | M3 m' -> M3.cofactor m' r c
    | M4 m' -> M4.cofactor m' r c
    | _ -> failwith "cannot get cofactor from M2"

