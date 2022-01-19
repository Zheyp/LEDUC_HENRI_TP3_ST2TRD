using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using TP3;

namespace LEDUC_HENRI_TP3_ST2TRD
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("########### C# TP3 HENRI LEDUC ###########");
            //-------------------------LINQ--------------------//
            var listMovie = new MovieCollection().Movies;

            Console.Write("1 - Display the title of the oldest movie : ");
            var q1 = listMovie.OrderBy(x => x.ReleaseDate).First().Title;
            Console.WriteLine(q1);

            Console.Write("2 - Count all movies : ");
            var q2 = listMovie.Count();
            Console.WriteLine(q2);

            Console.Write("3 - Count all movies with the letter e. at least once in the title : ");
            var q3 = listMovie.Count(movie => movie.Title.Contains("e"));
            Console.WriteLine(q3);

            Console.Write("4 - Count how many time the letter f is in all the titles from this list : ");
            var q4 = 
                string.Join(" ,", listMovie
                        .Select(x => x.Title)
                        .Where(x => x.Contains('f')))
                    .Count(x => x == 'f');
            Console.WriteLine(q4);

            Console.Write("5 - Display the title of the film with the higher budget : ");
            var q5 = listMovie.OrderBy(x => x.Budget).Last().Title;
            Console.WriteLine(q5);

            Console.Write("6 - Display the title of the movie with the lowest box office : ");
            var q6 = listMovie.OrderBy(x => x.BoxOffice).First().Title;
            Console.WriteLine(q6);

            Console.Write("7 - Order the movies by reversed alphabetical order and print the first 11 of the list : ");
            var q7 = listMovie.OrderBy(x => x.Title.ToLower()).Take(11);
            foreach (MovieCollection.WaltDisneyMovies e in q7)
            {
                Console.WriteLine(e.Title);
            }

            Console.Write("8 - Count all the movies made before 1980 : ");
            var q8 = listMovie.Count(x => x.ReleaseDate < new DateTime(1980, 1, 1));
            Console.WriteLine(q8);

            Console.Write("9 - Display the average running time of movies having a vowel as the first letter : ");
            var q9 = listMovie.Where(x =>
                x.Title.StartsWith("A") || x.Title.StartsWith("E") || x.Title.StartsWith("I") ||
                x.Title.StartsWith("O") || x.Title.StartsWith("U") ||
                x.Title.StartsWith("Y")).Average(x => x.RunningTime);
            Console.WriteLine(q9);

            Console.Write("10 - Print all movies with the letter H or W in the title, but not the letter I or T : ");
            var q10 = listMovie.Where(x =>
                (x.Title.ToLower().Contains("h") || x.Title.ToLower().Contains("w")) 
                & (x.Title.ToLower().Contains("i") == false & x.Title.ToLower().Contains("t") == false));
            foreach (MovieCollection.WaltDisneyMovies e in q10)
            {
                Console.WriteLine(e.Title);
            }

            Console.Write("11 - Calculate the mean of all Budget / Box Office of every movie ever : ");
            var q11 = listMovie.Sum(x => x.Budget)/listMovie.Sum(x => x.BoxOffice);
            Console.WriteLine(q11);

            Console.Write("12 - Group all films by the number of characters in the title screen and print the count of movies by letter in the film : ");
            var q12 = from movie in listMovie
                let charCount = movie.Title.Length
                group  movie.Title by charCount into charCountGroup
                orderby charCountGroup.Key
                select charCountGroup;

            foreach (var e in q12)
            {
                var nchar = 0;
                foreach (var item in e)
                {
                    nchar += 1;
                }
                Console.WriteLine(e.Key + " char => " + nchar + " films");
                
            }
            Threading();

        } 
        private static Mutex mut = new Mutex();
        private static int cpt1 = 0;
        private static int cpt2 = 0;
        private static int cpt3 = 0;

        static void Threading()
        {
            Thread T1 = new Thread(T1Proc);
            Thread T2 = new Thread(T2Proc);
            Thread T3 = new Thread(T3Proc);
            T1.Start();
            T2.Start();
            T3.Start();
        }

        private static void T1Proc()
        {
            while (cpt1 < 10000)
            {
                PrintParameter(" ");
                cpt1 += 50;
                Thread.Sleep(50);
            }
        }
        
        private static void T2Proc()
        {
            while (cpt2 < 11000)
            {
                PrintParameter("*");
                cpt2 += 40;
                Thread.Sleep(40);
            }
        }
        
        private static void T3Proc()
        {
            while (cpt3< 9000)
            {
                PrintParameter("°");
                cpt3 += 20;
                Thread.Sleep(20);
            }
        }

        public static void PrintParameter(string parameter)
        {
            mut.WaitOne();
            Console.Write(parameter);
            mut.ReleaseMutex();
        } 
    }
}