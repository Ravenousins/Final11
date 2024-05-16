using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject[] monsterPrefabs;
    private GameObject selectedMonster;
    private int selectedMonsterCost;
    [SerializeField] private float towerSize = 1f;
    [SerializeField] private LayerMask nonPlaceableMask;

    private List<Vector3> towerPositions = new List<Vector3>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle the active state of the UI
            uiObject.SetActive(!uiObject.activeSelf);
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedMonster = null;
        }

        if (Input.GetMouseButtonDown(0) && selectedMonster != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 10;

            if (CanPlaceTower(mousePos) && LevelManager.main.souls >= selectedMonsterCost)
            {
                LevelManager.main.SpendSouls(selectedMonsterCost);
                Instantiate(selectedMonster, mousePos, Quaternion.identity);
                selectedMonster = null;

                towerPositions.Add(mousePos);
            }
        }
    }

    public void SelectMonster(int monsterIndex)
    {
        Debug.Log("monsterIndex: " + monsterIndex);
        selectedMonster = monsterPrefabs[monsterIndex];
        Debug.Log("selectedMonster: " + selectedMonster);
        Turret selectedTower = selectedMonster.GetComponentInChildren<Turret>();
        Debug.Log("selectedTower: " + selectedTower);
        selectedMonsterCost = selectedTower.cost;
        Debug.Log("selectedMonsterCost: " + selectedMonsterCost);
    }
    public void OnCancelButtonClick()
    {
        // Cancel building with button click
        selectedMonster = null;
    }

    private bool CanPlaceTower(Vector3 position)
    {
        if (Physics2D.OverlapCircle(position, towerSize, nonPlaceableMask))
        {
            return false;
        }

        foreach (Vector3 towerPosition in towerPositions)
        {
            if (Vector3.Distance(position, towerPosition) < towerSize)
            {
                // If there's a tower too close to the desired position, return false
                return false;
            }
        }

        return true;
    }
}
