using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private float _boostSpeed;
    public GameObject camera;
    public Camera _camera;
    private Rigidbody rb;
    public bool isBoost;
    private float moveZ;
    public float fuelBoost;
    public float timeBost;
    public AudioSource _audioShoot;

    public GameObject textLife;
    public GameObject textAmmo;
    
    public GameObject _ps1;
    public GameObject _ps2;
    public int hp;

    public int heavyAmmo;

    public GameObject cursor;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        _boostSpeed = speed * 2;
        fuelBoost = 100.0f;
        heavyAmmo = 8;
    }

    public void Update()
    {
        _rotate();
        _fire();
        _boost();
        if (hp <= 0)
        {
            dead();
        }
        textLife.GetComponent<TextMeshProUGUI>().text = "HP:" + hp;
        textAmmo.GetComponent<TextMeshProUGUI>().text = "AMMO:" + heavyAmmo;
    }

    void FixedUpdate()
    {        
        _move();
    }

    private void dead()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    
    public void hit(int damage)
    {
        hp -= damage;
    }
    
    private void _rotate()
    {
        float rotationY = Input.GetAxis("Horizontal") * 10.0f;
        transform.Rotate (0, rotationY, 0);
        rotationY = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void _fire()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + 40, 0));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                GameObject ps = Instantiate(_ps1, hit.point, Quaternion.identity);
                ps.GetComponent<ParticleSystem>().Play();
                _audioShoot.Play();
                StartCoroutine(deleteShot(ps));
                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Weapon")
                {
                    hit.transform.gameObject.GetComponent<WeaponEnemy>().enemyBody.GetComponent<EnemyController>()
                        .hit(10);
                    cursor.GetComponent<Image>().color = Color.red;
                    StartCoroutine(changeColor());
                }
                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Tank")
                {
                    hit.transform.gameObject.GetComponent<EnemyController>().hit(10);
                    cursor.GetComponent<Image>().color = Color.red;
                    StartCoroutine(changeColor());
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && heavyAmmo > 0)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + 40, 0));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                GameObject ps = Instantiate(_ps2, hit.point, Quaternion.identity);
                ps.GetComponent<ParticleSystem>().Play();
                StartCoroutine(deleteShot(ps));
                heavyAmmo--;
                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Weapon")
                {
                    hit.transform.gameObject.GetComponent<WeaponEnemy>().enemyBody.GetComponent<EnemyController>()
                        .hit(100);
                }
                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Tank")
                {
                    hit.transform.gameObject.GetComponent<EnemyController>().hit(100);
                }
            }
        }
    }

    IEnumerator changeColor()
    {
        yield return new WaitForSeconds(0.01f);
        cursor.GetComponent<Image>().color = Color.white;
    }

    IEnumerator deleteShot(GameObject go)
    {
        yield return new WaitForSeconds(0.9f);
        Destroy(go);
    }

    private void _move()
    {
        if (isBoost)
        {
            moveZ = Input.GetAxis("Vertical") * _boostSpeed * Time.fixedDeltaTime;
        }
        else
        {
            moveZ = Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;
        }
        transform.Translate(0, 0, moveZ);   
    }

    private void _boost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && fuelBoost == 100.0f)
        {
            isBoost = true;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            isBoost = false;
        }

        if (isBoost && Time.time - timeBost > 0.05)
        {
            if (fuelBoost > 0)
                fuelBoost -= 1;
            timeBost = Time.time;
        }
        
        if (!isBoost && Time.time - timeBost > 0.05 && fuelBoost != 100.0f)
        {
            if (fuelBoost < 100)
                fuelBoost += 1;
            timeBost = Time.time;
        }
    }
}
