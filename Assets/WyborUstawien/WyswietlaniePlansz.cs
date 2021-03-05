using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class WyswietlaniePlansz : MonoBehaviour
{
    [SerializeField]
    GameObject plansza;
    private void Start()
    {
        int[] wielkosci = ((Gracz)System.Activator.CreateInstance(Ustawienia.Przeciwnik)).ObslugiwaneRozmiaryPlansz;

        var poleDlaPlansz = GetComponent<PolygonCollider2D>().GetPath(1);
        Vector2 rozmiarPolaDlaPlansz = new Vector2(Vector2.Distance(poleDlaPlansz[0], poleDlaPlansz[1]), Vector2.Distance(poleDlaPlansz[2], poleDlaPlansz[1]));
        float wielkoscPlanszyNaEkranie = Mathf.Min(rozmiarPolaDlaPlansz.x,rozmiarPolaDlaPlansz.y)/Mathf.Ceil(Mathf.Sqrt(wielkosci.Length))-0.05f;
        Vector2 punktCentralny = poleDlaPlansz.Aggregate(Vector2.zero, (acc, v) => acc + v)/poleDlaPlansz.Length;

        float kat = 0f;
        foreach (var wielkosc in wielkosci)
        {
            kat += 2f*Mathf.PI/wielkosci.Length;

            GameObject nowa = Instantiate(plansza);
            float spawnR = 1f;

            //nowa.transform.position = new Vector3(Random.Range(-spawnR,spawnR),Random.Range(-spawnR,spawnR),0f);
            nowa.transform.position = new Vector3(spawnR*Mathf.Sin(kat), spawnR * Mathf.Cos(kat), 0f);


            var script = nowa.GetComponent<PlanszaWUstawieniach>();
            script.Init(wielkosc,wielkoscPlanszyNaEkranie,Klik);

            nowa.GetComponent<PrzemieszczanieObiektow>().punktCentralny=punktCentralny;
        }
    }
    void Klik(int wielkoscPlanszy)
    {
        Ustawienia.WielkoscPlanszy = wielkoscPlanszy;
        SceneManager.LoadScene("Gra");
    }
}
