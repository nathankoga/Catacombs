"""
A module containing a debug dungeon generation interface.
(Ran on Python 3.8)
"""
import random
from typing import List, Dict, Optional
from enum import IntEnum, auto


class Tile(IntEnum):
    Ground = auto()
    Path = auto()
    Wall = auto()

    def getLetter(self):
        return {
            Tile.Ground: 'x',
            Tile.Path: 'O',
            Tile.Wall: '-',
        }.get(self)


class Entity(IntEnum):
    Enemy = auto()
    Chest = auto()
    Shop = auto()
    Boss = auto()
    Spawn = auto()

    def getLetter(self):
        return {
            Entity.Enemy: 'e',
            Entity.Chest: 'g',
            Entity.Shop: 's',
            Entity.Boss: 'b',
            Entity.Spawn: 'c',
        }.get(self)


class Vector2(list):
    """
    Really basic 2d vector class.
    (Please. Do not ask me why it inherits from list.)
    """

    def __init__(self, x: int, y: int):
        super().__init__([x, y])

    @property
    def x(self):
        return self[0]

    @property
    def y(self):
        return self[1]

    def __add__(self, other):
        return Vector2(self.x + other.x, self.y + other.y)

    def __repr__(self):
        return f'Vector2({self.x}, {self.y})'

    def __hash__(self):
        if self.x > 999_999:
            raise AttributeError("vec2 hash undefined for x > 999,999")
        return self.x + (self.y * 1_000_000)


class Dungeon:
    """
    A class interface providing the mapping for a dungeon.
    """

    def __init__(self, size: int):
        self.size: int = size
        self.tiles: Dict[Vector2, Tile] = {}
        self.entities: Dict[Vector2, Entity] = {}

        # Create the base map.
        for x in range(size):
            for y in range(size):
                self.tiles[Vector2(x, y)] = Tile.Wall

    def setTile(self, pos: Vector2, tile: Tile) -> None:
        self.tiles[pos] = tile

    def setEntity(self, pos: Vector2, entity: Optional[Entity]) -> None:
        if entity:
            self.entities[pos] = entity
        else:
            self.entities.pop(pos)

    def getTile(self, pos: Vector2) -> Optional[Tile]:
        return self.tiles.get(pos)

    def getEntity(self, pos: Vector2) -> Optional[Entity]:
        return self.entities.get(pos)

    def printMap(self) -> None:
        mapStr = ""
        for y in range(self.size):
            for x in range(self.size):
                vec = Vector2(x, y)
                entity = self.entities.get(vec)
                tile = self.tiles.get(vec)
                if entity:
                    mapStr += entity.getLetter()
                elif tile:
                    mapStr += tile.getLetter()
                else:
                    print(self.tiles[vec])
                    raise KeyError("this should never happen, but if it does you messed up baaaad 🐑🐑")
            mapStr += '\n'

        # Print the map.
        print(f"Dungeon Map - {self.size}x{self.size}")
        print(mapStr)

    def getRandomClearTile(self, requiredTile: Optional[Tile] = None) -> Vector2:
        """
        Returns the location of a random tile w/o an entity present.
        """
        tilePos = tuple(self.tiles.items())
        while True:
            vec, tile = random.choice(tilePos)
            if requiredTile is not None and tile != requiredTile:
                continue
            if tile == Tile.Wall:
                continue
            if vec in self.entities:
                continue
            return vec


