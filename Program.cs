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

        //Handle blog creation
        public static void CreateBlog()
        {
            // Create and save a new Blog
            while (true)
            {
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                //If entered name is blank, give error
                if (name == "")
                {
                    logger.Error("Blog name can not be null!");
                    Console.WriteLine("Please enter a name for the blog!");
                }
                else
                {
                    var blog = new Blog { Name = name };
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);
                    break;
                }
            }

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

                string[] findBlog = navMenu.CreatePostMenuSelection();

                //searches for blog to post to
                if(findBlog[0] == "1")
                {
                    var query = db.SearchBlogs(findBlog[1]);

                    //If only one result is found. Store the blog ID and continue 
                    if(query.Count() == 1)
                    {

                        db.DisplayBlogs(query);
                        if (navMenu.CorrectBlogCheck())
                        {
                            bID = query[0].BlogId;
                        }
                        else
                        {
                            goto postsRetry;
                        }
                    }
                    //If None are found, go back.
                    else if(query.Count == 0)
                    {
                        Console.WriteLine("No blogs found. Please try again");
                        goto postsRetry;
                    }
                    //select from a list and store blog id.
                    else
                    {
                        db.DisplayBlogs(query);

                        int choice = navMenu.MultipleBlogSelection(query);

                        if (choice == -1)
                        {
                            goto postsRetry;
                        }
                        else
                        {
                            bID = query[choice].BlogId;
                        }
                    }
                }
                //Enter the blog id to post to
                else if(findBlog[0] == "2")
                {
                    //if the blog ID is invalid jump back up to try again
                    if (!int.TryParse(findBlog[1], out bID))
                    {
                        logger.Error("Entry not valid. Make sure the entry is a number. Please try again");
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

                    Post newPost = db.AddPost(bID);
                    logger.Info("Post added - {name}", newPost.Title);
                }
                else
                {
                    logger.Error("The blog ID was not found.");
                    Console.WriteLine("Please try again");
                    goto postsRetry;
                }

            }
            else if (keyPress.Key == ConsoleKey.D4 || keyPress.Key == ConsoleKey.NumPad4)
            {
                try
                {
                    //prompts for display of all blogs and post or specific ones.
                    string[] decision = navMenu.DisplayPostsMenu();

                    if (decision[0] == "all")
                    {
                        db.DisplayAllBlogPosts();
                    }
                    else if (decision[0] == "one")
                    {
                        var blogList = db.SearchBlogs(decision[1]);

                        if (blogList.Count() == 0)
                        {
                            logger.Error("No blogs found");
                        }
                        else
                        {
                            db.DisplayBlogPosts(blogList);
                        }
                    }
                    else if (decision[0] == "id")
                    {
                        if(int.TryParse(decision[1], out int bID))
                        {
                            if (db.Verify(bID))
                            {
                                db.DisplayBlogPosts(bID);
                            }
                            else
                            {
                                logger.Error("The blog ID was not found.");
                            }

                        }
                        else
                        {
                            logger.Error("Blog ID entered is not valid.");
                        }

                    }
                    else
                    {
                        goto dropback;
                    }

                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
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

                //loop menu until escape is pressed
                while (true)
                {

                    ConsoleKeyInfo keyPress = menu.TopMenuSelection();

                    if (keyPress.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {
                        //Go to navigation
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
