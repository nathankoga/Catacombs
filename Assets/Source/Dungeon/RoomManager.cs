using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // refactor all room logic 
    // (check for overlaps, store individual rooms, generate random rooms)
    // into this class: RoomManager   
    
}

class Room 
{ 
    private Vector2Int pos;   // origin coords for <botLeft, botRight>
    private Vector2Int size;  // vector for <width, height>
    
    public Room(Vector2Int roomPos, Vector2Int roomSize){
        pos = roomPos;   // constructor 
        size = roomSize; 
        }

    public int getMinX() { return pos[0]; }
    public int getMaxX() { return pos[0] + size[0]; }
    public int getMinY() { return pos[1]; }
    public int getMaxY() { return pos[1] + size[1]; }

    public bool intersects(Room other){
        // rooms intersect if projections overlap on both axes (both have self.max > other.min)
        // int lPad = -1; int rPad = 1; int uPad = -1; int dPad = 1;  
        // try to add padding between rooms later?       
        bool xOverlap = (this.getMaxX() > other.getMinX()) & (other.getMaxX() > this.getMinX());
        bool yOverlap = (this.getMaxY() > other.getMinY()) & (other.getMaxY() > this.getMinY());
        return xOverlap & yOverlap;
    }
    
}
