using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace p2
{
    public class HtmlElement
    {


        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Classes { get; set; }
        public List<string> Attributes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Classes = new List<string>();
            Attributes = new List<string>();
            Children = new List<HtmlElement>();
        }



        public static HtmlElement Serializer(IEnumerable<string> htmlStrings)
        {


            HtmlHelper htmlHelper = HtmlHelper.SingleHelper;

            HtmlElement root = new HtmlElement();
            root.Name = "root";

            HtmlElement currentElement = root;



            foreach (var htmlString in htmlStrings)
            {
                string sentences = htmlString.ToString();
                string[] words = htmlString.Split(' ');
                var firstWord = words[0];

                if (firstWord.StartsWith("html/"))
                {

                    break;
                }
                else if (firstWord.StartsWith("/"))
                {



                    currentElement = currentElement.Parent != null ? currentElement.Parent : root;

                }
                else if (htmlHelper.arrTags.Contains(firstWord) || htmlHelper.NoClosarrTags2.Contains(firstWord))
                {
                    HtmlElement newElement = new HtmlElement();
                    currentElement.Children.Add(newElement);
                    var attributes = new List<string>();

                    var attributesMatch = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlString);
                    foreach (var oneAtrribute in attributesMatch)
                    {
                        newElement.Attributes.Add(oneAtrribute.ToString());
                        if (oneAtrribute.ToString().ToLower().StartsWith("class"))
                        {
                            string[] classes = oneAtrribute.ToString().Replace("class=", "").Replace("\"", "").Split(' ');
                            foreach (var className in classes)
                            {
                                if (className != "class" && className != "=")
                                {
                                    newElement.Classes.Add(className);
                                }
                            }
                        }
                        if (oneAtrribute.ToString().ToLower().StartsWith("id"))
                        {
                            newElement.Id = oneAtrribute.ToString().Replace("id=", "").Replace("\"", "");
                        }

                    }
                    newElement.Name = firstWord;
                    newElement.Parent = currentElement;
                    currentElement = newElement;

                    if (firstWord.EndsWith("/") || htmlHelper.NoClosarrTags2.Contains(firstWord))
                    {
                        currentElement = newElement;
                    }
                }
                else
                {
                    currentElement.InnerHtml = htmlString;
                }

            }

            return root;


        }//builds the tree

        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {

            HashSet<HtmlElement> resultSet = FindElementsRecursive(this, selector);

            return resultSet;
        }// return a list of elements that meet the criteria of the selector​

        private HashSet<HtmlElement> FindElementsRecursive(HtmlElement currentElement, Selector selector)
        {
            HashSet<HtmlElement> resultSet = new HashSet<HtmlElement>();
            if (currentElement == null || selector == null)
            {
                return resultSet;
            }

            var elements = currentElement.Descendants<HtmlElement>(n => n.Children);

            foreach (var descendant in elements)
            {
                if (!string.IsNullOrEmpty(selector.Id) && selector.Id != descendant.Id)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != descendant.Name)
                {
                    continue;
                }

                if (selector.Classes.Count > 0)
                {
                    selector.Classes.ForEach(x =>
                    {
                        if (descendant.Classes.Contains(x))
                        {

                            resultSet.Add(descendant);
                        }
                    });
                }
                else
                {
                    if (selector.Id != null || selector.TagName != null)
                    {
                        resultSet.Add(descendant);
                    }
                }
            }

            if (resultSet.Count > 0 && selector.Child != null)
            {
                foreach (var res in resultSet)
                {
                    resultSet = FindElementsRecursive(res, selector.Child);
                }
            }
            else
            {
                return resultSet;
            }

            return resultSet;

        }//Compares and if they match adds to the HashSet
    }
}
