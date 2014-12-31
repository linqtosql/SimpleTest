using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;

namespace TestRazor
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = Razor.Parse("<h2>这是视图内容 @Model.Name</h2>", new { Name = "张三" });
        }
    }
}
