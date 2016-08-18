using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TwitterSentimentsCore.Models
{
    public class Request
    {
        // Primary key
        public int Id { get; set; }

        // Form inputs
        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; set; }
        public int Count { get; set; }

        [Display(Name = "Score")]
        public double Result { get; set; }

        // Auto timestamp
        [Display(Name = "Date Created")]
        public DateTime Timestamp { get; } = DateTime.Now;
    }
    //https://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html
    public class RequestDbContext : DbContext
    {
        public RequestDbContext(DbContextOptions<RequestDbContext> options) : base(options) { }

        public DbSet<Request> Requests { get; set; }
    }
}