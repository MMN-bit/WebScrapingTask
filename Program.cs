using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
public class ProductP 
{ 
	public string? Name { get; set; } 
	public string? Price { get; set; } 
    public string? Rating { get; set; } 
   
}

public class Urls
{
    public string? Url { get; set; }
}

class ScrapeP
	{ 
		static void Main(string[] args) 
		{ 
            var document = new HtmlDocument();
            document.Load("test.html");
            var products = new List<ProductP>();
            var souldout = new List<ProductP>();
            var urls = new List<Urls>();
            var productHTMLElements = document.DocumentNode.QuerySelectorAll("div.item");
 
			foreach(var productElement in productHTMLElements)
            {   
                var name = HtmlEntity.DeEntitize(productElement.QuerySelector("img").Attributes["alt"]?.Value);
                var price = HtmlEntity.DeEntitize(productElement.QuerySelector("span.dollars").InnerText) +  HtmlEntity.DeEntitize(productElement.QuerySelector("span.cents").InnerText) ;
                var rating = HtmlEntity.DeEntitize(productElement.QuerySelector("div.item")?.Attributes["rating"]?.Value);
                var url = HtmlEntity.DeEntitize(productElement.QuerySelector("a")?.Attributes["href"]?.Value);
                Double.TryParse(rating, out double normalize);
                // Math equation to normalize the rating
                if(normalize > 5){
                    normalize = (normalize - 1) / (10 - 1) * (5 - 1) + 1;
                    rating = normalize.ToString("#.0");
                }
                // Sold out check
                if(productElement.QuerySelector(".sold-out") != null)
                {
                    souldout.Add(new ProductP
                    {
                        Name = name,
                        Price = price,
                        Rating = rating,
                    });
                    continue;
                }
                
                products.Add(new ProductP
                {
                    Name = name,
                    Price = price,
                    Rating = rating,
                
                });

                urls.Add(new Urls
                {
                    Url = url
                });
            }

            Console.WriteLine("Products:");
            foreach(var product in products)
            {
                Console.WriteLine($"name: {product.Name},{Environment.NewLine}price: {product.Price},{Environment.NewLine}rating: {product.Rating},{Environment.NewLine}");
            }
            
            Console.WriteLine("URLs for available products:");
            foreach(var url in urls)
            {
                Console.WriteLine($"{url.Url}{Environment.NewLine}");
            }

            Console.WriteLine("Sold-out Products:");
            foreach(var product in souldout)
            {
                Console.WriteLine($"name: {product.Name},{Environment.NewLine}price: {product.Price},{Environment.NewLine}rating: {product.Rating}{Environment.NewLine}");
            }
            
		} 
	}
