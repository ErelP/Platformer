using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public Room StartingRoom;

    private Room[,] spawnedRooms;

    private void Start()
    {
        spawnedRooms = new Room[11, 11];
        spawnedRooms[9, 9] = StartingRoom;

        for (int i = 0; i < 12; i++)
        {
            PlaceOneRoom();
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        GameObject[] boss = GameObject.FindGameObjectsWithTag("RoomB");
        GameObject[] treasure = GameObject.FindGameObjectsWithTag("RoomT");

        if (boss.Length >= 2 && treasure.Length >= 3)
        {
            Room newRoom = Instantiate(RoomPrefabs[Random.Range(1, RoomPrefabs.Length - 1)]);

            CreateRoom(newRoom, vacantPlaces);
        }
        else if (boss.Length >= 2)
        {
            Room newRoom = Instantiate(RoomPrefabs[Random.Range(1, RoomPrefabs.Length)]);

            CreateRoom(newRoom, vacantPlaces);
        }
        else
        {
            Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);

            CreateRoom(newRoom, vacantPlaces);
        }
    }

    public void CreateRoom(Room nR, HashSet<Vector2Int> vP)
    {
        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vP.ElementAt(Random.Range(0, vP.Count));
            nR.RotateRandomly();

            if (ConnectToSomething(nR, position))
            {
                nR.transform.position = new Vector3(position.x - 9, 0, position.y - 9) * 20;
                spawnedRooms[position.x, position.y] = nR;
                return;
            }
        }

        Destroy(nR.gameObject);
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null) neighbours.Add(Vector2Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null) neighbours.Add(Vector2Int.down);
        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
        }

        return true;
    }
}