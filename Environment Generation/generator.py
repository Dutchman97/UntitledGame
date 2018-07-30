import numpy as np

class Generator:
    def __init__(self):
        # up, right, down, left
        self.direction_vectors = [(0,-1),(1,0),(0,1),(-1,0)]
        self.forward_steps = 2

    def generate(self, shape, chance_array):
        values = np.zeros(shape, dtype=np.float32)
        positions = [(int(shape[0]/2) - 1, shape[1] - 1)]
        directions = [0]
        while len(positions) > 0:
            print(len(positions))
            values, positions, directions = self.path(values, positions, directions, chance_array)

        while False: # np.sum(values) < shape[0] * shape[1] * 0.15:
            values = np.zeros(shape, dtype=np.float32)
            positions = [(int(shape[0]/2) - 1, shape[1] - 1)]
            directions = [0]
            while len(positions) > 0:
                print(len(positions))
                values, positions, directions = self.path(values, positions, directions, chance_array)

        print('sum:', np.sum(values), '/', shape[0] * shape[1], '-', 100 * np.sum(values) / (shape[0] * shape[1]), '%')
        return values

    def path(self, values1, positions1, directions1, chance_array):
        position = positions1[0]
        positions = positions1[1:]
        direction = directions1[0]
        directions = directions1[1:]
        values = values1

        print('position:', position)

        shape = np.shape(values)
        if position[0] >= shape[0] or position[1] >= shape[1] or position[0] < 0 or position[1] < 0:
            print('returning 0')
            return values, positions, directions
        if values[position[1]][position[0]] == 1:
            print('returning 1')
            return values, positions, directions

        values[position[1]][position[0]] = 1

        chance = chance_array[position[1]][position[0]]
        print('chance:', chance)

        a = 0.3
        if chance * a > np.random.rand():
            direction_left = (direction + 1) % 4
            direction_vector_left = self.direction_vectors[direction_left]
            position_left = (position[0]+direction_vector_left[0], position[1]+direction_vector_left[1])
            values, position_left = self.forward(values, position_left, direction_left)
            positions.append(position_left)
            directions.append(direction_left)

        if chance * 3 > np.random.rand():
            direction_forward = (direction + 0) % 4
            direction_vector_forward = self.direction_vectors[direction_forward]
            position_forward = (position[0]+direction_vector_forward[0], position[1]+direction_vector_forward[1])
            values, position_forward = self.forward(values, position_forward, direction_forward)
            positions.append(position_forward)
            directions.append(direction_forward)

        if chance * a > np.random.rand():
            direction_right = (direction - 1) % 4
            direction_vector_right = self.direction_vectors[direction_right]
            position_right = (position[0]+direction_vector_right[0], position[1]+direction_vector_right[1])
            values, position_right = self.forward(values, position_right, direction_right)
            positions.append(position_right)
            directions.append(direction_right)

        return values, positions, directions

    def forward(self, values1, position1, direction1):
        position = position1
        direction_vector = self.direction_vectors[direction1]
        values = values1
        shape = np.shape(values1)

        for i in range(self.forward_steps):
            if position[0] >= shape[0] or position[1] >= shape[1] or position[0] < 0 or position[1] < 0:
                return values, position
            if values[position[1]][position[0]] == 1:
                return values, position

            values[position[1]][position[0]] = 1
            position = (position[0]+direction_vector[0], position[1]+direction_vector[1])

        return values, position
