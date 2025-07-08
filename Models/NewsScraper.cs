using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack; // Asigură-te că ai instalat pachetul NuGet HtmlAgilityPack

namespace NewsScraperApp.Models // Ajustează namespace-ul la cel al proiectului tău
{
    public class NewsScraper
    {
        /// <summary>
        /// Extrage titlurile știrilor de pe o pagină web dată, folosind web scraping.
        /// </summary>
        /// <param name="url">URL-ul paginii web de la care se extrag titlurile.</param>
        /// <param name="xpathSelector">Selectorul XPath pentru elementele HTML care conțin titlurile.</param>
        /// <returns>O listă de șiruri de caractere reprezentând titlurile știrilor.</returns>
        public static async Task<List<string>> GetNewsTitlesFromHtml(string url, string xpathSelector)
        {
            List<string> newsTitles = new List<string>();

            try
            {
                // Creează o instanță de HttpClient pentru a face cereri HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Setează un header User-Agent pentru a imita un browser web.
                    // Unele site-uri pot bloca cererile fără un User-Agent adecvat.
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.88 Safari/537.36");

                    // Preluarea conținutului HTML al URL-ului țintă
                    string html = await client.GetStringAsync(url);

                    // Creează un obiect HtmlDocument nou din HtmlAgilityPack
                    HtmlDocument doc = new HtmlDocument();
                    // Încarcă HTML-ul preluat în HtmlDocument
                    doc.LoadHtml(html);

                    // --- PARTEA CRITICĂ: IDENTIFICAREA ELEMENTELOR HTML CORECTE ---
                    // Trebuie să inspectezi structura HTML a site-ului țintă (folosind DevTools-ul browserului, F12)
                    // pentru a găsi selectorii XPath exacți pentru titlurile știrilor.
                    // Exemplu: Dacă titlurile sunt în interiorul tag-urilor <h3> cu o clasă specifică, de ex., <h3 class="news-title">
                    // xpathSelector ar putea fi "//h3[@class='news-title']"
                    // Sau dacă sunt link-uri <a> copii ai unui <div class="article-item">: "//div[@class='article-item']/a"

                    var titleNodes = doc.DocumentNode.SelectNodes(xpathSelector);

                    // Dacă nu s-au găsit noduri, titleNodes va fi null
                    if (titleNodes != null)
                    {
                        // Iterăm prin fiecare nod HTML găsit
                        foreach (var node in titleNodes)
                        {
                            // Extragem textul intern al nodului și eliminăm spațiile albe de la început/sfârșit
                            string title = node.InnerText.Trim();
                            if (!string.IsNullOrEmpty(title))
                            {
                                newsTitles.Add(title);
                            }
                        }
                    }
                    else
                    {
                        // Dacă niciun nod nu a corespuns selectorului XPath
                        newsTitles.Add("Nu s-au găsit elemente corespunzătoare selectorului specificat. Vă rugăm să verificați structura HTML a site-ului și selectorul XPath.");
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Gestionarea erorilor specifice HTTP (ex: probleme de rețea, 404, 500)
                newsTitles.Add($"Eroare cerere HTTP: {httpEx.Message}. Verificați dacă URL-ul este corect și accesibil.");
            }
            catch (Exception ex)
            {
                // Gestionarea oricăror alte excepții generale
                newsTitles.Add($"A apărut o eroare neașteptată: {ex.Message}");
            }

            return newsTitles;
        }
    }
}
