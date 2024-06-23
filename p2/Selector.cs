using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector FromQueryString(string queryString)
        {
            HtmlHelper htmlHelper = HtmlHelper.SingleHelper;
            string[] parts = queryString.Split(' ');
            Selector rootSelector = new Selector();

            Selector currentSelector = rootSelector;
           
            


            // Assume the query string is in the format "tag#id.class1.class2.class3"
            
                

                for (int j = 0; j < parts.Length; j++)
                {
                var partOfPart = parts[j].Split('#', '.');
                foreach (var part in partOfPart)
                {
                   
                    if (queryString.Contains($"#{part}"))
                    {
                        // Extracting Id
                        currentSelector.Id = part;
                    }
                    else if (queryString.Contains($".{part}"))
                    {
                        // Extracting Classes
                        currentSelector.Classes.Add(part);
                    }
                    else if (htmlHelper.arrTags.Contains(part) || htmlHelper.NoClosarrTags2.Contains(part))
                    {
                        currentSelector.TagName = part;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (j < parts.Length-1)
                {
                    Selector selector = new Selector();
                    selector.Parent = currentSelector;
                    currentSelector.Child = selector;
                    currentSelector = selector;
                }

            }
              



            return rootSelector;
        }
        public bool HasChildSelector()
        {
            return Child != null;
        }//Checking whether the selector has a child

        public bool Match(HtmlElement element) // Check if the element matches the criteria of the selector
        {
            bool flag=true;
            
            for (int i = 0;i<Classes.Count;i++)
            {
                if (!element.Classes.Contains(Classes[i]))
                {
                   flag = false;
                }
            }
           if (TagName == null || element.Name == TagName)
            {
                flag= true;
            }
            else
            {
                flag= false;
            }
           return flag;
        }
    }
}
