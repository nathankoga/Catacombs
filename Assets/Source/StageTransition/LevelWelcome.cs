using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LevelWelcome : MonoBehaviour
{
    public GameObject imageNode;
    public TextMeshProUGUI textMeshPro;

    float timer = 0.0f;

    private float TITLE_HOLD_DURATION = 4.0f;
    private float TITLE_DIE_TIME = 1.0f;
    private float TITLE_MOVE_SPEED = 3.0f;
    void Awake()
    {
        GameState.FloorStart += SpawnLevelGUI;
    }

    void SpawnLevelGUI(GameState gs, DungeonFloor f)
    {
        // Slide the image node up.
        textMeshPro.text = GetLevelName(f);
        timer = TITLE_HOLD_DURATION;
        Vector3 pos = imageNode.transform.position;
        imageNode.transform.position = new Vector3(pos.x, -22 * 8, pos.z);
        imageNode.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (timer <= 0.0f)
        {
            imageNode.SetActive(false);
            return;
        }
        Vector3 pos = imageNode.transform.position;
        if (timer <= TITLE_DIE_TIME)
        {
            imageNode.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, -22.0f * 8.0f, TITLE_MOVE_SPEED * Time.deltaTime), pos.z);
        } else
        {
            imageNode.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, 0.0f, TITLE_MOVE_SPEED * Time.deltaTime), pos.z);
        }
        timer -= Time.deltaTime;
    }

    string GetLevelName(DungeonFloor f)
    {
        switch (f)
        {
            case DungeonFloor.FLOOR1:
                return "GREAT CREEK";
            case DungeonFloor.FLOOR2:
                return "GRAVEL PIT";
            case DungeonFloor.FLOOR3:
                return "HELLMOUTH";
            default:
                return "Idk";
        }
    }
}
