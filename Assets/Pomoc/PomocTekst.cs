using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PomocTekst : MonoBehaviour
{
    [SerializeField]
    TextAsset textFile;
    [SerializeField]
    Text tekst;

    void Start()
    {
        tekst.text = textFile.text;
    }
}
