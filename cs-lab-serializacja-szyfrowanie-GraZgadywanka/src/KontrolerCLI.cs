using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraZaDuzoZaMalo.Model;
using static GraZaDuzoZaMalo.Model.Gra.Odpowiedz;

namespace AppGraZaDuzoZaMaloCLI
{
    public class KontrolerCLI
    {
        public const char ZNAK_ZAKONCZENIA_GRY = 'X';
        public const char GAME_PAUSE_SIGN = 'P';

        private Gra gra;
        private WidokCLI widok;

        private bool CanOverrideSave { get; set; }
        private bool CanContinue { get; set; }

        public int MinZakres { get; private set; } = 1;
        public int MaxZakres { get; private set; } = 100;

        public TimeSpan CalkowityCzasGry => gra.CalkowityCzasGry;

        public IReadOnlyList<Gra.Ruch> ListaRuchow {
            get
            { return gra.ListaRuchow;  }
 }

        public KontrolerCLI()
        {
            gra = new Gra();
            widok = new WidokCLI(this);
            CanContinue = true;
            CanOverrideSave = false;
        }

        public async void Background()
        {
            var timeInMs = 5000;

            while (true)
            {
                var delayTask = Task.Delay(timeInMs);
                if(CanOverrideSave && (gra.StatusGry == Gra.Status.WTrakcie || gra.StatusGry == Gra.Status.Zawieszona))
                {
                    Gra.SaveGame(gra, false);
                }

                await delayTask;
            }
        }

        public void Uruchom()
        {
            ThreadStart childref = new ThreadStart(Background);
            Thread childThread = new Thread(childref);
            childThread.Start();

            widok.OpisGry();
            while( CanContinue && widok.ChceszKontynuowac("Czy chcesz kontynuować aplikację (t/n)? ") )
            {
                UruchomRozgrywke();
            }
        }

        public void UruchomRozgrywke()
        {
            widok.CzyscEkran();

            try {
                gra = new Gra(MinZakres, MaxZakres);
            } catch {
                gra = new Gra(1, 100);
            }

            if (Gra.SaveExists())
            {
                try { 
                    var gameSave = Gra.GetGameSave();
                    widok.ShowGameSummary(gameSave);

                    if (widok.ChceszKontynuowac("Znaleziono poprzedni zapis gry, czy chcesz kontynuować (t/n)?"))
                    {
                        gra.LoadSave(gameSave);
                    }

                    Gra.DeleteSave();
                }
                catch 
                {
                    Gra.DeleteSave();
                    Console.WriteLine("Nie udało się wczytać zapisu gry.");
                }
            }

            CanOverrideSave = true;

            do
            {
                while(gra.StatusGry == Gra.Status.Zawieszona)
                {
                    widok.CzyscEkran();
                    if (widok.ChceszKontynuowac("Game is paused, do you want to play (t/n)?"))
                    {
                        gra.UnPauseGame();
                    }
                }

                //wczytaj propozycję
                int propozycja = 0;
                try
                {
                    propozycja = widok.WczytajPropozycje();
                }
                catch (PauseGameException)
                {
                    gra.PauseGame();
                    widok.PauseGame();
                }
                catch ( KoniecGryException)
                {
                    gra.Przerwij();
                    Gra.SaveGame(gra);
                    CanContinue = false;
                }

                if (gra.StatusGry == Gra.Status.Poddana) {
                    break;
                }
                else if (gra.StatusGry == Gra.Status.Zakonczona) {
                    continue;
                }

                Console.WriteLine(propozycja);

                var result = gra.Ocena(propozycja);
                widok.CzyscEkran();
                widok.HistoriaGry();

                switch (result)
                {
                    case ZaDuzo:
                        widok.KomunikatZaDuzo();
                        break;
                    case ZaMalo:
                        widok.KomunikatZaMalo();
                        break;
                    case Trafiony:
                        widok.KomunikatTrafiono();
                        break;
                    default:
                        break;
                }
                
            }
            while (gra.StatusGry == Gra.Status.WTrakcie || gra.StatusGry == Gra.Status.Zawieszona);
                      
            //if StatusGry == Przerwana wypisz poprawną odpowiedź
            //if StatusGry == Zakończona wypisz statystyki gry
        }

        ///////////////////////

        public void UstawZakresDoLosowania(ref int min, ref int max)
        {

        }

        public int LiczbaProb() => gra.ListaRuchow.Count();

        public void ZakonczGre()
        {
            //np. zapisuje stan gry na dysku w celu późniejszego załadowania
            //albo dopisuje wynik do Top Score
            //sprząta pamięć
            gra = null;
            widok.CzyscEkran(); //komunikat o końcu gry
            widok = null;
            System.Environment.Exit(0);
        }

        public void ZakonczRozgrywke()
        {
            gra.Przerwij();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <exception cref="KoniecGryException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <returns></returns>
        public int WczytajLiczbeLubKoniec(string value, int defaultValue )
        {
            if( string.IsNullOrEmpty(value) )
                return defaultValue;

            value = value.TrimStart().ToUpper();
            if ( value.Length>0 && value[0].Equals(ZNAK_ZAKONCZENIA_GRY))
                throw new KoniecGryException();

            //UWAGA: ponizej może zostać zgłoszony wyjątek 
            return Int32.Parse(value);
        }
    }

    [Serializable]
    internal class KoniecGryException : Exception
    {
        public KoniecGryException()
        {
        }

        public KoniecGryException(string message) : base(message)
        {
        }

        public KoniecGryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KoniecGryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    internal class PauseGameException: Exception
    {
        public PauseGameException() {}

        public PauseGameException(string message) : base(message) {}

        public PauseGameException(string message, Exception innerException) : base(message, innerException) {}
    }
}
