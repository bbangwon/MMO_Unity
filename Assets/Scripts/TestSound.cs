using UnityEngine;

public class TestSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public AudioClip audioClip;
    //public AudioClip audioClip2;

    int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        //var audio = GetComponent<AudioSource>();
        //audio.PlayOneShot(audioClip);
        //audio.PlayOneShot(audioClip2);

        //float lifeTime = Mathf.Max(audioClip.length, audioClip2.length);
        //GameObject.Destroy(gameObject, lifeTime);

        i++;

        if(i % 2 == 0)
            Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Bgm);
        else
            Managers.Sound.Play("UnityChan/univ0002", Define.Sound.Bgm);
    }
}
