using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public List<GameObject> _enemy;
    public NavMeshAgent agent;
    public AudioSource _audio;
    public WeaponEnemy _weapon;
    public int currentEnemy;
    public int hp;
    public GameObject _ps;
    public bool isEnemy;

    public float oldTime;

    public void Start()
    {
        currentEnemy = Random.Range(0, _enemy.Count);
        oldTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            agent.SetDestination(_enemy[currentEnemy].transform.position);
            if (hp <= 0)
            {
                StartCoroutine(_dead());

            }
        }
        catch (Exception e)
        {
            _enemy.Remove(_enemy[currentEnemy]);
            currentEnemy = Random.Range(0, _enemy.Count);
        }

        if (isEnemy)
        {
            if (Time.time - oldTime > 0.5)
            {
                _fire();
                oldTime = Time.time;
            }
        }
    }

    IEnumerator deleteShot(GameObject go)
    {
        yield return new WaitForSeconds(0.9f);
        Destroy(go);
    }
    
    private void _fire()
    {
        RaycastHit hit;
        Vector3 fwd = _weapon.transform.TransformDirection(Vector3.forward);
        
        if (Physics.Raycast(transform.position, fwd, out hit, 10.0f))
        {
            GameObject ps = Instantiate(_ps, hit.point, Quaternion.identity);
            ps.GetComponent<ParticleSystem>().Play();
            _audio.Play();
            StartCoroutine(deleteShot(ps));
            int res = Random.Range(0, 10);
            if (res > 7)
            {
                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Weapon")
                {
                    if (hit.transform.gameObject.GetComponent<WeaponEnemy>().enemyBody.name != gameObject.name)
                    {
                        hit.transform.gameObject.GetComponent<WeaponEnemy>().enemyBody.GetComponent<EnemyController>()
                            .hit(25);
                    }
                }

                if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject.tag == "Tank")
                {
                    if (hit.transform.gameObject.name != gameObject.name)
                    {
                        hit.transform.gameObject.GetComponent<EnemyController>().hit(25);
                    }
                }

                if (hit.transform.gameObject.layer == 9 && hit.transform.gameObject.tag == "Weapon")
                {
                    hit.transform.gameObject.GetComponent<WeaponController>().player.GetComponent<PlayerController>()
                        .hit(25);

                }

                if (hit.transform.gameObject.layer == 9 && hit.transform.gameObject.tag == "Tank")
                {
                    hit.transform.gameObject.GetComponent<PlayerController>().hit(25);
                }
            }
        }
    }
    
    IEnumerator _dead()
    {
        _ps.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        try
        {
            Destroy(gameObject);
        }
        catch (UnityException e)
        {
            yield break;
        }
    }

    public void hit(int damage)
    {
        hp -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tank" && other.gameObject.name != gameObject.name)
        {
            try
            {
                for (int i = 0; i < _enemy.Count; i++)
                {
                    if (_enemy[i].gameObject.name == other.gameObject.name)
                    {
                        currentEnemy = i;
                        isEnemy = true;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                ;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tank")
        {
            isEnemy = false;
        }
    }
}
