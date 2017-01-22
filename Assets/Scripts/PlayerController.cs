using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject[] turretPrefabs;

    public GameObject turret;//Should change prefab when a button is clicked, for now this will just create the temp turret
    public Sprite CorrectPlacement;
    public Sprite WrongPlacement;

    private bool placementMode;
    private bool canPlace = false;

    void Start () {
		
	}
	
	void FixedUpdate ()
    {
        if (placementMode)
        {
            Placement();
        }
        else
        {
            canPlace = false;
        }

        if (Input.GetKeyDown("e"))
        {
            placementMode = !placementMode;
            print(placementMode);
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
    }

    void Placement()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos.x = (float)Mathf.Floor(mousePos.x) + 0.5f;//Rounds down to nearest x then adds 0.5 to get the center point of the transform of the nearest grid square.
        mousePos.y = (float)Mathf.Floor(mousePos.y) + 0.5f;//Same as above.
        gameObject.transform.position = mousePos;//set position of cursor object to rounded mouse position.

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f); //issue with going off grid, can probably fix by if Switched to if(raycast), Also problem with turret range collider invalidating placement may rework turret range
        if (hit && hit.transform.tag == "Placeable")
        {
            GetComponent<SpriteRenderer>().sprite = CorrectPlacement;
            canPlace = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = WrongPlacement;
            canPlace = false;
        }

        if (Input.GetButton("Fire1") && canPlace)//If lmb is pressed and canPlace is true
        {
            Instantiate(turret, mousePos, transform.rotation);
            placementMode = !placementMode;
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
    }

    public void SetSunGun()
    {
        // sets the turret type for placement by the index in the prefab array
        print("Setting SG");
        SetTurretType(0);

        // if the placement mode is not turned on, 
        if (!placementMode)
        {
            // then turn it on
            placementMode = true;
            Placement();
        }
    }

    public void SetSlickShooter()
    {
        print("Setting SS");
        SetTurretType(1);

        if (!placementMode)
        {
            placementMode = true;
            Placement();
        }
    }

    public void SetWaterWhomper()
    {
        print("Setting WW");
        SetTurretType(2);

        if (!placementMode)
        {
            placementMode = true;
            Placement();
        }
    }

    public void SetKebabBalista()
    {
        print("Setting KB");
        SetTurretType(3);

        if (!placementMode)
        {
            placementMode = true;
            Placement();
        }
    }

    public void SetPushOffParasol()
    {
        print("Setting POP");
        SetTurretType(4);

        if (!placementMode)
        {
            placementMode = true;
            Placement();
        }
    }

    public void SetBigBloodyCannon()
    {
        print("Setting BFG");
        SetTurretType(5);

        if (!placementMode)
        {
            placementMode = true;
            Placement();
        }
    }

    private void SetTurretType(int turretType)
    {
        turret = turretPrefabs[turretType];
    }
}
