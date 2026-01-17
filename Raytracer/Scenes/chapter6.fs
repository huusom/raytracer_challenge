module Raytracer.Scenes.Chapter6

open Raytracer.Math
open Raytracer.Graphics
open Raytracer

let name = "chapter6"

let render (camera: Scene.Camera.T) =
    let ray_origin = Tuple.pointOf 0 0 -5
    let wall_z = 10
    let wall_size = 7
    let canvas_pixels = camera.hsize
    let pixel_size = (float wall_size) / (float canvas_pixels)
    let half = (float wall_size) / 2.
    let canvas = Canvas.create canvas_pixels canvas_pixels

    let sphere =
        Color.create 1 0.2 1
        |> Material.create 0.1 0.9 0.9 200
        |> Geometry.Shape.sphereOf Geometry.Transformation.identity

    let light = Scene.Light.pointLightOf (Tuple.pointOf -10 10 -10) Color.white

    for y in 0 .. (canvas_pixels - 1) do
        let world_y = half - pixel_size * (float y)

        for x in 0 .. (canvas_pixels - 1) do
            let world_x = -half + pixel_size * (float x)
            let position = Tuple.pointOf world_x world_y wall_z

            let r = Tuple.normalize (position - ray_origin) |> Geometry.Ray.create ray_origin

            let hit =
                Geometry.Intersection.intersectionsOf sphere r |> Geometry.Intersection.hitFrom

            match hit with
            | None -> ()
            | Some i ->
                let point = Geometry.Ray.positionFrom r i.t
                let normal = sphere.normal point
                let eye = -r.direction
                canvas[x, y] <- Material.lightningFrom sphere.material light point eye normal false

    canvas
