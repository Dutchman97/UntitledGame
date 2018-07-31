import cv2
import numpy as np
from noise import Noise
from generator import Generator

shape = (256, 256)
image = Noise.white(shape)


# kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (3,3))
# a = 2
# image1 = cv2.dilate(image, kernel, iterations=a)
# b = 1
# image = cv2.erode(image1, kernel, iterations=a+b)
# image1 = cv2.dilate(image, kernel, iterations=b)
# image = image1

resized_shape = tuple((x*3 for x in shape))
resized = cv2.resize(image, resized_shape, interpolation=cv2.INTER_NEAREST)

cv2.imshow('image', resized)

image2 = Noise.white_thresholded(tuple((int(x/2) for x in shape)), 0.5)
image2 = cv2.resize(image2, shape, interpolation=cv2.INTER_LINEAR)
image2 = cv2.resize(image2, resized_shape, interpolation=cv2.INTER_NEAREST)

cv2.imshow('image2', image2)

g = Generator()
generated = g.generate(shape, image)
generated = cv2.resize(generated, resized_shape, interpolation=cv2.INTER_NEAREST)

cv2.imshow('generated', generated)

cv2.waitKey()