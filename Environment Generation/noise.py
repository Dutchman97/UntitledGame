import numpy as np

class Noise:
    @staticmethod
    def white(shape):
        result = np.zeros(shape, dtype=np.float32)
        for y in range(shape[1]):
            for x in range(shape[0]):
                result[y][x] = np.random.rand()
        return result

    @staticmethod
    def white_thresholded(shape, threshold):
        result = np.zeros(shape, dtype=np.float32)
        for y in range(shape[1]):
            for x in range(shape[0]):
                rand = np.random.rand()
                result[y][x] = 1 if rand > threshold else 0
        return result
