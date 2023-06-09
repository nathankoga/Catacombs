using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // refactor all room logic 
    // (check for overlaps, store individual rooms, generate random rooms)
    // into this class: RoomManager   

    private int minRoomSize;
    private int maxRoomSize;
    private int mapSize;
    private int numRooms;
    private int[,] placementMap;  
    private List<Room> finalRooms = new List<Room>();

    public void initRoomManager(int size, int nRooms, int min, int max) {
        mapSize = size;
        numRooms = nRooms;
        minRoomSize = min;
        maxRoomSize = max;
        placementMap = new int[size, size];  // defaults values to 0 in c#, says internet
    }

    void Awake(){ }
    
    public List<Room> getFinalRooms(){ return finalRooms; }

    public void setRooms(){

        Debug.Log("starting");
        int roomsPlaced = 0;
        // Debug.LogFormat("roomsPlaced: {0}   numRooms: {1}", roomsPlaced, numRooms);
        while (roomsPlaced < numRooms){
            // Debug.Log("in the while loop");
            // create a new room
            // Random.Range(inclusive, exclusive);
            Vector2Int roomSize = new Vector2Int(Random.Range(minRoomSize, maxRoomSize + 1), 
                                            Random.Range(minRoomSize, maxRoomSize + 1));
            
            Vector2Int roomPos = new Vector2Int(Random.Range(0, mapSize - roomSize[0] + 1), 
                                            Random.Range(0, mapSize - roomSize[1] + 1));
            Room newRoom = new Room(roomPos, roomSize);

            // check for intersections. If no intersections, add to finalRooms
            bool overlap = false;

            // probably don't need to use quadTree for collisions.
            for (int idx = 0; idx < roomsPlaced; idx++){
                bool intersect = newRoom.intersects(finalRooms[idx]);
                if (intersect){
                    Debug.Log("room failed");
                    overlap = true; 
                    break; 
                }
            }

            if (!overlap){
                finalRooms.Add(newRoom);
                roomsPlaced++;
                Debug.LogFormat("room {0} placed!", roomsPlaced);
            }
        }

        // Print statement to make sure all rooms don't collide 

        for (int curr = 0; curr < numRooms; curr++){
            int xStart = finalRooms[curr].getMinX();
            int xEnd= finalRooms[curr].getMaxX();
            int yStart = finalRooms[curr].getMinY();
            int yEnd= finalRooms[curr].getMaxY();

            Debug.LogFormat("Room {0}: bot left: ({1},{2}) top right: ({3},{4})", curr, xStart, yStart, xEnd, yEnd); 
        }
    }
}

// public so that it's returnable in setRooms()
public class Room 
{ 
    private Vector2Int pos;   // origin coords for <botLeft, botRight>
    private Vector2Int size;  // vector for <width, height>
    public Vector2Int pathOrigin;
    
    public Room(Vector2Int roomPos, Vector2Int roomSize){
        pos = roomPos;   // constructor 
        size = roomSize; 
        pathOrigin = GetRandomInBounds();
        }

    public int getMinX() { return pos[0]; }
    public int getMaxX() { return pos[0] + size[0]; }
    public int getMinY() { return pos[1]; }
    public int getMaxY() { return pos[1] + size[1]; }

    public int getHeight()
    {
        return size[1];
    }

    public Vector2Int GetRandomInBounds()
    {
        return new Vector2Int(Random.Range(getMinX(), getMaxX()), Random.Range(getMinY(), getMaxY()));
    }

    public bool intersects(Room other){
        // rooms intersect if projections overlap on both axes (both have self.max > other.min)
        // int lPad = -1; int rPad = 1; int uPad = -1; int dPad = 1;  
        // try to add padding between rooms later?       
        bool xOverlap = (this.getMaxX() >= other.getMinX()) & (other.getMaxX() >= this.getMinX());
        bool yOverlap = (this.getMaxY() >= other.getMinY()) & (other.getMaxY() >= this.getMinY());
        return xOverlap & yOverlap;
    }

    public bool ContainsTile(Vector2 tilePos)
    {
        // check if a tile is within the bounds of the room
        return (tilePos[0] >= pos[0]) & (tilePos[0] < pos[0] + size[0]) & (tilePos[1] >= pos[1]) & (tilePos[1] < pos[1] + size[1]);
    }

    public Vector2 GetCenter()
    {
        return new Vector2(pos[0] + size[0] / 2, pos[1] + size[1] / 2);
    }
}
