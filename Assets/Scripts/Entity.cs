using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Entity : MonoBehaviour
{
    private bool isThisGameObjectPlayer;

    [SerializeField] private float health = 3f;
    [SerializeField] private float radiationLevel = 0f;
    [SerializeField] private float maxRadiationLevel = 20f;
    [SerializeField] private float timeForNullRadiation = 5;
    [SerializeField] public bool isInRadiation = false;
    
    private UIControl uiControl;
    private float startHealth;

    private GameObject tilemapGameObject;
    private Tilemap tilemap;

    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile openChestTile;
    private string gearTileName = GlobalFields.gearTileName;

    // [SerializeField] private PointCounter pointCounter;
    [SerializeField] private GearCounter gearsCounter;
    private int gearsCount = 0;

    //public float RadiationLevel { get => radiationLevel; set => radiationLevel = value; }

    private void Start()
    {
        startHealth = health;
        uiControl = GameObject.FindGameObjectsWithTag(GlobalFields.canvasTag)[0].GetComponent<UIControl>();

        if (GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag).Length > 0)
        {
            tilemapGameObject = GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag)[0];
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }

        isThisGameObjectPlayer = gameObject.tag == GlobalFields.playerTag;
        if (isThisGameObjectPlayer)
        {
            new Points();
        }
    }

    public void AddRadiation(float radLevel)
    {
        radiationLevel += radLevel;
        if (isThisGameObjectPlayer) uiControl.radiationIndicatorWidth = radiationLevel / maxRadiationLevel;
        if (radiationLevel >= maxRadiationLevel)
        {
            Debug.Log(gameObject.name + " Dead by radiation");
            gameObject.SendMessage("Death by radiation", null, SendMessageOptions.DontRequireReceiver);
            Death();
        }
    }

    private void AddHealth(float _delta)
    {
        health += _delta;
        if (health > startHealth) health = startHealth;
        if (isThisGameObjectPlayer) uiControl.healthIndicatorWidth = health / startHealth;
    }

    private void ReduceHealth(float _delta)
    {
        health -= _delta;
        if (isThisGameObjectPlayer)
        {
            uiControl.healthIndicatorWidth = health / startHealth;
            SoundManager soundManager = SoundManager.getInstance();
            soundManager.PlaySound(soundManager.damageClip, 0.3f);
        }

        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is Dead");
            gameObject.SendMessage("Death", null, SendMessageOptions.DontRequireReceiver);
            Death();
        }
    }
    
    private void Death()
    {
        if (isThisGameObjectPlayer)
        {
            uiControl.OpenGameOverMenu();
            Time.timeScale = 0f;
            SoundManager soundManager = SoundManager.getInstance();
            soundManager.PlaySound(soundManager.deathClip, 1f);
            // ������� ����������� ����� � ��������� �� � �������
            
        }
        if(gameObject.tag == GlobalFields.tronedTag)
        {
            uiControl.OpenWinMenu();
            Time.timeScale = 0f;
            Points.getCurrentInstance().pointCounter += Points.pointsForTroned;
            Debug.Log($"{Points.getCurrentInstance().pointCounter} points");
        }
        if (gameObject.tag == GlobalFields.vrudniTag)
        {
            Points.getCurrentInstance().pointCounter += (int)Mathf.Sqrt(Points.CurrentChunk) * Points.pointsForVruden;
            Debug.Log($"{Points.getCurrentInstance().pointCounter} points");
        }
        if (gameObject.tag == GlobalFields.grohogTag)
        {
            Points.getCurrentInstance().pointCounter += (int)Mathf.Sqrt(Points.CurrentChunk) * Points.pointsForGrohog;
            Debug.Log($"{Points.getCurrentInstance().pointCounter} points");
        }
        Destroy(gameObject);
    }

    public IEnumerator NullRadiationAfterSomeSeconds()
    {
        yield return new WaitForSeconds(timeForNullRadiation);
        if (!isInRadiation)
        {
            this.radiationLevel = 0f;
            uiControl.radiationIndicatorWidth = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isThisGameObjectPlayer) return;
        
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                Vector3Int vector = tilemap.WorldToCell(hitPosition);
                TileBase tile = tilemap.GetTile(vector);

                if (tile != null && tile.name == gearTileName)
                {
                    SoundManager soundManager = SoundManager.getInstance();
                    soundManager.PlaySound(soundManager.gearPickUpClip, 0.8f);
                    tilemap.SetTile(vector, floorTile);
                    gearsCount++;
                    gearsCounter.SetCount(gearsCount);
                    Points.getCurrentInstance().pointCounter += Points.pointsForGear * gearsCount;
                    Debug.Log($"{Points.getCurrentInstance().pointCounter} points");
                }

                if (tile != null && tile.name == GlobalFields.chestTileName)
                {
                    tilemap.SetTile(vector, openChestTile);
                    GetBonus();
                }
            }
        }
    }

    private void GetBonus()
    {
        int bonusType = Random.Range(0, 1);
        switch(bonusType)
        {
            case 0:
                {
                    AddHealth(Random.Range(1, 4));
                    break;
                }
            case 1:
                {
                    break;
                }
        }
    }
}

enum BonusType
{
    PlusHealth = 0,
    RadiationShield = 1
}
