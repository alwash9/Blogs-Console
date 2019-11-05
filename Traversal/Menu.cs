using BlogsConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsConsole.Traversal
{

    public class Menu
    {

        //Top menu
        public ConsoleKeyInfo TopMenuSelection()
        {
            Console.WriteLine("Please Choose an option.");
            Console.WriteLine("Press 1 to view the list of blogs");
            Console.WriteLine("Press 2 to create a blog");
            Console.WriteLine("Press 3 to create a post");
            Console.WriteLine("Press 4 to view posts");

            Console.WriteLine("\nPress the ESC key to exit");

            ConsoleKeyInfo keyPress = Console.ReadKey();
            Console.WriteLine("");

            return keyPress;


        }

        //Menu for making a post. Returns an array with a number for which option was chosen and either the name to search or the blog ID to post to.
        //Or nothing in order to go to previous menu.
        public string[] CreatePostMenuSelection()
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

        //Menu to display post. 
        public string[] DisplayPostsMenu()
        {
            Console.WriteLine("Display all blogs and posts (1) \nDisplay posts for a specific blog (2) \nAny other key will take you back");
            ConsoleKeyInfo keyPress = Console.ReadKey();
            Console.WriteLine("\n");

            if (keyPress.Key == ConsoleKey.D1 || keyPress.Key == ConsoleKey.NumPad1)
            {
                string[] dAll = { "all", "" };
                return dAll;
            }
            else if (keyPress.Key == ConsoleKey.D2 || keyPress.Key == ConsoleKey.NumPad2)
            {
                while (true)
                {

                    Console.WriteLine("Search by name (1), or enter the blog ID (2)");

                    var searchChoice = Console.ReadKey();

                    if (searchChoice.Key == ConsoleKey.D1 || searchChoice.Key == ConsoleKey.NumPad1)
                    {
                        Console.WriteLine("\nWhat is the name of the blog that you want to view posts for?");
                        string blog = Console.ReadLine();

                        string[] dOne = { "one", blog };
                        return dOne;
                    }
                    else if (searchChoice.Key == ConsoleKey.D2 || searchChoice.Key == ConsoleKey.NumPad2)
                    {
                        Console.WriteLine("\nWhat is the blog ID you want to view posts for?");
                        string blog = Console.ReadLine();

                        string[] dID = { "id", blog };
                        return dID;
                    }
                    else
                    {
                        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                        logger.Warn("Please press either 1 or 2.");
                    }
                }


            }
            else
            {
                string[] dNone = { "none", "" };
                return dNone;
            }
        }

        public bool CorrectBlogCheck()
        {
            Console.WriteLine("\nIs this the correct blog.");
            Console.WriteLine("If yes, press y otherwise press any other key to go back.");

            var keypress = Console.ReadKey();
            Console.WriteLine("");
            if(keypress.Key == ConsoleKey.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int MultipleBlogSelection(List<Blog> query)
        {
            
            int tally = 0;

            Console.WriteLine("{0} results found. Which blog would you like to select?\n", query.Count);

            foreach (var blog in query)
            {
                Console.WriteLine("({0}) {1}", ++tally, blog.Name);
            }

            Console.WriteLine("\nOr enter \"Back\" to go back.");

            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            while (true)
            {
                string choice = Console.ReadLine();


                if (int.TryParse(choice, out int blogPick))
                {
                    if(query[blogPick - 1] != null)
                    {
                        return blogPick - 1;
                    }
                    else
                    {
                        logger.Warn("Please enter a selection from the list.");
                    }
                }
                else if (choice.ToUpper() == "BACK")
                {
                    return -1;
                }
                else
                {
                    logger.Warn("Please enter a valid selection, either a listed number or \"Back\"");
                }
            }
        }

    }
}
