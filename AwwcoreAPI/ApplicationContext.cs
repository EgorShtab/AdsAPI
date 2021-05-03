using AwwcoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwwcoreAPI
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<PhotoLink> PhotoLinks { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> contextOptions) :base(contextOptions)
        {

        }
    }
}
