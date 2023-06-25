namespace ShopAPI.Parsing
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ShopAPI.Models;
    public static class StaticInjectionData
    {
        public static void InjectToDb(DatabaseContext db)
        {
            // read JSON directly from a file
            using (StreamReader file = File.OpenText(@"Parsing/data.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JArray o2 = (JArray)JToken.ReadFrom(reader);
                foreach (var o in o2)
                {
                    GuitarCategory guitarCategory = new GuitarCategory();
                    foreach (JProperty item in (JToken)o)
                    {
                        if (item.Name == "title")
                        {
                            guitarCategory.Name = item.Value.ToString();
                        }
                        if (item.Name == "image")
                        {
                            guitarCategory.ImageUrl = item.Value.ToString();
                        }
                        if (item.Name == "all")
                        {
                            foreach (var item2 in (JArray)item.Value)
                            {
                                Guitar guitar = new Guitar();
                                guitar.Category = guitarCategory;
                                foreach (JProperty it in (JToken)item2)
                                {
                                    if (it.Name == "title")
                                    {
                                        guitar.Name = it.Value.ToString();
                                    }
                                    if (it.Name == "price")
                                    {
                                        guitar.Price = it.Value.ToString();
                                    }
                                    if (it.Name == "img")
                                    {
                                        guitar.ImgUrl = it.Value.ToString();
                                    }
                                    if (it.Name == "link")
                                    {
                                        guitar.Link = it.Value.ToString();
                                    }
                                    Console.WriteLine(it.Value);
                                }
                                db.Guitars.Add(guitar);
                            }
                        }
                    }
                    db.GuitarCategories.Add(guitarCategory);
                }
            }
            db.SaveChanges();
        }
    }
}
