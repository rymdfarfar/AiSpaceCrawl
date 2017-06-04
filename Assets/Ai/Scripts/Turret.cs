using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Turret : MonoBehaviour {
    public AudioSource source;
    public AudioClip[] clips;
    public GameObject[] guns;
    
    public ObjectPoolerScript pool;
    public float bulletSpeed = 1500f;
    public float bulletLife = 10f;
    public ParticleSystem muzzleFlash;
    public float range;
    public LineRenderer[] lasers;
    Vector3[] points = new Vector3[2];
    public int index = 0;

    // Use this for initialization
    void Start () {
        lasers[0].enabled = false;
        lasers[0].enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {

      




         RaycastHit hit;
           
            if (Physics.Raycast(guns[index].transform.position, guns[index].transform.forward, out hit, range))
            {
                 
                lasers[index].enabled = true;
                points[0] = guns[index].transform.position;
                points[1] = hit.point;
                lasers[index].SetPositions(points);
               
                
                muzzleFlash.transform.position = guns[index].transform.position;
                source.PlayOneShot(clips[0]);
                muzzleFlash.Emit(1);
               

                if (hit.transform.CompareTag("Player"))
                {
                    //Player Loses Healths
                    Debug.Log("-Hp");

                }
                StartCoroutine(DisableLaser(index, hit.point, bulletLife));

            ++index;
            if (index >= guns.Length)
                index = 0;
                
            }
            
           
        
    }

     IEnumerator DisableLaser(int num, Vector3 pos, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject explosion;
        explosion = pool.GetPooledObject();
        ParticleSystem pa = explosion.GetComponent<ParticleSystem>();
        explosion.SetActive(true);
        
        pa.transform.position = pos;
        pa.Emit(1);
        lasers[num].enabled = false;
        StartCoroutine(DisableExplosion(explosion, 0.2f));

    }

    IEnumerator DisableExplosion(GameObject explosion, float time)
    {
        yield return new WaitForSeconds(time);
        explosion.SetActive(false);
    }


}
