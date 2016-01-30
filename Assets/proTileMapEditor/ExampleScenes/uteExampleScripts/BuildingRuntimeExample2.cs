using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingRuntimeExample2 : MonoBehaviour {

	// Main class for runtime building
	private uteRuntimeBuilder runtimeBuilder;
	// Category class for mcraft tiles
	private uteRuntimeBuilder.Category mCraftCategory;
	// For UI
	private bool isShow;
	private MouseLook mouseLookX;
	private MouseLook mouseLookY;
	public Texture2D TargetIcon;
	private GameObject Weapon;
	private bool isiOSExample;

	private void Start()
	{
		QualitySettings.shadowDistance = 100;

		Weapon = GameObject.Find("weapon");

		isShow = false;

		// Get runtime builder main class
		runtimeBuilder = (uteRuntimeBuilder) this.gameObject.GetComponent<uteRuntimeBuilder>();

		// Set Raycast distance (to not let build far away)
		runtimeBuilder.SetRaycastDistance(3.0f);

		// Set snapping to be fixed 1.0f
		runtimeBuilder.SetSnapOption("fixed");

		// Set fixed raycast position from Screen point (form center of the screen)
		runtimeBuilder.SetFixedRaycastPosition(new Vector2(Screen.width/2,Screen.height/2));

		// Get mcraft Category
		mCraftCategory = runtimeBuilder.GetCategoryByCategoryName("mcraft");

		// Get mouseLook component to disable or enable looking if UI shown
		if(!GameObject.Find("iOSControls"))
		{
			isiOSExample = false;
			mouseLookX = GameObject.Find("First Person Controller").GetComponent<MouseLook>();
			mouseLookY = GameObject.Find("First Person Controller/Main Camera").GetComponent<MouseLook>();
		}
		else
		{
			isiOSExample = true;

			// Disale shadows for iOS
			GameObject.Find("Directional light").GetComponent<Light>().shadows = LightShadows.None;
		}

		StartCoroutine(GenerateMapProcedurally());
	}

	private IEnumerator GenerateMapProcedurally()
	{
		// Disable mouse Input while map is generating
		runtimeBuilder.DisableMouseInputForBuild();

		int x = 0;
		int xSize = 80;
		int z = 0;
		int zSize = 80;

		uteRuntimeBuilder.Tile tile;

		for(int i=0;i<xSize*zSize;i++)
		{
			// Get Tile from Category by Tile Name
			tile = runtimeBuilder.GetTileFromCategoryByName(mCraftCategory.name, "water");

			// Pass Tile mainObject to RuntimeBuilder
			runtimeBuilder.SetCurrentTileInstantly(tile.mainObject);

			// Place Tile at Vector3 position
			runtimeBuilder.PlaceCurrentTileAtPosition(new Vector3(x-25f,0,z-25f));

			if(x>10&&x<xSize-10&&z>10&&z<zSize-10)
			{
				tile = runtimeBuilder.GetTileFromCategoryByName(mCraftCategory.name, "grass");
				runtimeBuilder.SetCurrentTileInstantly(tile.mainObject);
				runtimeBuilder.PlaceCurrentTileAtPosition(new Vector3(x-25f,1,z-25f));

				if(Random.Range(0,10)==0)
				{
					for(int j=0;j<Random.Range(4,10);j++)
					{
						tile = runtimeBuilder.GetTileFromCategoryByName(mCraftCategory.name, "stone");
						runtimeBuilder.SetCurrentTileInstantly(tile.mainObject);
						runtimeBuilder.PlaceCurrentTileAtPosition(new Vector3(x-25f,2+j,z-25f));
					}
				}
				else if(Random.Range(0,10)==0)
				{
					for(int k=0;k<Random.Range(2,5);k++)
					{
						tile = runtimeBuilder.GetTileFromCategoryByName(mCraftCategory.name, "dirt");
						runtimeBuilder.SetCurrentTileInstantly(tile.mainObject);
						runtimeBuilder.PlaceCurrentTileAtPosition(new Vector3(x-25f,2+k,z-25f));
					}
				}
			}

			x++;
			if(x==xSize)
			{
				yield return 0;
				x=0;
				z++;
			}
		}

		// Enable mouse Input
		runtimeBuilder.EnableMouseInputForBuild();
		runtimeBuilder.CancelCurrentTile();
	}

	private void Update()
	{
		if(Input.mousePosition.y<150)
		{
			runtimeBuilder.DisableMouseInputForBuild();
			return;
		}
		else
		{
			runtimeBuilder.EnableMouseInputForBuild();
		}

		// Press right mouse button to destroy hovering Tile (if hovering)
		if(Input.GetMouseButtonDown(1))
		{
			// Get GameObject which is mouse hovering
			if(runtimeBuilder.GetCurrentSelectedObject())
			{
				// Destroy selected tile
				bool isSuccess = runtimeBuilder.DestroyCurrentSelectedObject();

				if(isSuccess)
				{
					Debug.Log("Object Destroyed");
				}
				else
				{
					Debug.Log("No Object Selected");
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Tab))
		{
			ShowOrHideUI();
		}

		// Weapon animation
		if(Input.GetMouseButtonDown(0))
		{
			Weapon.transform.localPosition = new Vector3(1.02f,-0.3856888f,1.018038f);
			Weapon.transform.localEulerAngles = new Vector3(33f,0,0);
		}

		if(Input.GetMouseButtonUp(0))
		{
			Weapon.transform.localPosition = new Vector3(1.02f,0.1f,1.0f);
			Weapon.transform.localEulerAngles = new Vector3(-14f,0,0);
		}
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width/2-32,Screen.height/2-32,64,64),TargetIcon);

		if(isShow)
		{
			int x = 0;
			int y = 0;
			int yOffset = 0;
			GUI.Box(new Rect(200,80,520,180),"Items");

			for(int j=0;j<mCraftCategory.allTiles.Count;j++)
			{
				// Get Tile from current cateogry tiles
				uteRuntimeBuilder.Tile tile = (uteRuntimeBuilder.Tile) mCraftCategory.allTiles[j];

				if(GUI.Button(new Rect(210+(x*125),120+(y*125)+yOffset,120,125),tile.name))
				{
					// Set selected tile for building
					runtimeBuilder.SetCurrentTile(tile.mainObject);
					// Hide list
					ShowOrHideUI();
				}

				// Draw Tile preview texture
				GUI.DrawTexture(new Rect(215+(x*125),130+(y*125)+yOffset,110,110),tile.preview);

				// Show tile name
				GUI.Label(new Rect(254+(x*125),100+(y*125)+yOffset,120,125),tile.name);

				x++;
				if(x==8)
				{
					x=0;
					y++;
				}
			}

			yOffset += 125;
			x=0;
		}
		
		if(!isiOSExample)
		{
			GUI.Label(new Rect(Screen.width/2+110,Screen.height-40,200,30),"Press TAB");
		}

		if(Input.mousePosition.y<150)
		{
			return;
		}

		// Show/Hide items
		if(GUI.Button(new Rect(Screen.width/2-100,Screen.height-60,200,50),"Inventory"))
		{
			ShowOrHideUI();
		}
	}

	private void ShowOrHideUI()
	{
		if(isShow)
		{
			isShow = false;

			if(!isiOSExample)
			{
				Screen.lockCursor = true;
				mouseLookX.sensitivityX = 5.0f;
				mouseLookY.sensitivityX = 5.0f;
				mouseLookY.sensitivityY = 5.0f;
			}
		}
		else
		{
			isShow = true;
			if(!isiOSExample)
			{
				Screen.lockCursor = false;
				mouseLookX.sensitivityX = 0.0f;
				mouseLookY.sensitivityX = 0.0f;
				mouseLookY.sensitivityY = 0.0f;
			}
			// Cancel current Tile
			runtimeBuilder.CancelCurrentTile();
		}
	}
}
