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

        public void DisplayBlogs()
        {
            // Display all Blogs from the database
            var query = this.Blogs.OrderBy(b => b.BlogId);

            Console.WriteLine("All blogs in the database:");

            Console.WriteLine($"{"Blog ID",-10}Blog Name\n");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId, -10}{item.Name}\n");
            }

        }

        //Add a post
        public void AddPost(Post post)
        {
            this.Posts.Add(post);
            this.SaveChanges();
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

    }
}
