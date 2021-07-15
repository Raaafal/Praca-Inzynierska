using UnityEngine;

public class Gracz
{
    public PreferencjeGracza preferencjeGracza;
    public virtual int[] ObslugiwaneRozmiaryPlansz { get; } = new int[] { 3, 4, 5, 6, 8, 10 };
    public Color KolorKrolowej { get; set; } = new Color(1, 1, 1, 1);
    public static (int x, int y) BrakRuchu { get; } = (-1, -1);
    public virtual (int x, int y) WykonajRuch(int[][] plansza)
    {
        return BrakRuchu;
    }
    public virtual (int x, int y) WykonajRuch(LogikaPlanszy plansza)
    {
        return WykonajRuch(plansza.Plansza);
    }
    public bool czyNasluchujeKlikniec = false;
    public virtual void NasluchujKlikniec(int x, int y)
    {
    }
    public virtual void Inicjalizuj()
    {
    }
    public virtual void Zakoncz(LogikaPlanszy plansza)
    {
    }
    public string nazwa = "Przeciwnik";
    public string grajZ = "Przeciwnikiem";
    protected bool bot = true;
}
