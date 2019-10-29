using System.Data.Entity;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole.Models
{
    public class BloggingContext : DbContext
    {
        public BloggingContext() : base("name=BlogContext") { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void AddBlog(Blog blog)
        {
            this.Blogs.Add(blog);
            this.SaveChanges();

        }

        // Display all Blogs from the database
        public void DisplayBlogs()
        {

            var query = this.Blogs.OrderBy(b => b.BlogId);

            if(query.Count() == 0)
            {
                Console.WriteLine("No blogs exist in database.\n");
            }
            else
            {
                Console.WriteLine("\nAll blogs in the database: {0} found", query.Count());

                Console.WriteLine($"{"Blog ID",-10}Blog Name\n");
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.BlogId,-10}{item.Name}\n");
                }

            }


        }

        //Displays a passed in list of blogs
        public void DisplayBlogs(List<Blog> displayList)
        {
            string plural;
            if (displayList.Count() == 1)
            {
                plural = "blog";
            }
            else
            {
                plural = "blogs";
            }

            Console.WriteLine("\n{0} {1} found", displayList.Count(), plural);

            Console.WriteLine($"{"Blog ID",-10}Blog Name\n");
            foreach (var item in displayList)
            {
                Console.WriteLine($"{item.BlogId,-10}{item.Name}\n");
            }

        }

        //Search for a blog name returns a list
        public List<Blog> SearchBlogs (string searchName)
        {
            var searchResult = this.Blogs.Where(b => b.Name.Contains(searchName)).ToList();
            return searchResult;
        }

        //Search for a blog name returns a IQueryable
        public IQueryable<Blog> SearchBlogs(string searchName, bool x)
        {
            var searchResult = this.Blogs.Where(b => b.Name.Contains(searchName));
            return searchResult;
        }


        //Check if the blog id entered is in the database
        public bool Verify(int blogID)
        {
            var query = this.Blogs.OrderBy(b => b.BlogId);
            return query.Any(b => b.BlogId == blogID);

        }

        //Add a post
        public Post AddPost(int blogID)
        {
            Console.WriteLine("What is the title of your post?");
            string title = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Enter your post's content.");
            string content = Console.ReadLine();
            Console.WriteLine("");

            var post = new Post { BlogId = blogID, Title = title, Content = content };

            this.Posts.Add(post);
            this.SaveChanges();

            //Returning for logger
            return post;
        }

        public void DisplayAllBlogPosts() //Would like to make blogs an optional parameter, but because of LINQ's compile time stuff, I can't.
        {
            var blogs = this.Blogs.OrderBy(b => b.BlogId).ToList();
            Console.WriteLine("{0} blog/s in the database\n", blogs.Count());

            foreach (var item in blogs)
            {
                var posts = this.Posts.OrderBy(p => p.PostId).Where(p => p.BlogId == item.BlogId).ToList();

                Console.WriteLine($"BLOG: {item.BlogId}{"",5}{item.Name}");
                if (posts.Count() == 0)
                {
                    Console.WriteLine("NO POST WERE MADE TO THIS BLOG YET.\n");
                }


                foreach (var post in posts)
                {
                    Console.WriteLine($"POST: {post.PostId}{"", 5}{post.Title}");
                    Console.WriteLine($"{"", 2}{post.Content, 5}\n");
                }

            }

            Console.WriteLine("\nEND OF LIST!\n");

        }

        public void DisplayBlogPosts(List<Blog> blogs)
        {
            if(blogs.Count() != 1)
            {
                Console.WriteLine("{0} blogs found\n", blogs.Count());
            }
            foreach (var item in blogs)
            {
                var posts = this.Posts.OrderBy(p => p.PostId).Where(p => p.BlogId == item.BlogId).ToList();

                Console.WriteLine($"BLOG: {item.BlogId}{"",5}{item.Name}");
                if (posts.Count() == 0)
                {
                    Console.WriteLine("NO POST WERE MADE TO THIS BLOG YET.\n");
                }


                foreach (var post in posts)
                {
                    Console.WriteLine($"POST: {post.PostId}{"",5}{post.Title}");
                    Console.WriteLine($"{"",2}{post.Content,5}\n");
                }

            }
            Console.WriteLine("\n");
        }

    }
}
