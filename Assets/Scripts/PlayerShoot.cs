using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(weaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
        
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;


    private PlayerWeapon currentWeapon;
    private weaponManager weaponManager; 

    void Start()
    {
         if (cam == null)
        {
            Debug.Log("Player Shoot: No Camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<weaponManager>();
    }
    void Update()
    {

        currentWeapon = weaponManager.GetCurrentWeapon();  

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
        } else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1 / currentWeapon.fireRate);
            } else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
       
    }

    [Client]
    void Shoot()
    {
        Debug.Log("SHOOT!");
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider.tag ==    PLAYER_TAG) 
           {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
           }
        }
    }

    [Command]
    void CmdPlayerShot ( string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been hit!");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    } 
}
