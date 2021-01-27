using GraZaDuzoZaMalo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AppGraZaDuzoZaMaloCLI
{
    class WidokCLI
    {
        private KontrolerCLI kontroler;

        public WidokCLI(KontrolerCLI kontroler) => this.kontroler = kontroler;

        public void CzyscEkran() => Clear();

        public void KomunikatPowitalny() => WriteLine("Wylosowałem liczbę z zakresu ");

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="KoniecGryException"></exception>
        /// <returns>Selected number.</returns>
        public int WczytajPropozycje()
        {
            int wynik = 0;
            bool sukces = false;
            while (!sukces)
            {
                Write($"Podaj swoją propozycję ({KontrolerCLI.ZNAK_ZAKONCZENIA_GRY} aby przerwać, {KontrolerCLI.GAME_PAUSE_SIGN} aby zawiesić): ");
                try
                {
                    string value = ReadLine().TrimStart().ToUpper();
                    if (value.Length > 0 && value[0].Equals(KontrolerCLI.ZNAK_ZAKONCZENIA_GRY))
                        throw new KoniecGryException();
                    else if (value.Length > 0 && value[0].Equals(KontrolerCLI.GAME_PAUSE_SIGN))
                        throw new PauseGameException();

                    //UWAGA: ponizej może zostać zgłoszony wyjątek 
                    wynik = Int32.Parse(value);
                    sukces = true;
                }
                catch (FormatException)
                {
                    WriteLine("Podana przez Ciebie wartość nie przypomina liczby! Spróbuj raz jeszcze.");
                    continue;
                }
                catch (OverflowException)
                {
                    WriteLine("Przesadziłeś. Podana przez Ciebie wartość jest zła! Spróbuj raz jeszcze.");
                    continue;
                }
                catch (KoniecGryException)
                {
                    throw new KoniecGryException();
                }
                catch (PauseGameException)
                {
                    throw new PauseGameException();
                }
                catch (Exception)
                {
                    WriteLine("Nieznany błąd! Spróbuj raz jeszcze.");
                    continue;
                }
            }
            return wynik;
        }

        public void OpisGry()
        {
            WriteLine("Gra w \"Za dużo za mało\"." + Environment.NewLine
                + "Twoim zadaniem jest odgadnąć liczbę, którą wylosował komputer." + Environment.NewLine + "Na twoje propozycje komputer odpowiada: za dużo, za mało albo trafiłeś");
        }

        public bool ChceszKontynuowac( string prompt )
        {
                Write( prompt );
                char odp = ReadKey().KeyChar;
                WriteLine();
                return (odp == 't' || odp == 'T');
        }

        public void HistoriaGry()
        {
            if (kontroler.ListaRuchow.Count == 0)
            {
                WriteLine("--- pusto ---");
                return;
            }

            WriteLine("=================================================");
            int i = 1;
            foreach ( var ruch in kontroler.ListaRuchow)
            {
                var time = $"{ruch.Czas.Hour:D2}:{ruch.Czas.Minute:D2}:{ruch.Czas.Second:D2}";
                
                if(ruch.StatusGry == Gra.Status.Zakonczona)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    WriteLine($"Nr: {i}, propozycja: {ruch.Liczba:D2}, odpowiedź: {ruch.Wynik}, czas: {time}.");
                    WriteLine($"Całkowity czas gry: {kontroler.CalkowityCzasGry}");
                    WriteLine($"Status: {ruch.StatusGry}.");
                    Console.ResetColor();
                } 
                else if(ruch.StatusGry == Gra.Status.Zawieszona)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLine($"Nr: {i}, czas: {time}, status: {ruch.StatusGry}.");
                    Console.ResetColor();
                }
                else
                {
                    WriteLine($"Nr: {i}, propozycja: {ruch.Liczba:D2}, odpowiedź: {ruch.Wynik}, czas: {time}, status: {ruch.StatusGry}.");
                }

                i++;
            }
        }

        public void KomunikatZaDuzo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine("Za dużo!");
            Console.ResetColor();
        }

        public void KomunikatZaMalo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine("Za mało!");
            Console.ResetColor();
        }

        public void KomunikatTrafiono()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine("Trafiono!");
            Console.ResetColor();
        }

        public void ShowGameSummary(Gra game)
        {
            WriteLine("=================================================");
            WriteLine("============== Poprzedni stan gry ===============");
            WriteLine($"Czas rozpoczecia: {game.CzasRozpoczecia}");
            WriteLine($"Minimalna liczba: {game.MinLiczbaDoOdgadniecia}");
            WriteLine($"Maksymalna liczba: {game.MaxLiczbaDoOdgadniecia}");
            WriteLine();
        }

        public void PauseGame()
        {
            Console.WriteLine("========================");
            Console.WriteLine("= Game has been paused =");
        }
    }

}
