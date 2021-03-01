using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Powrot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    string nazwaSceny = "Menu";
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(nazwaSceny);
    }
}
