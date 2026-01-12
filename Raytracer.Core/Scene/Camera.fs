module Raytracer.Scene.Camera

open Raytracer.Math
open Raytracer.Geometry

type T =
    { hsize: int
      vsize: int
      fov: float
      pixel_size: float
      half_width: float
      half_height: float
      mutable transform: Matrix.M4.T }

let create hsize vsize fov pixel_size half_width half_height transform =
    { hsize = hsize
      vsize = vsize
      fov = fov
      pixel_size = pixel_size
      half_width = half_width
      half_height = half_height
      transform = transform }

let init hsize vsize fov =
    let half_view = tan (fov / 2.)
    let aspect = float hsize / float vsize

    let half_width, half_height =
        if aspect >= 1. then
            half_view, half_view / aspect
        else
            half_view * aspect, half_view

    let pixel_size = half_width * 2. / float hsize

    create hsize vsize fov pixel_size half_width half_height Transformation.identity

let ray camera x y = 
    let xoffset = (float x + 0.5) * camera.pixel_size 
    let yoffset = (float y + 0.5) * camera.pixel_size

    let worldx = camera.half_width - xoffset
    let worldy = camera.half_height - yoffset 

    let i = Transformation.inverse camera.transform
    let pixel = i * Tuple.point worldx worldy -1 
    let origin = i * Tuple.point 0 0 0 
    let direction = Tuple.normalize(pixel - origin)

    Ray.create origin direction 

let render camera world = 
    let image = Raytracer.Graphics.Canvas.create camera.hsize camera.vsize 
    for y in 0.. (camera.vsize - 1) do 
        for x in 0..(camera.hsize - 1) do 
            let r = ray camera x y 
            let c = World.color world r 
            image[x,y] <- c 

    image