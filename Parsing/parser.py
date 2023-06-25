import json
from bs4 import BeautifulSoup
import time
import aiohttp
import asyncio
import platform

finish_data = []
headers = {
    "accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
    "user-agent": "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36"
}

async def get_page(session,url,page,data):
    newurl = url+f'?page={page}'
    async with session.get(newurl) as response:    
        soup = BeautifulSoup(await response.text(), "lxml")
        for l2 in soup.find(id="products").find_all("article"):
            try:
                price = l2.find(class_="price").text
                title = l2.find(class_="product-title").text
                link = l2.find('a').get('href')
                img = l2.find('img').get('src')
                data.append({
                    "title":title.split("-")[0],
                    "price":price.split("\\")[0],
                    "link":link,
                    "img":img
                })
                print(price.split("\\")[0])
            except Exception as ex:
                print(ex)

async def pages_data(session,url):
    tasks = []
    data = []
    for i in range(1,21):
        tasks.append(asyncio.create_task(get_page(session=session,url=url,page=i,data=data)))
    await asyncio.gather(*tasks)
    return data

async def get_page_data():
    url = "https://riff.net.pl/1863-gitarybasy"

    async with aiohttp.ClientSession() as session:
        async with session.get(url=url, headers=headers) as response:
            response_text = await response.text()
            bs = BeautifulSoup(response_text, 'lxml')
            for l in bs.find(id="subcategories").find_all('li'):
                a_title = l.find(class_="subcategory-name")
                print(a_title.text)
                print(a_title.get('href'))
                data = await pages_data(url=a_title.get('href'),session=session)
                finish_data.append({
                    "title":a_title.text,
                    "image":l.find('img').get('src'),
                    "all":data
                })


async def main():
    stroi_url = "https://riff.net.pl"
    await get_page_data()
    with open("Parsing_5/data.json","w",encoding="utf-8") as js:
        json.dump(finish_data,js,indent=4,ensure_ascii=False)
    print(finish_data)
        
    
    
if __name__ == '__main__':
    if platform.system()=='Windows':
        asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())
    asyncio.run(main())
    