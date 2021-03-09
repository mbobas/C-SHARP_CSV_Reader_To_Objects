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
        
        public void show(List<DzienPracy> dzien)
        {
            foreach (DzienPracy d in dzien)
            {
                Console.WriteLine(d.KodPracownika + " | " + d.Data + " | " + d.GodzinaWejscia
                           + " | " + d.GodzinaWyjscia + "\n");
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
                        try
                        {
                            var values = line.Split(';');
                            
                            if (!values[0].Equals("") && values[1] != null && values[2] != null && values[3] != null)
                            {
                                DzienPracy day = new DzienPracy();
                                day.KodPracownika = Convert.ToString(values[0]);
                                day.Data = Convert.ToDateTime(values[1]);
                                day.GodzinaWejscia = TimeSpan.Parse(values[2]);
                                day.GodzinaWyjscia = TimeSpan.Parse(values[3]);
                                result.Add(day);
                            } else
                            {
                                continue;
                            }
                            
                        }
                        catch (System.FormatException ex)
                        {
                            continue;
                        }
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

            bool readOldLine = false;
            var line = "";

            if (dayReader != null)
            {
                int line_number = 0;
                while (!dayReader.EndOfStream)
                {
                    if (readOldLine)
                    {
                        readOldLine = false;
                    }
                    else
                    {
                        line = dayReader.ReadLine();
                    }

                    if (line_number >= 0)
                    {
                        var values = line.Split(';');

                        if (!values[0].Equals("") && !values[1].Equals("") && !values[2].Equals("") && !values[3].Equals(""))
                        {
                            DzienPracy day = new DzienPracy();
                            day.KodPracownika = Convert.ToString(values[0]);
                            day.Data = Convert.ToDateTime(values[1]);
                            try
                            {
                                if (values[3] == "WE" )
                                {
                                    day.GodzinaWejscia = TimeSpan.Parse(values[2] + ":00");
                                    var line2 = dayReader.ReadLine();
                                    var values2 = line2.Split(';');
                                    if (values2[0] == values[0] && values2[1] == values[1] && !values2[2].Equals(""))
                                    {
                                        day.GodzinaWyjscia = TimeSpan.Parse(values2[2] + ":00");
                                    }
                                    else
                                    {
                                        line = line2;
                                        readOldLine = true;
                                        continue;
                                    }
                                }
                                else if (values[3] == "WY")
                                {
                                    day.GodzinaWyjscia = TimeSpan.Parse(values[2] + ":00");
                                    var line2 = dayReader.ReadLine();
                                    var values2 = line2.Split(';');
                                    if (values2[0] == values[0] && values2[1] == values[1] && !values2[2].Equals(""))
                                    {
                                        day.GodzinaWejscia = TimeSpan.Parse(values2[2] + ":00");
                                    }
                                    else
                                    {
                                        line = line2;
                                        readOldLine = true;
                                        continue;
                                    }
                                }
                                else if (values[2] == null)
                                {
                                    continue;
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
            List<DzienPracy> dayList1 = program.ReadFileCompany1();
            List<DzienPracy> dayList2 = program.ReadFileCompany2();
            program.SavaResults(dayList1, "dayList1");
            program.SavaResults(dayList2, "dayList2");


            Console.WriteLine("***********MENU***********\n");
            Console.WriteLine("***Nacisnij odpowiedni klawisz***\n");
            Console.WriteLine("Program Wczytuje 2 pliki o nazwach  rcp1.csv i rcp2.csv\nDzieje sie to automatycznie z folderu ConsoleApp1\\bin\\Debug\\ \nczyli tego, w ktorym znajduje sie aplikacja. \n\n");
            Console.WriteLine("Dla pliku rcp1 program uwzględnia wyjątki braku danych w różnych komórkach, mimo, że przykładowy plik był kompletny.\n");
            Console.WriteLine("Dla pliku rcp2 program pomija wiersz jeśli na dany dzien przypadło jedno wejście lub wyjscie, robi to również w przypadku braku innych danych wejściowych.\n ");

            Console.WriteLine("Wciśnij (1), aby wyświetlic wczytaną listę dla pliku rcp1.csv\n");
            Console.WriteLine("Wciśnij (2), aby wyświetlic wczytaną listę dla pliku rcp2.csv\n");
            Console.WriteLine("Dodatkowo Kopie wyświetlanych list są zapisywane w folderze głównym jako copy_of_dayList1.csv & copy_of_dayList2.csv \n");

            Console.WriteLine("Wciśnij (Q,q), aby wyjść\n");

            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.Clear();
                program.show(dayList1);
                menu();
            }
            else if (userInput == "2")
            {
                Console.Clear();
                program.show(dayList2);
                menu();
            }
            else if (userInput == "q" || userInput == "Q")
            {
                System.Environment.Exit(1);
            }
            Console.ReadKey();

        }

        public void SavaResults(List<DzienPracy> dzienToSave, string fileName) 
        {
           
            StreamWriter writer = new StreamWriter($"copy_of_{fileName}.csv", false);
            if (writer !=null)
            {
                writer.WriteLine(@"KodPracownika;Data_DD.MM.YY.;Data_HH:MM:SS;GodzinaWejscia;GodzinaWyjscia"); // name of columns
                foreach(DzienPracy d in dzienToSave)
                {
                    writer.WriteLine(String.Format(@"{0};{1};{2};{3};", d.KodPracownika,d.Data,d.GodzinaWejscia,d.GodzinaWyjscia));
                }
                writer.Close();
            }
        }

    }
}
