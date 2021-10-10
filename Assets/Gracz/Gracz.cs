using UnityEngine;

public class Gracz
{
    public class OgraniczeniaGracza
    {
        public float MaksCzasNaRuch = 0.5f;
        public float MinCzasNaRuch = 0.5f;
        public uint MinIteracji = 0;
        public uint MaksIteracji = 0;//0 oznacza brak
    }
    public OgraniczeniaGracza Ograniczenia=new OgraniczeniaGracza();
    public PreferencjeGracza preferencjeGracza;
    public virtual int[] ObslugiwaneRozmiaryPlansz { get; } = new int[] { 3, 4, 5, 6, 8, 10 };
    public Color KolorKrolowej { get; set; } = new Color(1, 1, 1, 1);
    public static (int x, int y) BrakRuchu { get; } = (-1, -1);
    public virtual (int x, int y) PlanujRuch(LogikaPlanszy plansza)
    {
        return BrakRuchu;
    }
    private float pozostalyCzas = 0.5f;
    private uint iteracja = 0;
    public float PozostalyCzas=>pozostalyCzas;
    public float Iteracja => iteracja;
    (int x, int y) planowanyRuch = BrakRuchu;
    public virtual (int x, int y) WykonajRuch(LogikaPlanszy plansza)
    {
        if (iteracja < Ograniczenia.MaksIteracji || Ograniczenia.MaksIteracji == 0)
        {
            pozostalyCzas -= Time.deltaTime;
            (int x, int y) ruch = PlanujRuch(plansza);
            if (ruch != BrakRuchu)
            {
                planowanyRuch = ruch;
            }
            ++iteracja;
            if (pozostalyCzas < 0)
            {
                pozostalyCzas += Ograniczenia.MaksCzasNaRuch;
                if (planowanyRuch == BrakRuchu)
                {
                    for(int x = 0; x < plansza.Wielkosc; ++x)
                    {
                        for (int y = 0; y < plansza.Wielkosc; ++y)
                        {
                            if (LogikaPlanszy.CzyWolne(plansza.Plansza[x][y]))
                            {
                                return (x, y);
                            }
                        }
                    }
                }
                ruch = planowanyRuch;
                planowanyRuch = BrakRuchu;
                return ruch;
            }
        }
        else
        {
            iteracja = 0;
            pozostalyCzas = Ograniczenia.MaksCzasNaRuch;
            return planowanyRuch;
        }
        return BrakRuchu;

    }
    public bool CzyOstatniaIteracja()
    {
        return pozostalyCzas <= 0 || (Ograniczenia.MaksIteracji != 0 && iteracja >= Ograniczenia.MaksIteracji);
    }
    public void zatwierdzRuch()
    {
        pozostalyCzas = 0;
    }
    public bool czyNasluchujeKlikniec = false;
    public virtual void NasluchujKlikniec(int x, int y)
    {
    }
    public virtual void Inicjalizuj()
    {
        pozostalyCzas = Ograniczenia.MaksCzasNaRuch;
    }
    public virtual void Zakoncz(LogikaPlanszy plansza)
    {
    }
    public string nazwa = "Przeciwnik";
    public string grajZ = "Przeciwnikiem";
    protected bool bot = true;
}
