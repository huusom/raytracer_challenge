module Raytracer.Presets.Chapter5

open Raytracer.Math
open Raytracer.Graphics
open Raytracer

let name = "chapter5"

let render (camera: Scene.Camera.T) =
    let ray_origin = Tuple.pointOf 0 0 -5
    let wall_z = 10
    let wall_size = 7
    let canvas_pixels = camera.hsize
    let pixel_size = (float wall_size) / (float canvas_pixels)
    let half = (float wall_size) / 2.
    let canvas = Canvas.create canvas_pixels canvas_pixels
    let color = Color.create 1 0 0

    let sphere =
        color
        |> Material.create 1 1 1 1
        |> Geometry.Shape.sphereOf Geometry.Transformation.identity

    for y in 0 .. (canvas_pixels - 1) do
        let world_y = half - pixel_size * (float y)

        for x in 0 .. (canvas_pixels - 1) do
            let world_x = -half + pixel_size * (float x)
            let position = Tuple.pointOf world_x world_y wall_z

            let hit =
                Tuple.normalize (position - ray_origin)
                |> Geometry.Ray.create ray_origin
                |> Geometry.Intersection.intersectionsOf sphere
                |> Geometry.Intersection.hitFrom


            if Option.isSome hit then
                canvas[x, y] <- color
    
    canvas
