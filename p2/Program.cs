using p2;
using System.Text.RegularExpressions;

HttpClient httpClient = new HttpClient();
var url = await Load("https://hebrewbooks.org/advanced");
var url1 = "<div id=\"first1\" onclick=\"aaa()\" class=\"allContent class2\">hellow</div><div><h1 id=\"first1\"><p class=\"allContent class1\">hello1</p></h1></div>";

var cleanHtml = new Regex("\\s").Replace(url, " ");
var htmlLines=new Regex("<(.*?)>").Split(cleanHtml).Where(s=>s.Length>0);
HtmlElement tree = new HtmlElement();
tree=HtmlElement.Serializer(htmlLines);

var selector1 = Selector.FromQueryString(".allContent");
var selector2 = Selector.FromQueryString("#first1 .allContent");
var selector3 = Selector.FromQueryString("div #first1 .allContent");
var selector4 = Selector.FromQueryString("div#first1.allContent");
var selector5 = Selector.FromQueryString("#first1");
var selector6 = Selector.FromQueryString("div");
var selector7 = Selector.FromQueryString("#Table1");
var selector8 = Selector.FromQueryString("div#Table1");



var resultSelector1 = tree.FindElements(selector1);
var resultSelector2 = tree.FindElements(selector2);
var resultSelector3 = tree.FindElements(selector3);
var resultSelector4 = tree.FindElements(selector4);
var resultSelector5 = tree.FindElements(selector5);
var resultSelector6 = tree.FindElements(selector6);
var resultSelector7= tree.FindElements(selector7);
var resultSelector8 = tree.FindElements(selector8);


Console.ReadLine();
static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}