class DungeonGenerator:
    """
    Generates a random dungeon, using a dungeon as input.
    """

    class Room:

        def __init__(self, pos: Vector2, size: Vector2):
            self.pos = pos
            self.size = size
            self.pathOrigin: Vector2 = self.getRandomInBounds()

        def __repr__(self):
            return f'Room({self.getLeft()}, {self.getRight()}, {self.getUp()}, {self.getDown()})'

        @classmethod
        def createRandomRoom(cls, dungeonSize: int, minRoomSize: int, maxRoomSize: int) -> 'Room':
            xsize = random.randint(minRoomSize, maxRoomSize)
            ysize = random.randint(minRoomSize, maxRoomSize)
            xpos = random.randint(0, dungeonSize - xsize)
            ypos = random.randint(0, dungeonSize - ysize)
            return cls(Vector2(xpos, ypos), Vector2(xsize, ysize))

        def intersects(self, other: 'Room') -> bool:
            """
            Returns True if these two rooms intersect.
            """
            lPad = -1
            rPad = 1
            uPad = -1
            dPad = 1
            leftInBounds  = (other.getLeft() + lPad) <= self.getLeft()  <= (other.getRight() + rPad)
            rightInBounds = (other.getLeft() + lPad) <= self.getRight() <= (other.getRight() + rPad)
            topInBounds   = (other.getUp() + uPad)   <= self.getUp()    <= (other.getDown() + dPad)
            downInBounds  = (other.getUp() + uPad)   <= self.getDown()  <= (other.getDown() + dPad)
            xbounded = leftInBounds or rightInBounds
            ybounded = topInBounds or downInBounds
            return xbounded and ybounded

        def setToDungeon(self, dungeon: Dungeon):
            for x in range(self.getLeft(), self.getRight()):
                for y in range(self.getUp(), self.getDown()):
                    dungeon.setTile(Vector2(x, y), Tile.Ground)

        def getLeft(self):
            return self.pos.x

        def getRight(self):
            return self.pos.x + self.size.x

        def getUp(self):
            return self.pos.y

        def getDown(self):
            return self.pos.y + self.size.y

        def getPathOrigin(self) -> Vector2:
            return self.pathOrigin

        def getRandomInBounds(self) -> Vector2:
            return Vector2(random.randint(self.getLeft() + 1, self.getRight() - 1), random.randint(self.getUp() + 1, self.getDown() - 1))

    class Path:

        def __init__(self, roomA: 'DungeonGenerator.Room', roomB: 'DungeonGenerator.Room'):
            self.roomA: DungeonGenerator.Room = roomA
            self.roomB: DungeonGenerator.Room = roomB
            self.vecPath = self._calculateVectorPath()

        def _calculateVectorPath(self) -> List[Vector2]:
            """
            Calculates a vector path list of all path tiles in between the two.
            """
            start = self.roomA.getPathOrigin()
            end = self.roomB.getPathOrigin()
            currentVec = start
            vecPath = [currentVec]

            while currentVec.x != end.x:
                direction = 1 if end.x > currentVec.x else -1
                currentVec = currentVec + Vector2(direction, 0)
                vecPath.append(currentVec)

            while currentVec.y != end.y:
                direction = 1 if end.y > currentVec.y else -1
                currentVec = currentVec + Vector2(0, direction)
                vecPath.append(currentVec)

            return vecPath

        def distance(self) -> int:
            """
            Gets the tile distance between the two rooms.
            """
            return len(self.vecPath)

        def setToDungeon(self, dungeon: Dungeon):
            for vec in self.vecPath:
                dungeon.setTile(vec, Tile.Path)

        def getRoomA(self):
            return self.roomA

        def getRoomB(self):
            return self.roomB

    class PathTree(list):

        def __init__(self, size: int):
            super().__init__()
            self.size = size

        def sortByLength(self):
            """Sorts the PathTree list by length of longest path."""
            newList = sorted(self, key=lambda p: p.distance())
            self.clear()
            self.extend(newList)

        def cull(self, rooms: List['DungeonGenerator.Room'], bonusPathRatio: float = 0.0):
            """
            Performs Kruskal's Algorithm to cull the path tree.
            """
            # First, sort by length.
            self.sortByLength()

            # Set up consts.
            newPath: List[DungeonGenerator.Path] = []
            extraPaths: List[DungeonGenerator.Path] = []

            # Algo consts/functions.
            i, e = 0, 0
            vertices = len(rooms)
            parent = [v for v in range(vertices)]
            rank = [0] * vertices

            def find(roomIndex: int):
                if parent[roomIndex] == roomIndex:
                    return roomIndex
                return find(parent[roomIndex])

            def union(roomA: int, roomB: int):
                aRoot = find(roomA)
                bRoot = find(roomB)
                if rank[aRoot] < rank[bRoot]:
                    parent[aRoot] = bRoot
                elif rank[aRoot] > rank[bRoot]:
                    parent[bRoot] = aRoot
                else:
                    parent[bRoot] = aRoot
                    rank[aRoot] += 1

            while e < (vertices - 1):
                path = self[i]
                i += 1
                roomA = rooms.index(path.getRoomA())
                roomB = rooms.index(path.getRoomB())
                x = find(roomA)
                y = find(roomB)
                if x != y:
                    e = e + 1
                    newPath.append(path)
                    union(x, y)
                else:
                    extraPaths.append(path)

            # Add bonus paths (aka, paths outside of the minimum viable spanning tree.)
            extraPaths.extend(self[i:])
            random.shuffle(extraPaths)
            bonusPathCount = round(len(extraPaths) * bonusPathRatio)
            for i in range(bonusPathCount):
                newPath.append(extraPaths.pop())

            self.clear()
            self.extend(newPath)
            return True

    def __init__(self,
                 dungeonSize: int = 16,
                 minRoomSize: int = 3,
                 maxRoomSize: int = 5,
                 roomCount: int = 7,
                 bonusPathRatio: float = 0.25,
                 shopCount: int = 1,
                 enemyCount: int = 4,
                 chestCount: int = 2):
        # Constant definitions
        self.dungeonSize = dungeonSize
        self.minRoomSize = minRoomSize
        self.maxRoomSize = maxRoomSize
        self.roomCount = roomCount
        self.bonusPathRatio = bonusPathRatio
        self.shopCount = shopCount
        self.enemyCount = enemyCount
        self.chestCount = chestCount

        # Dungeon state.
        self.rooms: List[DungeonGenerator.Room] = []
        self.paths: DungeonGenerator.PathTree[DungeonGenerator.Path] = DungeonGenerator.PathTree(dungeonSize)

    def generate(self) -> Dungeon:
        """
        Generates a random dungeon.
        """
        dungeon = Dungeon(self.dungeonSize)

        # Create rooms.
        for roomIndex in range(self.roomCount):
            # Run several attempts to generate a room.
            for attempt in range(100):
                # Make a random room.
                room = DungeonGenerator.Room.createRandomRoom(self.dungeonSize, self.minRoomSize, self.maxRoomSize)

                # This room is valid if it has no intersections.
                intersects = False
                for existingRoom in self.rooms:
                    intersects = room.intersects(existingRoom)
                    if intersects:
                        break
                if intersects:
                    continue

                # Add this room to the collection.
                self.rooms.append(room)
                break

        # Create paths.
        for i, roomA in enumerate(self.rooms):
            for o, roomB in enumerate(self.rooms):
                if roomA is roomB:
                    continue
                if o < i:
                    continue
                path = DungeonGenerator.Path(roomA, roomB)
                self.paths.append(path)

        # Clear out unnecessary paths.
        self.paths.cull(self.rooms, self.bonusPathRatio)

        # Update the dungeon to match our dataclasses.
        for room in self.rooms:
            room.setToDungeon(dungeon)
        for path in self.paths:
            path.setToDungeon(dungeon)

        # Set entities.
        # Player spawns in the initial room.
        spawnRoom = self.rooms[0]
        playerSpawnLocation = spawnRoom.getRandomInBounds()
        dungeon.setEntity(playerSpawnLocation, Entity.Spawn)

        # Boss spawns in the room furthest from the player.
        furthestRoom = None
        furthestDist = 0
        for room in self.rooms[1:]:
            path = DungeonGenerator.Path(spawnRoom, room)
            if path.distance() > furthestDist:
                furthestRoom = room
                furthestDist = path.distance()
        bossSpawnLocation = furthestRoom.getRandomInBounds()
        dungeon.setEntity(bossSpawnLocation, Entity.Boss)

        # Spawn misc entities.
        for _ in range(self.shopCount):
            spawnLocation = dungeon.getRandomClearTile(requiredTile=Tile.Ground)
            dungeon.setEntity(spawnLocation, Entity.Shop)
        for _ in range(self.chestCount):
            spawnLocation = dungeon.getRandomClearTile(requiredTile=Tile.Ground)
            dungeon.setEntity(spawnLocation, Entity.Chest)
        for _ in range(self.enemyCount):
            spawnLocation = dungeon.getRandomClearTile()
            dungeon.setEntity(spawnLocation, Entity.Enemy)

        # Return the dungeon.
        return dungeon


# Create a dungeon, generate, and print it.
dg = DungeonGenerator()
d = dg.generate()
d.printMap()
