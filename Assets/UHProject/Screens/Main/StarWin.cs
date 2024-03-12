using System.Collections;
using UnityEngine;

public class StarWin : MonoBehaviour
{
    [SerializeField] private GameObject _starL;
    [SerializeField] private GameObject _starR;
    [SerializeField] private GameObject _starC;
    [SerializeField] private ParticleSystem _psL;
    [SerializeField] private ParticleSystem _psR;
    [SerializeField] private ParticleSystem _psC;
    
    public void Begin()
    {
        StartCoroutine(PlayAnimation());
    }
    
    private IEnumerator PlayAnimation()
    {
        _psL.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _starL.SetActive(true);
        yield return new WaitForSeconds(0.27f);
        _psR.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _starR.SetActive(true);
        yield return new WaitForSeconds(0.27f);
        _psC.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _starC.SetActive(true);
        _psR.gameObject.SetActive(false);
        _psL.gameObject.SetActive(false);
    }
}
