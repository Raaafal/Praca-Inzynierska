using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Arena : LogikaAreny
{
    const string tekstPrzyciskuDolacz = "Uruchom obecne",
        tekstPrzyciskuOlacz = "Zatrzymaj obecne";
    public Dropdown Pierwszy, Drugi;
    public Text PierwszyWygrane, PierwszyWygraneProcentowo, PierwszyIteracje;
    public Text DrugiWygrane, DrugiWygraneProcentowo, DrugiIteracje;
    public Text Ruchy;
    public Button Obecni;

    (int pierwszy, int drugi) pokaz = (0, 0);

    // Start is called before the first frame update
    void Start()
    {
        var opcje = new List<Dropdown.OptionData>();
        foreach (var opcja in uczestnicy)
        {
            opcje.Add(new Dropdown.OptionData(opcja.Name));
        }
        Pierwszy.options = opcje;
        Drugi.options = opcje;
    }

    // Update is called once per frame
    void Update()
    {
        if (KrokSymulacjiRozgrywek())//jeśli jedna z rozgrywek się zakończyła, trzeba zaktualizować wyświetlane dane dane
        {
            PokazStatystyki();
        }

    }
    public void ZmienPierwszego()
    {
        pokaz.pierwszy = Pierwszy.value;
        PokazStatystyki();
    }
    public void ZmienDrugiego()
    {
        pokaz.drugi = Drugi.value;
        PokazStatystyki();
    }
    public void DoGry()
    {
        if (Obecni.GetComponentInChildren<Text>().text == tekstPrzyciskuDolacz)
        {
            DoGry(Pierwszy.value, Drugi.value);
            PokazStatystyki();
        }

    }
    public void ZGry()
    {
        if (Obecni.GetComponentInChildren<Text>().text == tekstPrzyciskuOlacz)
        {
            ZGry(Pierwszy.value, Drugi.value);
            PokazStatystyki();
        }
    }
    public void WszyscyDoGry()
    {
        base.WszyscyDoGry();
        PokazStatystyki();
    }
    public void PokazStatystyki()
    {
        var statystyka = statystyki[pokaz.pierwszy, pokaz.drugi];
        PierwszyWygrane.text = statystyka.Wygrane.ToString();
        PierwszyWygraneProcentowo.text = statystyka.WygraneProcent.ToString();
        PierwszyIteracje.text = statystyka.SrodnioIteracjiPierwszego.ToString();
        DrugiWygrane.text = statystyka.Przegrane.ToString();
        DrugiWygraneProcentowo.text = statystyka.PrzegraneProcent.ToString();
        DrugiIteracje.text = statystyka.SrodnioIteracjiDrugiego.ToString();
        Ruchy.text = statystyka.SrednioRuchow.ToString();

    }
    public void AktualizujPrzyciskObecnych()
    {
        if(rozgrywki.IndexOf((Pierwszy.value, Drugi.value))>=0)
        {
            Obecni.GetComponentInChildren<Text>().text = tekstPrzyciskuOlacz;
        }
        else
        {
            Obecni.GetComponentInChildren<Text>().text = tekstPrzyciskuDolacz;
        }
    }
}