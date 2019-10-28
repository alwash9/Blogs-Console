using NLog;
using BlogsConsole.Models;
using BlogsConsole.Traversal;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static BloggingContext db = new BloggingContext();


        public static void CreateBlog()
        {
            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();
            var blog = new Blog { Name = name };
            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name); 
        }

        //Attempt at organizing the code better. Top menu was meant to send key press here in an attempt at abstraction.
        public static void Navigation(ConsoleKeyInfo keyPress)
        {
            //Another attempt at better organization. 
            //Currently holds some of the menu options for posting a post.
            Menu navMenu = new Menu { };

            //display blog list
            if (keyPress.Key == ConsoleKey.D1 || keyPress.Key == ConsoleKey.NumPad1)
            {
                db.DisplayBlogs();
            }
            //create blog
            else if (keyPress.Key == ConsoleKey.D2 || keyPress.Key == ConsoleKey.NumPad2)
            {
                CreateBlog();
            }
            //create post
            else if (keyPress.Key == ConsoleKey.D3 || keyPress.Key == ConsoleKey.NumPad3)
            {
                //Start out displaying list of blogs.
                Console.WriteLine($"{"",-10}BLOG LIST{"",10}");
                db.DisplayBlogs();

                //user entered blog ID
                int bID = -1;

            //label for retrying. alternative to infinite looping
            postsRetry:

                string[] findBlog = navMenu.PostMenuSelection();

                //searches for blog and gives opportunity to enter blog ID
                if(findBlog[0] == "1")
                {
                    var query = db.SearchBlogs(findBlog[1]);

                    //If only one result is found. Store the blog ID and continue 
                    if(query.Count() == 1)
                    {

                        Console.WriteLine($"{query[0].BlogId,-10}{query[0].Name}\n");
                        bID = query[0].BlogId;
                    }
                    else if(query.Count == 0)
                    {
                        Console.WriteLine("No blogs found. Please try again");
                        goto postsRetry;
                    }
                    else
                    {
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId,-10}{item.Name}\n");
                        }

                        Console.WriteLine("Multiple results found. Enter the blog ID to post to (1) or press any other key to go back");
                        if (keyPress.Key == ConsoleKey.D1 || keyPress.Key == ConsoleKey.NumPad1)
                        {
                            while (true)
                            {
                                Console.WriteLine("What is the blog ID?");
                                string inputID = Console.ReadLine();
                                Console.WriteLine("");
                                if (!int.TryParse(inputID, out bID))
                                {
                                    Console.WriteLine("Entry not valid. Try again.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Go to end of method to loop back
                            goto dropback;
                        }
                    }
                }
                else if(findBlog[0] == "2")
                {
                    //if the blog ID is invalid jump back up to try again
                    if (!int.TryParse(findBlog[1], out bID))
                    {
                        Console.WriteLine("Entry not valid. Please try again");
                        goto postsRetry;
                    }
                }
                else
                {
                    goto dropback;
                }

                //verifies that the blog ID exists in the database
                if(db.Verify(bID))
                {

                    Console.WriteLine("What is the title of your post?");
                    string title = Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("Enter your post's content.");
                    string content = Console.ReadLine();
                    Console.WriteLine("");

                    var post = new Post { BlogId = bID, Title = title, Content = content };

                    db.AddPost(post);
                    logger.Info("Post added - {name}", post.Title);
                }
                else
                {
                    logger.Error("The blog ID was not found.");
                    Console.WriteLine("Please try again");
                    goto postsRetry;
                }

            }
            //If a non-existant menu option was entered
            else
            {
                logger.Warn("A valid option was not entered.");
                Console.WriteLine("Please enter a valid option.");
            }
        //label to travel to end of method in order to loop back to the top menu.
        dropback:;
        }
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                Menu menu = new Menu { };

                //Top menu
                while (true)
                {
                    Console.WriteLine("Please Choose an option.");
                    Console.WriteLine("Press 1 to view the list of blogs");
                    Console.WriteLine("Press 2 to create a blog");
                    Console.WriteLine("Press 3 to create a post");

                    Console.WriteLine("Press the ESC key to exit");

                    ConsoleKeyInfo keyPress = Console.ReadKey();
                    Console.WriteLine("");

                    if (keyPress.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {
                        Navigation(keyPress);
                    }

                    
                }

                Console.WriteLine("Press any key to end");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
