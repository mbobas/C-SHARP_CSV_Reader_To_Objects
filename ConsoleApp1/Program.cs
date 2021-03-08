using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Program p = new Program();
            p.menu();

        }
        //Struktura pliku w firmie1:
        //Kod_pracownika;
        //data;
        //godzina_wejścia;
        //godzina_wyjścia;
        //czas_pracy

        //Struktura pliku w firmie2:
        //Kod_pracownika;
        //data;godzina;
        //WE/WY(WEjście/WYjście)
        public void wypisz(List<DzienPracy> dzien)
        {
            foreach (DzienPracy d in dzien)
            {
                Console.WriteLine(d.KodPracownika + " " + d.Data + " " + d.GodzinaWejscia
                           + " " + d.GodzinaWyjscia + "\n");

            }
        }
        public List<DzienPracy> ReadFileCompany1()
        {
            List<DzienPracy> result = new List<DzienPracy>();
            StreamReader dayReader = new StreamReader("rcp1.csv");

            if (dayReader != null)
            {
                int line_number = 0;
                while (!dayReader.EndOfStream)
                {
                    var line = dayReader.ReadLine();
                    if (line_number >= 0)
                    {
                        var values = line.Split(';');
                        DzienPracy day = new DzienPracy();
                        day.KodPracownika = Convert.ToString(values[0]);
                        day.Data = Convert.ToDateTime(values[1]);
                        day.GodzinaWejscia = TimeSpan.Parse(values[2]);
                        day.GodzinaWyjscia = TimeSpan.Parse(values[3]);
                        result.Add(day);
                    }
                    line_number++;
                }
                dayReader.Close();
            }
            return result;
        }

        public List<DzienPracy> ReadFileCompany2()
        {
            List<DzienPracy> result = new List<DzienPracy>();
            StreamReader dayReader = new StreamReader("rcp2.csv");
            string lastKodPracownika = "";
            bool czytajStaraLinie = false;
            var line = "";

            if (dayReader != null)
            {
                int line_number = 0;
                while (!dayReader.EndOfStream)
                {
                    if (czytajStaraLinie)
                    {
                        czytajStaraLinie = false;
                    }else
                    {
                         line = dayReader.ReadLine();
                    }
                    
                    if (line_number >= 0)
                    {
                        var values = line.Split(';');
                        if (values[0] != lastKodPracownika)
                        {
                            DzienPracy day = new DzienPracy();
                            day.KodPracownika = Convert.ToString(values[0]);
                            day.Data = Convert.ToDateTime(values[1]);
                            try
                            {
                                if (values[3] == "WE")
                                {
                                    day.GodzinaWejscia = TimeSpan.Parse(values[2] + ":00");
                                    var line2 = dayReader.ReadLine();
                                    var values2 = line2.Split(';');
                                    if (values2[0]== values[0])
                                    {
                                        day.GodzinaWyjscia = TimeSpan.Parse(values2[2] + ":00");
                                    }else
                                    {
                                        line = line2;
                                        czytajStaraLinie = true;
                                    }
                                }
                                else if (values[3] == "WY")
                                {
                                    day.GodzinaWyjscia = TimeSpan.Parse(values[2] + ":00");
                                    var line2 = dayReader.ReadLine();
                                    var values2 = line2.Split(';');
                                    if (values2[0] == values[0])
                                    {
                                        day.GodzinaWejscia = TimeSpan.Parse(values2[2] + ":00");
                                    }
                                    else
                                    {
                                        line = line2;
                                        czytajStaraLinie = true;
                                    }


                                }
                            }
                            catch (System.FormatException ex)
                            {
                                continue;
                            }

                            result.Add(day);
                        }


                    }
                    line_number++;
                }
                dayReader.Close();
            }
            return result;
        }

        public void menu()
        {
            Program program = new Program();
            List<DzienPracy> dzien1 = program.ReadFileCompany1();
            List<DzienPracy> dzien2 = program.ReadFileCompany2();
            Console.WriteLine("***********MENU***********\n");
            Console.WriteLine("***Nacisnij odpowiedni klawisz***\n");
            Console.WriteLine("Program Wczytuje 2 pliki o nazwach  rcp1.csv i rcp2.csv \n Dzieje sie to automatycznie z folderu ConsoleApp1\\bin\\Debug\\ \n");
            Console.WriteLine("Dla pliku rcp1 program nie uwzględnia wyjątków, gdyż plik był kompletny");
            Console.WriteLine("Dla pliku rcp2 program pomija wiersz jeśli na dany dzien przypadło jedno wejście lub wyjscie");

            Console.WriteLine("Wciśnij (1), aby wyświetlic wczytaną listę dla pliku rcp1.csv\n");
            Console.WriteLine("Wciśnij (2), aby wyświetlic wczytaną listę dla pliku rcp2.csv\n");
            Console.WriteLine("Wciśnij (Q,q), aby wyjść\n");

            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                program.wypisz(dzien1);
                menu();
            }
            else if (userInput == "2")
            {
                program.wypisz(dzien2);
                menu();
            }
            else if (userInput == "q" || userInput == "Q")
            {
                System.Environment.Exit(1);
            }
            Console.ReadKey();

        }

    }
}
