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
            Console.WriteLine("Hello World");

            Program program = new Program();
            program.ReadFileCompany2();

            Console.ReadKey();

           
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
                    if (line_number >=0 ) //>=
                    {
                        var values = line.Split(';');
                        DzienPracy day = new DzienPracy();
                        day.KodPracownika = Convert.ToString(values[0]);
                        day.Data = Convert.ToDateTime(values[1]);
                        day.GodzinaWejscia = TimeSpan.Parse(values[2]);
                        day.GodzinaWyjscia = TimeSpan.Parse(values[3]);

                        Console.WriteLine(day.KodPracownika + " " + day.Data + " " + day.GodzinaWejscia
                            + " " + day.GodzinaWyjscia + "\n");
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

            if (dayReader != null)
            {
                int line_number = 0;
                while (!dayReader.EndOfStream)
                {
                    var line = dayReader.ReadLine();
                    if (line_number >= 0) 
                    {
                        var values = line.Split(';');
                        if (values[0] != lastKodPracownika)
                        {
                            DzienPracy day = new DzienPracy();
                            day.KodPracownika = Convert.ToString(values[0]);
                            day.Data = Convert.ToDateTime(values[1]);
                            if (values[3] == "WE")
                            {
                                day.GodzinaWejscia = TimeSpan.Parse(values[2]+":00");
                                var line2 = dayReader.ReadLine();
                                var values2 = line2.Split(';');
                                day.GodzinaWyjscia = TimeSpan.Parse(values2[2] + ":00");

                            }
                            else if (values[3] == "WY")
                            {
                                day.GodzinaWyjscia = TimeSpan.Parse(values[2]+":00");
                                var line2 = dayReader.ReadLine();
                                var values2 = line2.Split(';');
                                day.GodzinaWejscia = TimeSpan.Parse(values2[2] + ":00");

                            }

                            Console.WriteLine(day.KodPracownika + " " + day.Data + " " + day.GodzinaWejscia
                            + " " + day.GodzinaWyjscia + "\n");
                            result.Add(day);
                        } 

                        
                    }
                    line_number++;
                }
                dayReader.Close();
            }
            return result;
        }

    }
    
}
