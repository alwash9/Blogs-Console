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


            Console.WriteLine("\nAll blogs in the database: {0} found", query.Count());

            Console.WriteLine($"{"Blog ID",-10}Blog Name\n");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId, -10}{item.Name}\n");
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

        //Search for a blog name
        public List<Blog> SearchBlogs (string searchName)
        {
            var searchResult = this.Blogs.Where(b => b.Name.Contains(searchName)).ToList();
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

        public void DisplayAllPosts()
        {
            var blogs = this.Blogs.OrderBy(b => b.BlogId);

            foreach (var item in blogs)
            {
                var posts = this.Posts.OrderBy(p => p.PostId).Where(p => p.BlogId == item.BlogId);

                Console.WriteLine($"{item.BlogId, 15}{item.Name, 15}");

                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.PostId, 10}{post.Title, 10}");
                    Console.WriteLine($"{post.Content, -5}");
                }

            }

            Console.WriteLine("\nEND OF LIST!");

        }

    }
}
