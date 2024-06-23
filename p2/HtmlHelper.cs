using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Channels;

namespace p2
{
  
    public static class HtmlExtensions
    {
        public static IEnumerable<T> Descendants<T>(this T tree, Func<T, IEnumerable<T>> getChildren)
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(tree);

            while (queue.Count > 0)
            {
                T currentElement = queue.Dequeue();
                yield return currentElement;


                // Enqueue children for further exploration
                foreach (var child in getChildren(currentElement))
                {
                    queue.Enqueue(child);
                }
            }
        }//Returns the whole tre

       //Retur
        public static IEnumerable<HtmlElement> Ancestors(this HtmlElement elementTree)
        {
            HtmlElement currentElement = elementTree;

            while (currentElement.Parent != null)
            {
                yield return currentElement.Parent;
                currentElement = currentElement.Parent;
            }
        }//returns the ancestors of the element
    }
    public  class HtmlHelper 
    {
        private readonly static HtmlHelper _htmlHelper=new HtmlHelper();
        public static HtmlHelper SingleHelper { get { return _htmlHelper; } }
        public string[] arrTags { get; set; }
        public string[] NoClosarrTags2 { get; set; }

        
        
        private HtmlHelper()
        {
            arrTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlTags.json"));
            NoClosarrTags2 = JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlVoidTags.json"));
           
        }
       
    }
}
