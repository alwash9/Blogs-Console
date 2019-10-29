using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsConsole.Traversal
{
    public class Menu
    {

        //In order to have everything loop, I have to put this back in the main

        //public ConsoleKeyInfo TopMenuSelection()
        //{
        //    Console.WriteLine("Please Choose an option.");
        //    Console.WriteLine("Press 1 to view the list of blogs");
        //    Console.WriteLine("Press 2 to create a blog");
        //    Console.WriteLine("Press 3 to create a post");

        //    ConsoleKeyInfo keyPress = Console.ReadKey();

        //    Console.WriteLine("");

        //    return keyPress;

        //}

        //Menu for making a post. Returns an array with a number for which option was chosen and either the name to search or the blog ID to post to.
        public string[] PostMenuSelection()
        {
            Console.WriteLine("Would you like to find a Blog to post to (1)\nOr enter the Blog ID if you know it (2)\nAny other key will take you to the previous menu.");
            ConsoleKeyInfo keyPress = Console.ReadKey();
            Console.WriteLine("");

            if (keyPress.Key == ConsoleKey.D1 || keyPress.Key == ConsoleKey.NumPad1)
            {
                Console.WriteLine("What is the name of the Blog that you are looking for?");
                string searchName = Console.ReadLine();

                string[] search = { "1", searchName };
                return search;
            }
            else if (keyPress.Key == ConsoleKey.D2 || keyPress.Key == ConsoleKey.NumPad2)
            {
                Console.WriteLine("Please enter the Blog ID that you would like to post to.");
                string blogID = Console.ReadLine();
                string[] known = { "2", blogID };
                return known;
            }
            else
            {
                string[] blank = { "0", "" };
                return blank;
            }
        }

        public string[] displayPostsMenu()
        {
            Console.WriteLine("Display all blogs and posts (1) \nDisplay posts for a specific blog (2) \nAny other key will take you back");
            ConsoleKeyInfo keyPress = Console.ReadKey();

            if (keyPress.Key == ConsoleKey.D1 || keyPress.Key == ConsoleKey.NumPad1)
            {
                string[] dAll = { "all", "" };
                return dAll;
            }
            else if (keyPress.Key == ConsoleKey.D2 || keyPress.Key == ConsoleKey.NumPad2)
            {
                Console.WriteLine("\nWhat is the name of the blog that you want to view posts for?");
                string blog = Console.ReadLine();

                string[] dOne = { "one", blog };
                return dOne;

            }
            else
            {
                string[] dNone = { "none", "" };
                return dNone;
            }
        }
    }
}
